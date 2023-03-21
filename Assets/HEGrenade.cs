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

        foreach (Collider collider in colliders)
        {
            //add damage
            Character character = collider.GetComponent<Character>();
            if (character != null)
            {
                float dist, actualDamage; 
                dist = Vector3.Distance(transform.position, character.transform.position);
                actualDamage = (radius - dist) / radius;

                character.takeDamage(damage * actualDamage, character);
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
