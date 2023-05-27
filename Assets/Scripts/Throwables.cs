using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : Equipment
{
    // Start is called before the first frame update
    public GameObject grenadePrefab;
    public float throwForce;


    protected void Start()
    {
        instantiateSource = GetComponentInParent<Character>().throwSource;
    }
    

    public override bool use()
    {
        if(base.use())
        {
            GameObject newgrenade = Instantiate(grenadePrefab);
            newgrenade.transform.position = instantiateSource.position;
            newgrenade.GetComponent<Rigidbody>().AddForce(character.transform.forward * throwForce);
            newgrenade.GetComponent<Grenade>().thrower = character;
            return true;
        }
        return false;
       
    }
}
