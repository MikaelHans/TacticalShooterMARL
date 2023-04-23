using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AudioSensorGizmo : MonoBehaviour
{
    public float radius = 1f, outerRadius = 8f;
    public int numPoints = 8;
    public float rotationOffset = 45f;

    private void OnDrawGizmos()
    {
        Vector3 center = transform.position;
        Quaternion rotation = Quaternion.Euler(0f, rotationOffset, 0f) * transform.rotation;
        Vector3[] points = new Vector3[numPoints];
        Vector3[] outerPoints= new Vector3[numPoints];
        Handles.color = Color.green;

        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * (360f / numPoints);
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            points[i] = center + rotation * (direction * radius);
        }

        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * (360f / numPoints);
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            outerPoints[i] = center + rotation * (direction * outerRadius);
        }

        for (int i = 0; i < numPoints; i++)
        {
            Handles.DrawLine(points[i], outerPoints[i]);
        }
        
        Handles.DrawPolyLine(points);
        Handles.DrawPolyLine(outerPoints);
    }
}
