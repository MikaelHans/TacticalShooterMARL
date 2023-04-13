using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGrenade : Grenade
{
    public float radius, stunTime;
    protected override void explode()
    {
        base.explode();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            //add damage
            Character character = collider.GetComponent<Character>();
            if (character != null)
            {
                character.stun(stunTime);
            }
        }
    }
}
