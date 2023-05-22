using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static Vector3 MinMaxNormalization(Vector3 vector, Vector3 minValue, Vector3 maxValue)
    {
        // Normalize the x, y, and z components separately
        float normalizedX = Mathf.Clamp01((vector.x - minValue.x) / (maxValue.x - minValue.x));
        float normalizedY = Mathf.Clamp01((vector.y - minValue.y) / (maxValue.y - minValue.y));
        float normalizedZ = Mathf.Clamp01((vector.z - minValue.z) / (maxValue.z - minValue.z));

        // Create a new normalized Vector3
        Vector3 normalizedVector = new Vector3(normalizedX, normalizedY, normalizedZ);
        return normalizedVector;
    }

    public static float MinMaxNormalization(float value, float minValue, float maxValue)
    {
        // Check for division by zero
        if (minValue == maxValue)
        {
            throw new ArgumentException("minValue and maxValue cannot be the same.");
        }

        // Perform Min-Max normalization
        float normalizedValue = (value - minValue) / (maxValue - minValue);
        return normalizedValue;
    }
}
