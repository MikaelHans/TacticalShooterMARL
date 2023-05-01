using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerManager : MonoBehaviour
{
    public int totalEpisodes;
    GameController[] trainers;

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
