using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class AudioSensor : MonoBehaviour
{
    public float[] sensorData;
    public List<float[]> sensorDatas = new List<float[]>();
    public int sensorGridCount, sensorCount;
    public float sensorOffset;
    float binSize;
    private void Awake()
    {
        for (int i = 0;i < sensorGridCount; i++)
        {
            sensorDatas.Add(new float[sensorGridCount]);
        }
        sensorData = new float[sensorGridCount];
        binSize = 360 / sensorGridCount;
    }

    public void receiveSound(float angleMagnitude, float soundMagnitude)
    {
        //Debug.Log("Angle: " + angleMagnitude.ToString());
        int gridPlacement = ((int)((angleMagnitude + sensorOffset) / binSize) % sensorGridCount);
        //Debug.Log(gridPlacement);
        sensorData[gridPlacement] = soundMagnitude;
    }

    public void resetSensorData()
    {
        for(int i=0;i<sensorData.Length;i++)
        {
            sensorData[i] = 0;
        }
    }
}
