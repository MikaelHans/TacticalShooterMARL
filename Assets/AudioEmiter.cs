using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AudioEmiter : MonoBehaviour
{
    //Character character;
    [SerializeField]
    List<Character> enemies;
    public float soundVolume;
    public int audioId;
    private void Awake()
    {
        //character = GetComponentInParent<Character>();
        if(enemies.Count == 0)
        {
            enemies = GetComponentInParent<Character>().enemies;
        }        
    }

    public void EmitSound()
    {
        foreach (Character enemy in enemies)
        {
            Vector3 enemyPos = enemy.transform.localPosition;
            Vector3 curPos = transform.localPosition;
            float distance = Vector3.Distance(enemyPos, curPos);
            if (distance < soundVolume) 
            {
                enemyPos.y = 0;
                curPos.y = 0;
                Vector3 targetDir = transform.localPosition - enemy.transform.localPosition;
                Vector3 forward = enemy.transform.forward;

                //angle
                float angle = Vector3.SignedAngle(forward, targetDir, Vector3.up);
                //Debug.Log(angle);
                angle = angle < 0 ? 180 + (180 + angle) : angle;
                //Debug.Log(angle);
                //sound magnitude
                //float soundMagnitude = soundVolume / distance;
                //enemy.audioSensor.receiveSound(angle, 1);
            }            
        }
    }
}
