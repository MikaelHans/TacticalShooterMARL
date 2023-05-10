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
    public float hp, points, fov;
    public int ammoLeft, heLeft, stunLeft, team, index, inBombSite, bombInRange = 0, isAlive = 1, counter = 0;
    public Movement movement;
    public EquipmentManager equipmentManager;
    public List<Character> allies, enemies;
    public Vector3[] enemyPositions;
    public int[] enemyInSight;
    List<Vector3> allyPositions;
    public FieldOfView vision;
    public Transform shootSource, throwSource, bombSource;
    public Bomb bombRef;
    public Queue<int> actions= new Queue<int>();
    public Vector3 hardcodedInitPos;
    public GameController gameManager;
    //public AudioSensor audioSensor;
    

    protected virtual void Awake()
    {
        //audioSensor = GetComponent<AudioSensor>();
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
        enemyPositions = new Vector3[enemyCount];
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
        sensor.AddObservation(transform.localPosition);
        Vector3 normalizedRotation = Utilities.MinMaxNormalization(transform.localRotation.eulerAngles, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        sensor.AddObservation(normalizedRotation);
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

        if (equipmentManager.isReloading())
        {
            sensor.AddObservation(-1);
        }
        else
        {
            sensor.AddObservation(1);
        }

        foreach (Character enemy in enemies)
        {
            if(enemy.fovCheck(gameObject))
            {
                sensor.AddObservation(-1);
            }
            else
            {
                sensor.AddObservation(1);
            }
        }

        sensor.AddObservation(movement.head.localRotation.eulerAngles.x);
        //for (int i = 0; i < audioSensor.sensorGridCount; i++)
        //{
        //    sensor.AddObservation(audioSensor.sensorData[i]);
        //}

        //for (int i = 0; i < allies.Count; i++)
        //{
        //    sensor.AddObservation(allies[i].transform.position);
        //}

        //for (int i = 0; i < allies.Count; i++)
        //{
        //    sensor.AddObservation(allies[i].isAlive);
        //}

        //audioSensor.resetSensorData();
        ////foreach (Character agent in agents)
        ////{
        ////    if (agent.team != team)
        ////    {
        ////        sensor.AddObservation(agent.transform.localPosition - transform.localPosition);
        ////    }
        ////}
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        doAction(actions.DiscreteActions[0], actions.DiscreteActions[1], actions.DiscreteActions[2]/*, actions.DiscreteActions[3]*/, actions.ContinuousActions[0]/*, actions.ContinuousActions[1]*/);
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
                    attacker.AddReward(0.6f / teamsize);
                }
                else
                {
                    attacker.AddReward(1f / teamsize);
                }
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

    public void doAction(int moveAction/*, int rotateAction*/, int fireAction, int moveType, float rotateX/*, float rotateY*/)
    {
        switch(moveAction)
        {
            case 0:
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
        //switch (rotateAction)
        //{
        //    case 0:
        //        break;
        //    case 1:
        //        movement.rotateLeft();
        //        //AddReward(0.05f);
        //        break;
        //    case 2:
        //        movement.rotateRight();
        //        break;
        //        //case 3:
        //        //    movement.rotateDown();
        //        //    //AddReward(0.05f);
        //        //    break;
        //        //case 4:
        //        //    movement.rotateUP();
        //        //    break;
        //}
        switch (fireAction)
        {
            case 0:
                equipmentManager.fire();
                break;
        }
        switch (moveType)
        {
            case 0:
                movement.SetMoveSpeedToRun();
                break;
            case 1:
                movement.SetMoveSpeedToWalk();
                break;
        }
        //Debug.Log(rotationX);
        //Debug.Log(rotationY);
        //movement.continuousRotationX(rotateY);
        movement.continuousRotationY(rotateX);
    }

    #region hide
    public void updateEnemyPositions(Vector3[] positions, int[]_enemyInSight)
    {
        enemyPositions = positions;
        enemyInSight = _enemyInSight;
    }
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
        if (angle <= fov / 2f)
        {
            //Debug.Log("Angle: " + angle.ToString() + " FOV: " + (fov / 2f).ToString());
            //Debug.Log("Player is within FOV!");
            return true;
        }
        return false;
    }
}
