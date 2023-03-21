using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Weapon : Equipment
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public LayerMask layerMask;
    public int damage;


    public override void processReward()
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
                character.AddReward(0.8f);
                if (character.team == 0)
                {
                    Debug.DrawLine(transform.position, hitcharacter.transform.position, Color.blue, 0.5f);
                }
                else
                {
                    Debug.DrawLine(transform.position, hitcharacter.transform.position, Color.red, 0.5f);
                }
            }
            else
            {
                character.AddReward(-0.8f);
            }

        }
    }

    protected override void Start()
    {
        base.Start();
        instantiateSource = GetComponentInParent<Character>().shootSource;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        
    }

    public void checkAim()
    {

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
                Debug.Log(hitcharacter.name);
                hitcharacter.takeDamage(damage, this.character);
            }
            else
            {
                character.AddReward(-0.3f);
            }
        }
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
