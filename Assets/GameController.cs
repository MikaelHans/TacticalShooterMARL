using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class GameController : MonoBehaviour
{
    public Character[] counterTerrorists, terrorist;
    public int ctTeamSize, tTeamSize, maxStep;
    public SpawnSystem ctSpawn, tSpawn;
    public bool bombPlanted = false;
    public float timer, roundLength;

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
        //Debug.Log("SUCCESS INIT");
        //terrorist[0].GetEquipmentManager().equipBomb();
    }


    private void FixedUpdate()
    {
        timer++;
        if (timer >= maxStep)
        {
            Debug.Log("Time Limit Exceeded");
            resetRound();
        }
        if (checkIfTeamAllDead(counterTerrorists))
        {
            Debug.Log("T WIN");
            roundEnd(1);
            //resetRound();
        }
        else if (checkIfTeamAllDead(terrorist))
        {
            Debug.Log("CT WIN");
            roundEnd(0);
            //resetRound();
        }
        //else
        //{
        //    counterTerroristTeam.AddGroupReward(-0.4f);
        //    terrorristTeam.AddGroupReward(-0.4f);
        //}
        ////check if bomb planted
        //if(bombPlanted)
        //{
        //    if (checkIfTeamAllDead(counterTerrorists))
        //    {
        //        Debug.Log("Terrorists Win");
        //        roundEnd(1);
        //        resetRound();
        //    }
        //}
        //else
        //{
        //    //check if team still has at least a member alive
        //    if (checkIfTeamAllDead(counterTerrorists))
        //    {
        //        roundEnd(1);
        //        resetRound();
        //    }
        //    else if (checkIfTeamAllDead(terrorist))
        //    {
        //        roundEnd(0);
        //        resetRound();
        //    }
        //}

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
        if (team == 1)
        {
            counterTerroristTeam.AddGroupReward(1f - timer / roundLength);
            terrorristTeam.AddGroupReward(-1f + timer / roundLength);
        }
        else if (team == 0)
        {
            terrorristTeam.AddGroupReward(1f - timer / roundLength);
            counterTerroristTeam.AddGroupReward(-1f + timer / roundLength);
        }
        resetRound();
    }

    public void resetRound()
    {
        Debug.Log("Round Done");

        //terrorristTeam.AddGroupReward(-5f);
        //counterTerroristTeam.AddGroupReward(-5f);
        float tpoints = 0, ctpoints = 0, absdiff;
        foreach (Character c in terrorist)
        {
            tpoints += c.points;
            //c.EndEpisode();
        }

        foreach (Character c in counterTerrorists)
        {
            ctpoints += c.points;
            //c.EndEpisode();
        }
        //absdiff = Mathf.Abs(tpoints - ctpoints);
        //float winreward = 0.8f;
        //Debug.Log(absdiff + winreward);
        //if (tpoints < ctpoints)
        //{
        //    counterTerroristTeam.AddGroupReward(winreward);
        //    counterTerroristTeam.AddGroupReward(absdiff);
        //}
        //else
        //{
        //    counterTerroristTeam.AddGroupReward(winreward);
        //    terrorristTeam.AddGroupReward(absdiff);
        //}

        counterTerroristTeam.EndGroupEpisode();
        terrorristTeam.EndGroupEpisode();

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
}
