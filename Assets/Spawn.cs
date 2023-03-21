using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Character characterPrefab, agent;
    public float spawn_offset, rotation_offset;
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
        //Character agent = Instantiate(characterPrefab, transform.position, transform.rotation);
        Vector3 spawnPos = transform.position;
        //Debug.Log(spawnPos);
        spawnPos.x = Random.Range(spawnPos.x  - spawn_offset, spawnPos.x + spawn_offset);
        agent.transform.position = spawnPos;
        agent.transform.rotation = transform.rotation;
        agent.resetAgent();
        agent.team = _team;
        agent.GetEquipmentManager().equipments[3] = null;
        agent.GetComponentInChildren<AudioListener>().enabled= false;
        return agent;
    }
}
