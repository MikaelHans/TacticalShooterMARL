using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Equipment : MonoBehaviour
{
    public int ammo, max_ammo, magazine_size;
    public float rateOfFire;
    protected Character character;
    public Transform instantiateSource;
    public float coolDown;
    public bool isCoolingDown;

    protected virtual void Start()
    {        
        character = GetComponentInParent<Character>();
    }

    protected virtual void Update()
    {

    }

    public virtual void processReward()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (isCoolingDown)
        {
            coolDown -= Time.deltaTime;
            if(coolDown< 0 )
            {
                isCoolingDown= false;

            }
        }
    }

    public virtual bool use()
    {        
        if (ammo <= 0 || isCoolingDown)
        {           
            return false;
        }
        coolDown = rateOfFire;
        ammo--;
        isCoolingDown = true;
        return true;
    }

    public virtual void resetEquipment()
    {
        ammo = max_ammo;
        coolDown = rateOfFire;
        isCoolingDown= false;
    }

    //public virtual void reload()
    //{

    //}
}
