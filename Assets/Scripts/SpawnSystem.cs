using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public Spawn[] spawns;
    public int team;

    public List<Character> spawnTeam(int n)
    {
        List<Character> teamContainer = new List<Character>(); 
        for (int i = 0; i < n; i++)
        {
            teamContainer.Add(spawns[i].spawn(team));
        }
        return teamContainer;
    }
}
