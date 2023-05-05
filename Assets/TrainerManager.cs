using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Analytics;

public class TrainerManager : MonoBehaviour
{
    public int totalEpisodes;
    GameController[] trainers;
    public string fileName = "EpisodeLog.txt"; // the name of the file to write to
    public string textToWrite, training_id;


    private void OnApplicationQuit()
    {
        string filePath = Path.Combine(Application.dataPath, fileName); // get the file path

        textToWrite = $"{training_id}\n{DateTime.Now}\ntotal episodes: {totalEpisodes}";
        


        // write the text to the file
        File.AppendAllText(filePath, textToWrite + "\n");
        Debug.Log($"Text written to file at path: {filePath}");
    }

    private void Start()
    {
        trainers = FindObjectsOfType<GameController>();
    }

    private void Update()
    {
        totalEpisodes = 0;
        foreach (GameController controller in trainers)
        {
            totalEpisodes += controller.episodeCount;
        }
        
    }
}
