using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using log4net.Util;

[CustomEditor(typeof(AudioEmiter))]

public class AudioEmiterEditor : Editor
{

    private void OnSceneGUI()
    {
        AudioEmiter audioEmiter = (AudioEmiter)target;
        Handles.color = Color.white;
        Handles.DrawWireDisc(audioEmiter.transform.position, Vector3.up, audioEmiter.soundVolume);
    }
}
