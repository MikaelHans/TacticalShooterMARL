using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    private void OnSceneGUI()
    {
        Character character = (Character)target;
        Handles.color = Color.red;
        Handles.DrawLine(character.shootSource.position, character.shootSource.position + character.shootSource.forward * 100);
    }
}
