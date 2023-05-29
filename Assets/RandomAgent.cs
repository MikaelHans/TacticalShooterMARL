using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class RandomAgent : Character
{
    // Start is called before the first frame update
    //public float maxReactionTime;
    //[Range(0, 100)]
    /*
        Controls the reaction time of the agent
        the higher the value the faster the random agent
        will snap the crosshair to the enemy and shoot
     */
    
    /*
     * 
        Behaviour Types
        1. Hold Position
        2. Purely Random Movement
        3. Rush
     *
     */
    public int behaviourType;
    [SerializeField]
    int waypointIndex = 0, strategyIndex;
    public FieldOfView fov;
    NavMeshAgent navmesh;
    public Vector3 currentWaypoint;
    //public GameObject waypointParent;
    //Waypoint[] waypoints;
    //public List<Vector3> waypointStaticLocations = new List<Vector3>();
    //public List<Quaternion> waypointStaticRotations= new List<Quaternion>();
    bool enemiesSighted = false;
    public float distanceThreshold;
    [SerializeField]
    Character target;
    public float waypointWaitTime, delay;
    [SerializeField]
    Strategy[] strategies;
    [SerializeField]
    Strategy currentStrategy;
    public float[] aim, shootDelay = new float[3];

    protected override void Awake()
    {
        base.Awake();
        navmesh = GetComponent<NavMeshAgent>();
        //currentWaypoint = enemies[index].GetComponent<Transform>();
        strategies = GetComponentsInChildren<Strategy>();
    }

    

    protected override void Start()
    {
        base.Start();        
        int strategy = 0;
        currentStrategy.waypoints = strategies[strategy].waypoints;
        currentWaypoint = currentStrategy.waypointStaticLocations[waypointIndex];
        delay = shootDelay[gameManager.difficulty];
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
    }

    // Update is called once per frame
    void Update()
    {
        if(waypointWaitTime > 0)
        {
            navmesh.isStopped = true;
            waypointWaitTime -= Time.deltaTime;
            //var rotation = Quaternion.LookRotation(waypointStaticRotations[waypointIndex].transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, currentStrategy.waypointStaticRotations[waypointIndex-1], Time.deltaTime * movement.rotationSpeed);
        }
        else
        {
            navmesh.isStopped = false;
            navmesh.destination = currentWaypoint;
            if (Vector3.Distance(transform.position, currentWaypoint) <= distanceThreshold)
            {
                //int tmp = waypointIndex + 1 >= waypoints.Length ? waypointIndex : waypointIndex+1;
                waypointWaitTime = currentStrategy.waypoints[waypointIndex].waypointWaitTime;
                
                if(waypointIndex < currentStrategy.waypoints.Length-1)
                {
                    waypointIndex++;
                    currentWaypoint = currentStrategy.waypointStaticLocations[waypointIndex];
                }
                else
                {
                    waypointIndex = 0;
                }
            }
        }
        
        /*
           check aim if there is enemy, shoot
         */
        Character[] enemiesInRange = fov.FieldOfViewCheck();
        if (enemiesInRange.Length > 0)
        {
            navmesh.isStopped = true;
            target = enemiesInRange[0];
            //if (equipmentManager.check())
            //{
            //    enemiesSighted = false;
            //}
            //else
            //{
            //    enemiesSighted = true;
            //}
            enemiesSighted = true;
        }

        if(enemiesSighted)
        {
            var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (movement.rotationSpeed * aim[gameManager.difficulty]));

            Vector3 enemyPos = target.transform.localPosition;
            Vector3 curPos = transform.localPosition;

            enemyPos.y = 0;
            curPos.y = 0;
            //Vector3 targetDir = curPos - enemyPos;
            //Vector3 forward = target.transform.forward;
        }

        if (equipmentManager.check())
        {
            delay -= Time.deltaTime;            
            if(delay < 0)
            {
                equipmentManager.fire();
                if (target.hp <= 0)
                {
                    enemiesSighted = false;
                    navmesh.isStopped = false;
                    delay = shootDelay[gameManager.difficulty];
                }
            }            
        }
        else
        {
            delay = shootDelay[gameManager.difficulty];
        }
    }

    void initStrategy()
    {
        if(index == 0 )
        {
            strategyIndex = UnityEngine.Random.Range(0, strategies.Length - 1);
            currentStrategy = strategies[strategyIndex];

            foreach(RandomAgent agent in gameManager.GetComponentsInChildren<RandomAgent>())
            {
                agent.strategyIndex = strategyIndex;
                agent.currentStrategy= agent.strategies[strategyIndex];
            }
        }
        
    }

    public override void resetAgent()
    {
        base.resetAgent();
        waypointIndex = 0;
        waypointWaitTime= 0;
        initStrategy();
        currentWaypoint = currentStrategy.waypointStaticLocations[waypointIndex];
    }

}
