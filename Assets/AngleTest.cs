using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{
    public Transform target;


    private void Update()
    {
        Vector3 targetDir = target.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
        Debug.Log(angle);
    }
}
