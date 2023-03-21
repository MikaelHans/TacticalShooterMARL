using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : Equipment
{
    // Start is called before the first frame update
    public GameObject grenadePrefab;
    public float throwForce;


    protected override void Start()
    {
        base.Start();
        instantiateSource = GetComponentInParent<Character>().throwSource;
    }
    

    public override bool use()
    {
        if(base.use())
        {
            GameObject newgrenade = Instantiate(grenadePrefab);
            newgrenade.transform.position = instantiateSource.position;
            newgrenade.GetComponent<Rigidbody>().AddForce(newgrenade.transform.forward * throwForce);
            return true;
        }
        return false;
       
    }
}
