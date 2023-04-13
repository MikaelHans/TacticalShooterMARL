using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float timeToExplode, defuseTime, timeToDefuse;
    public bool isDefusing, isActive;
    GameController gameManager;
    // Start is called before the first frame update
    void Start()
    {
        timeToDefuse = defuseTime;
        isActive = true;
        gameManager= FindObjectOfType<GameController>();
    }

    private void FixedUpdate()
    {
        if(isDefusing)
        {
            timeToDefuse -= Time.deltaTime;
        }
        else
        {
            timeToDefuse = defuseTime;
        }

        if(isActive)
            timeToExplode -= Time.deltaTime;

        if(timeToExplode < 0 )
        {
            //Debug.Log("Boom");
            gameManager.roundEnd(1);
        }
        else if(timeToDefuse < 0 )
        {
            //Debug.Log("Defused");
            gameManager.roundEnd(0);
            isActive = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("tes");
        Character character = collision.gameObject.GetComponent<Character>();
        if (character != null)
        {
            character.bombInRange = 1;
            character.bombRef = this;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if (character != null)
        {
            character.bombInRange = 0;
            character.bombRef = null;
        }
    }

    public void defuse()
    {

    }

}
