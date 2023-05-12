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

    public FieldOfView fov;
    NavMeshAgent navmesh;
    public Transform currentWaypoint;
    public Transform[] waypoints;

    protected override void Awake()
    {
        base.Awake();
        navmesh = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();        
        currentWaypoint = enemies[index].GetComponent<Transform>();
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
        navmesh.destination = currentWaypoint.position;
        
        /*
           check aim if there is enemy, shoot
         */
        if (equipmentManager.check())
        {
            equipmentManager.fire();
        }
    }
}
