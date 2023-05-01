using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : RewardingObject
{
    public Equipment[] equipments;
    public int currentlyEquipped;
    public Transform shootSource;
    protected Character character;



    private void Start()
    {
        character = GetComponent<Character>();
        //equipments = GetComponentsInChildren<Equipment>();
        Debug.Log(equipments);
        for (int i = 0; i < equipments.Length; i++)
        {
            equipments[i].setCharacter(character);
        }
    }

    public Equipment getCurrentlyEquiped()
    {
        return equipments[currentlyEquipped];
    }

    public override void processRewardPerTimestep()
    {
        character.SetReward(equipments[currentlyEquipped].processReward());
    }

    public void swap(int current, int prev)
    {
        equipments[prev].gameObject.SetActive(false);
        equipments[current].gameObject.SetActive(true);
    }

    public void swapUPDown(int i)
    {
        int temp = currentlyEquipped;
        currentlyEquipped = (currentlyEquipped + i) % equipments.Length;
        swap(currentlyEquipped, temp);
    }

    //public void swapDown()
    //{

    //    currentlyEquipped = (currentlyEquipped - 1) % equipments.Length;
    //    swap(currentlyEquipped);
    //}

    public void swapTo(int n)
    {
        int temp = currentlyEquipped;
        currentlyEquipped = n;
        swap(currentlyEquipped, temp);
    }

    public void fire()
    {
        equipments[currentlyEquipped].use();
    }

    public bool check()
    {
        return equipments[currentlyEquipped].checkAim();
    }

    public bool isReloading()
    {
        return equipments[currentlyEquipped].isReloading();
    }

    public void defuse(Bomb bomb)
    {
        bomb.isDefusing = true;
    }

    public void stopDefusing(Bomb bomb)
    {
        bomb.isDefusing = false;
    }

    public void equipBomb()
    {
        equipments[3] = GetComponentsInChildren<Equipment>(true)[3];
    }
}
