using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleSystemCallback : MonoBehaviour
{
    public delegate void afterParticleStopped();
    public afterParticleStopped particleStoppedMethod;
    public UnityEvent onParticleEnd;

    public void OnParticleSystemStopped()
    {
        Debug.Log("in");
        onParticleEnd.Invoke();
    }


}
