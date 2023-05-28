using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamObservation : MonoBehaviour
{
    public float[][] enemyPositions;
   
    public float decay;
    int teamSize;

    private void Awake()
    {
        teamSize = GetComponentInParent<GameController>().ctTeamSize;
        enemyPositions = new float[teamSize][];
        for (int i = 0;i < teamSize; i++)
        {
            enemyPositions[i] = new float[4];
        }        
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < teamSize; i++)
        {
            enemyPositions[i][3] -= decay;
        }
    }
}
