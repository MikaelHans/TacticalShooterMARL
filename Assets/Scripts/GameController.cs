using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System.IO.Abstractions;
using System.IO;
using System;
using System.Linq;
using UnityEditor;

public class GameController : MonoBehaviour
{
    public Character[] counterTerrorists, terrorist;
    public int ctTeamSize, tTeamSize, maxStep, episodeCount, tWin = 0, ctWin = 0, round = 15, currentRound, match, ctTotalKill, tTotalKill;
    public SpawnSystem ctSpawn, tSpawn;
    public bool bombPlanted = false;
    public float timer, roundLength;
    public int[] killcounts= new int[2];
    public int difficulty = 0;
    public string inference_id;
    public bool inference;
    public Camera spectatorCamera;
    private SimpleMultiAgentGroup counterTerroristTeam, terrorristTeam;

    private void Awake()
    {
        timer = roundLength;
    }

    private void Start()
    {

        currentRound = 1;
        match = 1;
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
            Debug.Log("Time Limit Exceeded");
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
            terrorristTeam.AddGroupReward(-0.1f);
            ctWin += 1;
            //counterTerroristTeam.SetGroupReward(1f - timer / roundLength);
            //terrorristTeam.SetGroupReward(-1f + timer / roundLength);
        }
        else if (team == 0)//twin
        {
            terrorristTeam.AddGroupReward(1f);
            counterTerroristTeam.AddGroupReward(-0.1f);
            tWin += 1;
            //counterTerroristTeam.SetGroupReward(-1f + timer / roundLength);
        }
        else if (team == -1)//time limit exceeded
        {
            //terrorristTeam.SetGroupReward(1f - numberOfAgentsAlive(counterTerrorists) / ctTeamSize);
            //counterTerroristTeam.SetGroupReward(1f - numberOfAgentsAlive(terrorist) / tTeamSize);
            terrorristTeam.AddGroupReward(-0.1f);
            counterTerroristTeam.AddGroupReward(-0.1f);

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
        tTotalKill += killcounts[0];
        ctTotalKill += killcounts[1];
        killcounts[0] = 0;
        killcounts[1] = 0;

        ctSpawn.spawnTeam(ctTeamSize);
        tSpawn.spawnTeam(tTeamSize);

        timer = 0;
        currentRound++;
        if(currentRound > round && inference)
        {
            currentRound = 0;
            string fileName = $"MatchResults/{inference_id}-{gameObject.name}.txt";
            string filePath = Path.Combine(Application.dataPath, fileName); // get the file path

            string textToWrite = $"Match: {match}\n{gameObject.name}\n{DateTime.Now}\nTWin: {tWin}\nCTWin: {ctWin}\n";
            foreach (Character t in counterTerrorists)
            {
                if (t.death <= 0)
                {
                    textToWrite += $"<T>{t.gameObject.name}\nK/D: {t.kill}/{t.death}\nKDS: {(float)(t.kill / 1)}\n";
                }
                textToWrite += $"<CT>{t.gameObject.name}\nK/D: {t.kill}/{t.death}\nKDS: {(float)(t.kill / t.death)}\n";
                t.kill = 0;
                t.death = 0;
            }
            foreach (Character t in terrorist)
            {
                if(t.death <= 0)
                {
                    textToWrite += $"<T>{t.gameObject.name}\nK/D: {t.kill}/{t.death}\nKDS: {(float)(t.kill / 1)}\n";
                }
                textToWrite += $"<T>{t.gameObject.name}\nK/D: {t.kill}/{t.death}\nKDS: {(float)(t.kill / t.death)}\n";
                t.kill = 0;
                t.death = 0;
            }

            tWin = 0;
            ctWin = 0;
            // write the text to the file
            File.AppendAllText(filePath, textToWrite + "\n\n");
            Debug.Log($"Text written to file at path: {filePath}");
            match++;
            Application.Quit();
            EditorApplication.isPlaying = false;
            gameObject.SetActive(false);

        }
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
