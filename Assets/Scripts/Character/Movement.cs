using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float runSpeed, walkSpeed, rotationSpeed, minPitch, maxPitch, mouseSensitivity;
    [SerializeField]
    bool isRunning;
    [SerializeField]
    float moveSpeed, xRotation = 0;
    public Transform head;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        SetMoveSpeedToRun();     
        Cursor.lockState= CursorLockMode.Locked;
        //audioEmiter = GetComponent<AudioEmiter>();
        isRunning = true;
    }

    public void processMovement(ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //discreteActions[0] = Input.GetKeyDown(KeyCode.Alpha2);
        if (Input.GetKey(KeyCode.W))
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActions[0] = 2;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            discreteActions[0] = 3;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActions[0] = 4;
        }
        else
        {
            discreteActions[0] = 0;
        }
        //fire weapon
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("test");
            discreteActions[2] = 0;
        }
        else
        {
            discreteActions[2] = 1;
        }
        //move and walk
        if(Input.GetKey(KeyCode.LeftShift))
        {
            discreteActions[3] = 1;
        }
        else
        {
            discreteActions[3] = 0;
        }

        if (Input.GetAxis("Mouse X") * mouseSensitivity < 0)
        {
            discreteActions[1] = 2;
        }
        else if (Input.GetAxis("Mouse X") * mouseSensitivity > 0)
        {
            discreteActions[1] = 1;
        }
        else
        {
            discreteActions[1] = 0;
        }
        //continuousActions[1] = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //continuousActions[1] = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //continuousRotationX(Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime);
        //continuousRotationY(Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime);
    }

    public void SetMoveSpeedToRun()
    {
        moveSpeed = runSpeed;
        isRunning = true;
    }

    public void SetMoveSpeedToWalk()
    {
        moveSpeed = walkSpeed;
        isRunning = false;
    }

    public void forward()
    {
        characterController.Move(transform.forward * Time.deltaTime * moveSpeed);
        //if(isRunning)
        //    audioEmiter.EmitSound();
        
    }

    public void backward()
    {
        characterController.Move(-(transform.forward) * Time.deltaTime * moveSpeed);
        //if (isRunning)
        //    audioEmiter.EmitSound();
    }

    public void right()
    {
        characterController.Move(transform.right * Time.deltaTime * moveSpeed);
        //if (isRunning)
        //    audioEmiter.EmitSound();
    }

    public void left()
    {
        characterController.Move((-transform.right) * Time.deltaTime * moveSpeed);
        //if (isRunning)
        //    audioEmiter.EmitSound();
    }    

    public void continuousRotationX(float rotation)
    {
        xRotation -= rotation;
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);
        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //head.eulerAngles += new Vector3(rotation, 0, 0);
        //clampRotationPitch();
    }

    public void continuousRotationY(float rotation)
    {
        transform.Rotate(Vector3.up * rotation);
        //clampRotationPitch();
    }

    public void clampRotationPitch()
    {
        Vector3 rotation = head.rotation.eulerAngles;

        // Clamp the pitch angle
        float clampedPitch = Mathf.Clamp(rotation.x, maxPitch, minPitch);

        // Apply the clamped pitch angle back to the rotation
        rotation.x = clampedPitch;

        // Apply the updated rotation to the game object
        head.transform.eulerAngles = rotation;
    }

    public void rotateUP()
    {
        float x = head.eulerAngles.x;
        x += -rotationSpeed;
        Vector3 temp = head.rotation.eulerAngles;
        temp.x = x;
        head.eulerAngles =temp;
        clampRotationPitch();
    }

    public void rotateDown()
    {
        float x = head.eulerAngles.x;
        x += rotationSpeed;
        Vector3 temp = head.rotation.eulerAngles;
        temp.x = x;
        head.eulerAngles = temp;
        clampRotationPitch();
    }

    public void rotateLeft()
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, rotationSpeed, 0);    
    }

    public void rotateRight()
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, -rotationSpeed, 0);
    }    
}
