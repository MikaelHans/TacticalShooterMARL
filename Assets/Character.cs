using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;
using Unity.MLAgents.Actuators;
using System.Linq;
using Unity.MLAgents.Sensors;

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
    public Character[] allies;
    public Vector3[] enemyPositions;
    public int[] enemyInSight;
    List<Vector3> allyPositions;
    public FieldOfView vision;
    public Transform shootSource, throwSource, bombSource;
    public Bomb bombRef;
    public Queue<int> actions= new Queue<int>();
    public Vector3 hardcodedInitPos;
    public GameController gameManager;


    private void Awake()
    {
        gameManager = GetComponentInParent<GameController>();
    }

    //public override void Initialize()
    //{
    //    gameManager = GetComponentInParent<GameManager>();  
    //}

    public override void OnEpisodeBegin()
    {
        hp = 100;
        equipmentManager.equipments[0].ammo = equipmentManager.equipments[0].max_ammo;
    }

    public override void CollectObservations(Unity.MLAgents.Sensors.VectorSensor sensor)
    {
        Character[] agents = gameManager.GetComponentsInChildren<Character>();
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation.eulerAngles);

        foreach (Character agent in agents)
        {
            if (agent.team != team)
            {
                //Debug.Log(agent);
                //sensor.AddObservation(agent.transform.localPosition);
                sensor.AddObservation(Vector3.Distance(transform.localPosition, agent.transform.localPosition));
                //obs.Add(agent.transform.localPosition);
            }
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.DiscreteActions[0]);
        //print(actions.DiscreteActions[2]);        
        
        
        //vision.processRewardPerTimestep();
        doAction(actions.DiscreteActions[0], actions.DiscreteActions[1], actions.DiscreteActions[2]);
        equipmentManager.processRewardPerTimestep();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        //discreteActions[] = 

    }

    private void Start()
    {                   
        int enemyCount = 0;
        Character[] enemies = gameManager.GetComponentsInChildren<Character>();
        //Debug.Log(enemyPositions.Length);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].team != team)
            {
                enemyCount++;
            }
        }
        //Debug.Log(enemyCount);
        enemyPositions = new Vector3[enemyCount];
        enemyInSight = new int[enemyCount];

        allies = new Character[5];
        allyPositions = new List<Vector3>();
        Character[] allAgents = FindObjectsOfType<Character>();
        

        foreach (Character agent in allAgents)
        {
            if (agent.team == team)
            {
                allyPositions.Add(new Vector3());
                allies[agent.index] = agent;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < enemyPositions.Length; i++)
        {
            if (enemyInSight[i] == 1)
            {
                Debug.DrawLine(transform.position, enemyPositions[i], Color.green);
            }
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(actions.Count);
        //if (actions.Count > 0)
        //{
        //    doAction(actions.Dequeue());
        //}
    }

    public EquipmentManager GetEquipmentManager()
    {
        return equipmentManager;    
    }

    public void takeDamage(float damage, Character attacker)
    {
        //attacker.AddReward(0.8f);
        //AddReward(-1f);
        Debug.Log(transform.name);
        hp -= damage;
        if(hp <= 0)
        {
            //Destroy(gameObject);
            attacker.AddReward(1f);
            AddReward(-1f);
            isAlive = 0;
            //gameObject.SetActive(false);
            
            //EndEpisode();
        }
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
                movement.forward();
                break;
            case 1:
                movement.backward();
                break;
            case 2:
                movement.right();
                break;
            case 3:
                movement.left();
                break;
        }
        switch (rotateAction)
        {
            case 0:
                movement.rotateLeft();
                //AddReward(0.05f);
                break;
            case 1:
                movement.rotateRight();
                break;
        }
        switch (fireAction)
        {
            case 0:
                equipmentManager.fire();
                break;
        }
    }

    public Observation getObservation()
    {
        Observation observation = new Observation();
        observation.Position = transform.position;
        observation.Rotation = transform.eulerAngles;
        observation.hp = hp;
        observation.ammoLeft = equipmentManager.equipments[0].ammo;
        observation.heLeft = equipmentManager.equipments[1].ammo;
        observation.stunLeft = equipmentManager.equipments[2].ammo;

        for (int i = 0; i < allyPositions.Count; i++)
        {
            allyPositions[i] = allies[i].transform.position;
        }
        observation.allyPositions = allyPositions.ToArray();
        observation.enemyPositions = enemyPositions;

        return observation;
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
