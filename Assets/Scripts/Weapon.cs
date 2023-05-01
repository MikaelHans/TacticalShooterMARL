using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class Weapon : Equipment
{
    public LayerMask layerMask;
    public int damage;

    //public override float processReward()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(instantiateSource.transform.position, instantiateSource.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
    //    {
    //        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
    //        //Debug.Log(hit.collider.transform.GetComponentInParent<Character>());
    //        Character hitcharacter = hit.collider.transform.GetComponentInParent<Character>();
    //        //TargetPractice hitcharacter = hit.collider.transform.GetComponent<TargetPractice>();

    //        if (hitcharacter != null && hitcharacter.team != character.team)
    //        {
    //            //Debug.Log(hitcharacter.name);
    //            character.SetReward(0.8f);
    //            if (character.team == 0)
    //            {
    //                Debug.DrawLine(transform.position, hitcharacter.transform.position, Color.blue, 0.5f);
    //            }
    //            else
    //            {
    //                Debug.DrawLine(transform.position, hitcharacter.transform.position, Color.red, 0.5f);
    //            }
    //        }
    //        else
    //        {
    //            character.SetReward(-0.05f);
    //        }

    //    }
    //}


    protected  void Start()
    {
        instantiateSource = GetComponentInParent<Character>().shootSource;
        //audioEmiter = GetComponent<AudioEmiter>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool checkAim()
    {
        RaycastHit hit;
        if (Physics.Raycast(instantiateSource.transform.position, instantiateSource.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Character hitcharacter = hit.collider.transform.GetComponentInParent<Character>();
            //TargetPractice hitcharacter = hit.collider.transform.GetComponent<TargetPractice>();

            if (hitcharacter != null && hitcharacter.team != character.team)
            {
                //Debug.Log(hitcharacter.name);
                //if target die
                return true;
            }
            return false;
        }
        return false;
    }


    public override GameObject getAim()
    {
        RaycastHit hit;
        if (Physics.Raycast(instantiateSource.transform.position, instantiateSource.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Character hitcharacter = hit.collider.transform.GetComponentInParent<Character>();
            //TargetPractice hitcharacter = hit.collider.transform.GetComponent<TargetPractice>();

            if (hitcharacter != null && hitcharacter.team != character.team)
            {
                //Debug.Log(hitcharacter.name);
                //if target die
                return hitcharacter.gameObject;
            }
            return null;
        }
        return null;
    }
    public override bool use()
    {
        if(base.use())
        {
            fire();
            return true;
        }            
        return false;
    }

    void fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(instantiateSource.transform.position, instantiateSource.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            //Debug.Log(hit.collider.transform.GetComponentInParent<Character>());
            Character hitcharacter = hit.collider.transform.GetComponentInParent<Character>();
            //TargetPractice hitcharacter = hit.collider.transform.GetComponent<TargetPractice>();
            
            if (hitcharacter != null)
            {
                //Debug.Log(hitcharacter.name);
                //if target die
                rewardBuffer = hitcharacter.takeDamage(damage, character);
                //Debug.Log(rewardBuffer);
            }
            character.AddReward(-1f/(float)max_ammo);
            Transform hitTransform = hit.collider.transform;
            Debug.DrawLine(transform.position, hit.transform.position, Color.red, 0.5f);
        }
        //audioEmiter.EmitSound();
        character.AddReward(-1f / (float)max_ammo);
    }


    //void FixedUpdate()
    //{
    //    // Bit shift the index of the layer (8) to get a bit mask

    //    // This would cast rays only against colliders in layer 8.
    //    // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.

    //    RaycastHit hit;
    //    // Does the ray intersect any objects excluding the player layer
    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
    //        Debug.Log("Did Hit");
    //    }
    //    else
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
    //        Debug.Log("Did not Hit");
    //    }
    //}
}
