using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class GameController : MonoBehaviour
{
    public Character[] counterTerrorists, terrorist;
    public int ctTeamSize, tTeamSize, maxStep, episodeCount;
    public SpawnSystem ctSpawn, tSpawn;
    public bool bombPlanted = false;
    public float timer, roundLength;
    public int[] killcounts= new int[2];

    private SimpleMultiAgentGroup counterTerroristTeam, terrorristTeam;

    private void Awake()
    {
        timer = roundLength;
    }

    private void Start()
    {
        counterTerroristTeam = new SimpleMultiAgentGroup();
        terrorristTeam = new SimpleMultiAgentGroup();
        //Debug.Log("CT_TEAM_ID: " + counterTerroristTeam.GetId());
        //Debug.Log("T_TEAM_ID: " + terrorristTeam.GetId());
        counterTerrorists = new Character[ctTeamSize];
        terrorist = new Character[tTeamSize];

        counterTerrorists = ctSpawn.spawnTeam(ctTeamSize).ToArray();
        terrorist = tSpawn.spawnTeam(tTeamSize).ToArray();

        foreach (Character c in counterTerrorists)
        {
            counterTerroristTeam.RegisterAgent(c);
        }

        foreach (Character c in terrorist)
        {
            terrorristTeam.RegisterAgent(c);
        }
        killcounts[0] = 0;
        killcounts[1] = 0;
        //Debug.Log(counterTerroristTeam.GetRegisteredAgents().Count);
        //Debug.Log(terrorristTeam.GetRegisteredAgents().Count);
        //Debug.Log("SUCCESS INIT");
        //terrorist[0].GetEquipmentManager().equipBomb();
    }


    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= roundLength)
        {
            Debug.Log("Time Limit Exceeded, CT Win");
            roundEnd(-1);
        }
        if (checkIfTeamAllDead(counterTerrorists))
        {
            Debug.Log("T WIN");
            roundEnd(0);
            //resetRound();
        }
        else if (checkIfTeamAllDead(terrorist))
        {
            Debug.Log("CT WIN");
            roundEnd(1);
            //resetRound();
        }
    }

    //IEnumerator countdown()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1);
    //    }
    //}

    public void roundEnd(int team)
    {
        Debug.Log("ROUNDEND");
        if (team == 1)//ctwin
        {
            counterTerroristTeam.AddGroupReward(1f);
            //terrorristTeam.AddGroupReward(-1f);
            //counterTerroristTeam.SetGroupReward(1f - timer / roundLength);
            //terrorristTeam.SetGroupReward(-1f + timer / roundLength);
        }
        else if (team == 0)//twin
        {
            terrorristTeam.AddGroupReward(1f);
            //counterTerroristTeam.AddGroupReward(-1f);
            //counterTerroristTeam.SetGroupReward(-1f + timer / roundLength);
        }
        else if (team == -1)//time limit exceeded
        {
            //terrorristTeam.SetGroupReward(1f - numberOfAgentsAlive(counterTerrorists) / ctTeamSize);
            //counterTerroristTeam.SetGroupReward(1f - numberOfAgentsAlive(terrorist) / tTeamSize);
            terrorristTeam.AddGroupReward(-0.15f);
            //counterTerroristTeam.AddGroupReward(-0.15f);

            counterTerroristTeam.EndGroupEpisode();
            terrorristTeam.EndGroupEpisode();
            
        }
        episodeCount++;
        resetRound();
    }

    public void resetRound()
    {
        Debug.Log("Round Done");
        //add reward for killing enemy
        terrorristTeam.AddGroupReward(1f - (ctTeamSize - killcounts[0]) / ctTeamSize);
        counterTerroristTeam.AddGroupReward(1f - (tTeamSize - killcounts[1]) / tTeamSize);

        counterTerroristTeam.EndGroupEpisode();
        terrorristTeam.EndGroupEpisode();

        killcounts[0] = 0;
        killcounts[1] = 0;

        ctSpawn.spawnTeam(ctTeamSize);
        tSpawn.spawnTeam(tTeamSize);

        timer = 0;
    }

    public bool checkIfTeamAllDead(Character[] team)
    {
        for (int i = 0; i < team.Length; i++)
        {
            if (team[i].isAlive == 1)
                return false;
        }
        return true;
    }

    public int numberOfAgentsAlive(Character[] team)
    {
        int res = 0;
        for (int i = 0; i < team.Length; i++)
        {
            if (team[i].isAlive == 1)
                res++;
        }
        return res;
    }
}
