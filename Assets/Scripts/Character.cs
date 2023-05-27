using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Unity.Barracuda;

[Serializable]
public struct Observation
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3[] enemyPositions, allyPositions;
    public int ammoLeft, heLeft, stunLeft, team, index;
    public float hp;
}

public class Character : Agent
{
    public float hp, points, fovAngle, waitTime;
    public int ammoLeft, heLeft, stunLeft, team, index, inBombSite, bombInRange = 0, isAlive = 1, counter = 0;
    public Movement movement;
    public EquipmentManager equipmentManager;
    public List<Character> allies, enemies;
    //public Vector3[] enemyPositions;
    public int[] enemyInSight;
    List<Vector3> allyPositions;
    public FieldOfView vision;
    public Transform shootSource, throwSource, bombSource;
    public Bomb bombRef;
    public Queue<int> actions= new Queue<int>();
    public Vector3 hardcodedInitPos;
    public GameController gameManager;
    public Transform head;
    public TeamObservation teamObservations;
    //public AudioSensor audioSensor;
    

    protected virtual void Awake()
    {
        //audioSensor = GetComponent<AudioSensor>();
        // teamObservations = GetComponentInParent<TeamObservation>();

        gameManager = GetComponentInParent<GameController>();
        int enemyCount = 0;
        Character[] allAgents = gameManager.GetComponentsInChildren<Character>();
        //Debug.Log(allAgents.Length);
        //Debug.Log(enemyPositions.Length);
        foreach (Character agent in allAgents)
        {
            if (agent.team != team && agent != this)
            {
                enemies.Add(agent);
            }
        }
        //Debug.Log(enemyCount);
        //enemyPositions = new Vector3[enemyCount];
        enemyInSight = new int[enemyCount];

        allyPositions = new List<Vector3>();

        foreach (Character agent in allAgents)
        {
            if (agent.team == team && agent != this)
            {
                allyPositions.Add(new Vector3());
                allies.Add(agent);
            }
        }
    }

    protected virtual void Start()
    {

    }

    public override void OnEpisodeBegin()
    {
        hp = 100;
        equipmentManager.equipments[0].ammo = equipmentManager.equipments[0].max_ammo;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Character[] agents = gameManager.GetComponentsInChildren<Character>();
        //counter++;
        sensor.AddObservation(transform.localPosition);//3
        foreach (Character ally in allies)
        {
            Vector3 allyPos = ally.transform.localPosition;
            //Debug.Log(allyPos);
            sensor.AddObservation(allyPos);//3
            sensor.AddObservation(ally.isAlive);//1
        }
        Vector3 normalizedRotation = Utilities.MinMaxNormalization(transform.localRotation.eulerAngles, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));//3
        //float rotationY = Utilities.MinMaxNormalization(transform.localRotation.eulerAngles.y, -180, 180);
        //float rotationX = Utilities.MinMaxNormalization(head.transform.localRotation.eulerAngles.x, movement.minPitch, movement.maxPitch);
        sensor.AddObservation(normalizedRotation);
        //sensor.AddObservation(head.transform.localRotation.eulerAngles.x);
        //1
        if (equipmentManager.check())
        {
            Character targetAgent = equipmentManager.getCurrentlyEquiped().getAim().GetComponent<Character>();
            if(targetAgent.fovCheck(gameObject))
            {
                sensor.AddObservation(0);
            }
            else
            {
                sensor.AddObservation(1);
            }            
        }
        else
        {
            sensor.AddObservation(-1);
        }
        //1
        if (equipmentManager.isReloading())
        {
            sensor.AddObservation(-1);
        }
        else
        {
            sensor.AddObservation(1);
        }

        Character[] enemyInFov = vision.FieldOfViewCheck();
        foreach (Character enemy in enemyInFov)
        {
            teamObservations.enemyPositions[enemy.index][0] = enemy.transform.localPosition.x;
            teamObservations.enemyPositions[enemy.index][1] = enemy.transform.localPosition.y;
            teamObservations.enemyPositions[enemy.index][2] = enemy.transform.localPosition.z;
            teamObservations.enemyPositions[enemy.index][3] = 1;
        }

        foreach (float[] enemy in teamObservations.enemyPositions)
        {
            //Vector3 tmp = enemy;
            ////Debug.Log(allyPos);
            //sensor.AddObservation(tmp);//3
            sensor.AddObservation(enemy[0]);
            sensor.AddObservation(enemy[1]);
            sensor.AddObservation(enemy[2]);
            sensor.AddObservation(enemy[3]);
        }
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        doAction(actions.DiscreteActions[0], actions.DiscreteActions[1], actions.DiscreteActions[2]/*, actions.DiscreteActions[3]*//*, actions.ContinuousActions[0], actions.ContinuousActions[1]*/);
        //Debug.Log(actions.DiscreteActions[1]);
        //AddReward(0.01f);
        //equipmentManager.processRewardPerTimestep();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //movement.processMovement(actionsOut);
    }

    public EquipmentManager GetEquipmentManager()
    {
        return equipmentManager;    
    }

    public float takeDamage(float damage, Character attacker)
    {
        //attacker.AddReward(0.8f);
        //AddReward(-1f);

        Debug.Log(transform.name + " Dead");
        hp -= damage;
        float attacker_reward = 0;
        if(hp <= 0)
        {
            float teamsize = gameManager.ctTeamSize;
            if (attacker.team == team)
            {
                attacker.AddReward(-1f/teamsize);
                //AddReward(-1f);
            }
            else
            {
                if(fovCheck(attacker.gameObject))
                {
                    attacker.AddReward(0.5f / teamsize);
                }
                else
                {
                    attacker.AddReward(1f / teamsize);
                }
                gameManager.killcounts[team] += 1;
            }
            isAlive = 0;
            gameObject.SetActive(false);

            return attacker_reward;
            //EndEpisode();
        }
        return attacker_reward;
    }

    public void stun(float stunTime)
    {
        StartCoroutine(getStunned(stunTime));
    }

    public void doAction(int moveAction, int rotateAction, int fireAction/*, int moveType*/)
    {
        switch(moveAction)
        {
            case 0:
                movement.forward();
                break;
            case 1:
                movement.forward();
                break;
            case 2:
                movement.backward();
                break;
            case 3:
                movement.right();
                break;
            case 4:
                movement.left();
                break;
        }
        switch (rotateAction)
        {
            case 0:
                //movement.rotateUP();
                break;
            case 1:
                movement.rotateLeft();
                break;
            case 2:
                movement.rotateRight();
                break;
            case 3:
                movement.rotateUP();
                break;
            case 4:
                movement.rotateDown();
                break;
        }
        switch (fireAction)
        {
            case 0:
                equipmentManager.fire();
                break;
        }
        //switch (moveType)
        //{
        //    case 0:
        //        movement.SetMoveSpeedToRun();
        //        break;
        //    case 1:
        //        movement.SetMoveSpeedToWalk();
        //        break;
        //}
    }

    #region hide
    //public void updateEnemyPositions(Vector3[] positions, int[]_enemyInSight)
    //{
    //    enemyPositions = positions;
    //    enemyInSight = _enemyInSight;
    //}
    #endregion

    public void resetAgent()
    {
        hp = 100;
        equipmentManager.equipments[0].resetEquipment();
        points = 0;
        isAlive = 1;
        gameObject.SetActive(true);
    }

    IEnumerator getStunned(float stunTime)
    {
        vision.stunned = true;

        yield return new WaitForSeconds(stunTime);

        vision.stunned = false;
    }

    public bool fovCheck(GameObject character)
    {
        Vector3 direction = character.transform.position - transform.position;
        direction.Normalize();

        float angle = Vector3.Angle(transform.forward, direction);
        if (angle <= fovAngle / 2f)
        {
            //Debug.Log("Angle: " + angle.ToString() + " FOV: " + (fov / 2f).ToString());
            //Debug.Log("Player is within FOV!");
            return true;
        }
        return false;
    }
}
