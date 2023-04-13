using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SmokeGrenade : Grenade
{
    public GameObject smoke;

    public Collider smokeCollider;
    public float shrinkRate, smokeTime;
    protected override void explode()
    {
        base.explode();
        Debug.Log("punten");
        StartCoroutine(SmokeActive(smokeTime));

    }

    IEnumerator SmokeActive(float smokeTime)
    {
        smokeCollider.enabled = true;
        yield return new WaitForSeconds(smokeTime);
        particleSystem.Stop();
        StartCoroutine(SmokeFade());
        
        //SmokeFade();
    }

    IEnumerator SmokeFade()
    {
        while(smoke.transform.localScale.sqrMagnitude > 1)
        {
            smoke.transform.localScale = Vector3.Lerp(smoke.transform.localScale, Vector3.zero, shrinkRate);
            yield return null;
        }        
    }
}
