using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Equipment : MonoBehaviour
{
    public int ammo, max_ammo, magazine_size;
    public float rateOfFire;
    [SerializeField]
    protected Character character;
    public Transform instantiateSource;
    public float coolDown, rewardBuffer;
    public bool isCoolingDown;

    public void setCharacter(Character _character)
    {
        character = _character;
    }

    public virtual bool isReloading()
    {
        if (isCoolingDown)
        {
            return true;
        }
        return false;
    }

    public virtual bool checkAim()
    {
        return true;
    }

    public virtual GameObject getAim()
    {
        return null;
    }

    public virtual float processReward()
    {
        float tmp = rewardBuffer;
        rewardBuffer = 0;
        return tmp;
    }

    protected virtual void FixedUpdate()
    {
        if (coolDown > 0)
        {
            //Debug.Log("IN6");
            coolDown -= Time.deltaTime;
            if(coolDown< 0 )
            {
                isCoolingDown= false;
            }
        }
    }

    public virtual bool use()
    {        
        if (ammo <= 0 || coolDown > 0)
        {          
            if(ammo <= 0)
            {
                //character.AddReward(-0.5f);
            }
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
