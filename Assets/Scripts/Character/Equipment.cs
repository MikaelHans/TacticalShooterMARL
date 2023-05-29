using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public int ammo, max_ammo, magazine_size;
    public float rateOfFire;
    [SerializeField]
    protected Character character;
    public Transform instantiateSource;
    public float coolDown, rewardBuffer;
    public bool isCoolingDown;
    public TextMeshProUGUI reloadUI;

    private void Start()
    {
        if (character.inference)
        {
            reloadUI = GameObject.FindGameObjectWithTag("reloadUI").GetComponent<TextMeshProUGUI>();
        }       
    }

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
        if (isCoolingDown)
        {
            if (character.inference)
            {
                reloadUI.text = $"{coolDown}";
            }
            
            coolDown -= Time.deltaTime;
            if(coolDown< 0 )
            {
                isCoolingDown= false;
            }
        }
        else
        {
            if (character.inference)
            {
                reloadUI.text = "Ready";
            }
            
        }
    }

    public virtual bool use()
    {        
        if (ammo <= 0 || isCoolingDown)
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
