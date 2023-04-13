using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEquipment : Equipment
{
    public GameObject bombPrefab;
    protected void Start()
    {      
        instantiateSource = GetComponentInParent<Character>().bombSource;        
    }

    public override bool use()
    {
        if(ammo > 0)
        {
            if (character.inBombSite == 1)
            {
                plantBomb();
                ammo--;
            }
            return true;
        }
        return false;        
    }

    public void plantBomb()
    {
        Vector3 spawnPoint;
        float characterLength = character.GetComponent<Collider>().bounds.size.y;
        spawnPoint = character.transform.position;
        spawnPoint.y -= characterLength / 2;
        Instantiate(bombPrefab, spawnPoint, character.transform.rotation);
    }
}
