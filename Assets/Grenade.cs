using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay;
    public bool active, exploded;
    public ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        exploded = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            active = true;
        }
    }

    private void Update()
    {
        if(active && !exploded)
        {
            if (delay <= 0)
            {
                explode();
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }
    }

    protected virtual void explode() 
    {
        particleSystem.Play();
        exploded = true;        
    }

    public virtual void afterParticleEffect()
    {

    }

    private void OnParticleSystemStopped()
    {
        Debug.Log("callde");
        afterParticleEffect();
    }
}
