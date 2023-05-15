
using UnityEngine;
using UnityEditor;


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
