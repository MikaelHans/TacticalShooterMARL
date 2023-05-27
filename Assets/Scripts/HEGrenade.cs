using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEGrenade : Grenade
{
    public float radius, damage;
    protected override void explode()
    {
        base.explode();

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        Debug.Log(colliders.Length);    
        foreach (Collider collider in colliders)
        {
            //add damage
            //Debug.Log($"{collider.name}");
            Character character = collider.GetComponentInParent<Character>();
            if (character != null)
            {
                float dist, actualDamage; 
                dist = Vector3.Distance(transform.position, character.transform.position);
                actualDamage = (radius - dist) / radius;

                character.takeDamage(100, thrower);
            }                
        }
        //Destroy(gameObject);
    }

    public override void afterParticleEffect()
    {
        base.afterParticleEffect();
        Destroy(gameObject); 
    }
}
