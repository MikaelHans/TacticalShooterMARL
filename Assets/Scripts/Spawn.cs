using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Character characterPrefab, agent;
    public float spawn_offset, rotation_offset;
    public bool randomizeSpawn, spawnX, randomizeRotation;
    //private void Awake()
    //{
    //    agent = GetComponentInChildren<Character>();
    //}

    private void Start()
    {
        Random.InitState(14190218 + (int)transform.position.magnitude);
    }

    public Character spawn(int _team)
    {
        if (randomizeSpawn)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.z = Random.Range(spawnPos.z - spawn_offset, spawnPos.z + spawn_offset);
            if(spawnX)
            {
                spawnPos.x = Random.Range(spawnPos.x - spawn_offset, spawnPos.x + spawn_offset);
            }
            agent.transform.position = spawnPos;
            agent.transform.rotation = transform.rotation;            
        }
        else
        {
            Vector3 spawnPos = transform.position;
            //spawnPos.x = Random.Range(spawnPos.x - spawn_offset, spawnPos.x + spawn_offset);
            agent.transform.position = spawnPos;
            agent.transform.rotation = transform.rotation;
        }
        //randomize rotation
        if (randomizeRotation)
        {
            Vector3 spawnRotation = transform.localRotation.eulerAngles;
            spawnRotation.y = Random.Range(0, 360);
            agent.transform.rotation = Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z);
        }

        //reset agent
        agent.resetAgent();
        agent.team = _team;
        if (agent.GetComponentInChildren<AudioListener>())
        {
            agent.GetComponentInChildren<AudioListener>().enabled = false;
        }
        //Debug.Log("Timestep: " + agent.counter.ToString());
        //agent.counter = 0;
        return agent;
    }
}
