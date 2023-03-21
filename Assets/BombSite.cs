using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSite : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Character agent = other.GetComponent<Character>();
        if(agent != null )
            agent.inBombSite = 1;
    }

    private void OnTriggerExit(Collider other)
    {
        Character agent = other.GetComponent<Character>();

        if (agent != null)
            agent.inBombSite = -1;

    }
}
