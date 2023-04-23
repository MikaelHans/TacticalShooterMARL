using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioSensor))]
public class AudioSensorEditor : Editor
{
    private void OnSceneGUI()
    {
        //AudioSensor script = (AudioSensor)target;
        //Transform transform = script.transform;

        //Handles.color = Color.green;
        ////Handles.matrix = transform.localToWorldMatrix;
        //Vector3[] points = new Vector3[8];
        //float angle = 0f;
        //float angleIncrement = Mathf.PI / 4f;

        //for (int i = 0; i < points.Length; i++)
        //{
        //    float x = Mathf.Cos(angle) * 1;
        //    float z = Mathf.Sin(angle) * 1;
        //    points[i] = transform.position + new Vector3(x, 0f, z);
        //    angle += angleIncrement;
        //}

        //Vector3[] outerpoints = new Vector3[8];
        //for (int i = 0; i < outerpoints.Length; i++)
        //{
        //    float x = Mathf.Cos(angle) * 15;
        //    float z = Mathf.Sin(angle) * 15;
        //    outerpoints[i] = transform.position + new Vector3(x, 0f, z);
        //    angle += angleIncrement;
        //}

        //Handles.matrix = Matrix4x4.Translate(transform.localPosition);
        //Handles.matrix = Matrix4x4.Rotate(transform.localRotation);
        ////Handles.matrix = Matrix4x4.Rotate();
        ////Debug.Log(Handles.matrix);

        //Handles.DrawSolidDisc(transform.position, Vector3.forward, 0.1f);
        //Handles.DrawPolyLine(points);

        //Handles.DrawSolidDisc(transform.position, Vector3.forward, 0.1f);
        //Handles.DrawPolyLine(outerpoints);

        //for (int i = 0; i < outerpoints.Length; i++)
        //{
        //    Handles.DrawLine(points[i], outerpoints[i]);
        //}
    }
}
