using System.Collections;
using System.Collections.Generic;
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
    int waypointIndex = 0;
    public FieldOfView fov;
    NavMeshAgent navmesh;
    public Vector3 currentWaypoint;
    public Transform[] waypoints;
    public List<Vector3> waypointStaticLocations = new List<Vector3>();
    bool enemiesSighted = false;
    public float distanceThreshold;
    [SerializeField]
    Character target;

    protected override void Awake()
    {
        base.Awake();
        navmesh = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();        
        //currentWaypoint = enemies[index].GetComponent<Transform>();
        foreach (Transform waypoint in waypoints)
        {
            waypointStaticLocations.Add(waypoint.position);
        }

        currentWaypoint = waypointStaticLocations[waypointIndex];
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
        navmesh.destination = currentWaypoint;
        if(Vector3.Distance(transform.position, currentWaypoint) <= distanceThreshold)
        {
            int tmp = waypointIndex + 1 >= waypoints.Length ? waypointIndex : waypointIndex+1;
            waypointIndex = tmp;
            currentWaypoint = waypointStaticLocations[waypointIndex];
        }
        /*
           check aim if there is enemy, shoot
         */
        Character[] enemiesInRange = fov.FieldOfViewCheck();
        if (enemiesInRange.Length > 0)
        {
            navmesh.isStopped = true;
            target = enemiesInRange[0];
            enemiesSighted = true;
        }

        if(enemiesSighted)
        {
            var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * movement.rotationSpeed);

            Vector3 enemyPos = target.transform.localPosition;
            Vector3 curPos = transform.localPosition;

            enemyPos.y = 0;
            curPos.y = 0;

            //Vector3 targetDir = curPos - enemyPos;
            //Vector3 forward = target.transform.forward;
        }

        if (equipmentManager.check())
        {
            equipmentManager.fire();
            if(target.hp <= 0)
            {
                enemiesSighted = false;
                navmesh.isStopped = false;
            }
        }
    }
}
