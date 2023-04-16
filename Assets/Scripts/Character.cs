using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;
using Unity.MLAgents.Actuators;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;

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
    public float hp, points;
    public int ammoLeft, heLeft, stunLeft, team, index, inBombSite, bombInRange = 0, isAlive = 1;
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


    protected virtual void Awake()
    {
        gameManager = GetComponentInParent<GameController>();
        int enemyCount = 0;
        Character[] allAgents = gameManager.GetComponentsInChildren<Character>();
        Debug.Log(allAgents.Length);
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
        sensor.AddObservation(transform.localPosition);
        Vector3 normalizedRotation = MinMaxNormalization(transform.localRotation.eulerAngles, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        sensor.AddObservation(normalizedRotation);
        if (equipmentManager.check())
        {
            sensor.AddObservation(1);
        }
        else
        {
            sensor.AddObservation(0);
        }

        if (equipmentManager.isReloading())
        {
            sensor.AddObservation(1);
        }
        else
        {
            sensor.AddObservation(0);
        }
        ////foreach (Character agent in agents)
        ////{
        ////    if (agent.team != team)
        ////    {
        ////        sensor.AddObservation(agent.transform.localPosition - transform.localPosition);
        ////    }
        ////}
    }

    public static double MinMaxNormalization(double value, double minValue, double maxValue)
    {
        // Check for division by zero
        if (minValue == maxValue)
        {
            throw new ArgumentException("minValue and maxValue cannot be the same.");
        }

        // Perform Min-Max normalization
        double normalizedValue = (value - minValue) / (maxValue - minValue);
        return normalizedValue;
    }

    public static Vector3 MinMaxNormalization(Vector3 vector, Vector3 minValue, Vector3 maxValue)
    {
        // Normalize the x, y, and z components separately
        float normalizedX = Mathf.Clamp01((vector.x - minValue.x) / (maxValue.x - minValue.x));
        float normalizedY = Mathf.Clamp01((vector.y - minValue.y) / (maxValue.y - minValue.y));
        float normalizedZ = Mathf.Clamp01((vector.z - minValue.z) / (maxValue.z - minValue.z));

        // Create a new normalized Vector3
        Vector3 normalizedVector = new Vector3(normalizedX, normalizedY, normalizedZ);
        return normalizedVector;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        doAction(actions.DiscreteActions[0], actions.DiscreteActions[1], actions.DiscreteActions[2]);
        //AddReward(0.01f);
        //equipmentManager.processRewardPerTimestep();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

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
                attacker_reward = -1;
                attacker.AddReward(-1f);
                //AddReward(-1f);
            }
            else
            {

                attacker.AddReward(1f/teamsize);
                attacker_reward = 1;
                //SetReward(-1f);
            }
            isAlive = 0;
            //attacker_reward = 999;
            //Debug.Log(attacker_reward);
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

    public void doAction(int moveAction, int rotateAction, int fireAction)
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
        switch (rotateAction)
        {
            case 0:
                break;
            case 1:
                movement.rotateLeft();
                //AddReward(0.05f);
                break;
            case 2:
                movement.rotateRight();
                break;
            case 3:
                movement.rotateDown();
                //AddReward(0.05f);
                break;
            case 4:
                movement.rotateUP();
                break;
        }
        switch (fireAction)
        {
            case 0:
                equipmentManager.fire();
                break;
        }
    }

    public void updateEnemyPositions(Vector3[] positions, int[]_enemyInSight)
    {
        enemyPositions = positions;
        enemyInSight = _enemyInSight;
    }

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
}
