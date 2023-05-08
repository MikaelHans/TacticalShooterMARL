using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class Weapon : Equipment
{
    public LayerMask layerMask;
    public int damage;




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
}
