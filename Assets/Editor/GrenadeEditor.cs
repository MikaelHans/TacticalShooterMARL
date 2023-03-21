using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HEGrenade))]
public class GrenadeEditor : Editor
{
    // Start is called before the first frame update
    private void OnSceneGUI()
    {
        HEGrenade grenade = (HEGrenade)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(grenade.transform.position, Vector3.up, Vector3.forward, 360, grenade.radius);
    }
}
