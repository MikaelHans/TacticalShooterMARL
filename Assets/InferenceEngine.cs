using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InferenceEngine : MonoBehaviour
{
    public int totalEpisodes;
    GameController inference;
    public string fileName = "RoundResults.txt"; // the name of the file to write to
    public string textToWrite, training_id;

    private void OnApplicationQuit()
    {
        string filePath = Path.Combine(Application.dataPath, fileName); // get the file path

        textToWrite = $"{training_id}\n{DateTime.Now}\nTWin: {inference.tWin}\nCTWin: {inference.ctWin}";



        // write the text to the file
        File.AppendAllText(filePath, textToWrite + "\n");
        Debug.Log($"Text written to file at path: {filePath}");
    }

    private void Update()
    {
        
    }
}
