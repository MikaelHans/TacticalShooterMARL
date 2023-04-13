using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;
using Unity.MLAgents.Actuators;
using System.Linq;

public class Character_1p : Agent
{
    public float hp, points;
    public int ammoLeft, heLeft, stunLeft, team, index, inBombSite, bombInRange = 0, isAlive = 1, max_timestep=1500, current_timestep=0;
    public Movement movement;
    public EquipmentManager equipmentManager;
    public Character[] allies;
    public Vector3[] enemyPositions;
    public int[] enemyInSight;
    List<Vector3> allyPositions;
    public FieldOfView vision;
    public Transform shootSource, throwSource, bombSource;
    public Bomb bombRef;
    public Queue<int> actions = new Queue<int>();
    public Vector3 hardcodedInitPos;
    GameController gameManager;
    public TargetPractice targetPractice;
    public LayerMask layerMask;
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
        //hp = 100;
        transform.localPosition = new Vector3(16,0,-16);
        //equipmentManager.equipments[0].ammo = equipmentManager.equipments[0].max_ammo;
    }

    //public override void CollectObservations(Unity.MLAgents.Sensors.VectorSensor sensor)
    //{
    //    sensor.AddObservation(transform.localPosition);
    //    sensor.AddObservation(transform.localRotation.eulerAngles);

        
    //}

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.DiscreteActions[0]);
        //print(actions.DiscreteActions[2]);        ;
        //vision.processRewardPerTimestep();
        doAction(actions.DiscreteActions[0], actions.DiscreteActions[1]);
        RaycastHit hit;
        if (Physics.Raycast(shootSource.transform.position, shootSource.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            //Debug.Log(hit.collider.transform.GetComponentInParent<Character_1p>());
            //Character_1p hitcharacter = hit.collider.transform.GetComponentInParent<Character_1p>();
            TargetPractice hitcharacter = hit.collider.transform.GetComponent<TargetPractice>();
            //Debug.Log(hit.collider.name);
            if (hitcharacter != null)
            {
                AddReward(0.5f);
                if (team == 0)
                {
                    Debug.DrawLine(transform.position, hitcharacter.transform.position, Color.blue, 0.5f);
                }
                else
                {
                    Debug.DrawLine(transform.position, hitcharacter.transform.position, Color.red, 0.5f);
                }
            }
            else
            {
                points -= 0.01f;
                SetReward(-0.01f);
            }

        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        //discreteActions[] = 

    }

    //private void Start()
    //{
    //    int enemyCount = 0;
    //    Character[] enemies = gameManager.GetComponentsInChildren<Character>();
    //    //Debug.Log(enemyPositions.Length);
    //    for (int i = 0; i < enemies.Length; i++)
    //    {
    //        if (enemies[i].team != team)
    //        {
    //            enemyCount++;
    //        }
    //    }
    //    //Debug.Log(enemyCount);
    //    enemyPositions = new Vector3[enemyCount];
    //    enemyInSight = new int[enemyCount];

    //    allies = new Character[5];
    //    allyPositions = new List<Vector3>();
    //    Character[] allAgents = FindObjectsOfType<Character>();


    //    foreach (Character agent in allAgents)
    //    {
    //        if (agent.team == team)
    //        {
    //            allyPositions.Add(new Vector3());
    //            allies[agent.index] = agent;
    //        }
    //    }
    //}

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
        current_timestep += 1;
        if(current_timestep >= max_timestep)
        {
            current_timestep= 0;
            EndEpisode();
        }
    }

    public EquipmentManager GetEquipmentManager()
    {
        return equipmentManager;
    }

    public void takeDamage(float damage, Character attacker)
    {
        attacker.SetReward(10f);
        SetReward(-10f);
        hp -= damage;
        if (hp <= 0)
        {
            //Destroy(gameObject);
            attacker.SetReward(50f);
            SetReward(-50f);
            //EndEpisode();
        }
    }

    public void stun(float stunTime)
    {
        StartCoroutine(getStunned(stunTime));
    }

    public void doAction(int moveAction, int rotateAction)
    {
        switch (moveAction)
        {
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
            case 1:
                movement.rotateLeft();
                break;
            case 2:
                movement.rotateRight();
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

    public void updateEnemyPositions(Vector3[] positions, int[] _enemyInSight)
    {
        enemyPositions = positions;
        enemyInSight = _enemyInSight;
    }

    IEnumerator getStunned(float stunTime)
    {
        vision.stunned = true;

        yield return new WaitForSeconds(stunTime);

        vision.stunned = false;
    }
}
