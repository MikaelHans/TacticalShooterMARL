using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
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
    Character character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
        characterController = GetComponent<CharacterController>();
        SetMoveSpeedToRun();     
        Cursor.lockState= CursorLockMode.Locked;
        //audioEmiter = GetComponent<AudioEmiter>();
        isRunning = true;
    }

    private void FixedUpdate()
    {
        if (character.inference)
        {
            processMovement();
        }        
    }

    public void processMovement()
    {
        //ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        //ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //discreteActions[0] = Input.GetKeyDown(KeyCode.Alpha2);
        if (Input.GetKey(KeyCode.W))
        {
            forward();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            backward();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            right();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            left();
        }
        //else
        //{
        //    discreteActions[0] = 0;
        //}
        //fire weapon
        if(Input.GetMouseButtonDown(0))
        {
            character.equipmentManager.fire();
        }

        //continuousActions[1] = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //continuousActions[1] = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        continuousRotationX(Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime);
        continuousRotationY(Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime);
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
        Vector3 rotation = head.localRotation.eulerAngles;

        // Clamp the pitch angle
        float clampedPitch = 0f;
        if(rotation.x < 360 - maxPitch)
        {
            //Debug.LogAssertion(rotation.x);
            //Debug.LogAssertion(maxPitch);
            clampedPitch = 360 - maxPitch;
        }
        else if(rotation.x < minPitch)
        {
            clampedPitch = minPitch;
        }
        else
        {
            clampedPitch = rotation.x;
        }
        Debug.Log(clampedPitch);
        // Apply the clamped pitch angle back to the rotation
        rotation.x = clampedPitch;

        // Apply the updated rotation to the game object
        head.transform.localRotation = Quaternion.Euler(rotation.x, 0,0); 
    }

    public void rotateUP()
    {
        //float x = head.localRotation.eulerAngles.x;
        //x += -rotationSpeed;
        //Vector3 temp = head.localRotation.eulerAngles;
        //temp.x = x;
        //head.localRotation = Quaternion.Euler(temp.x, temp.y, temp.z);
        head.Rotate(-rotationSpeed,0,0);
        clampRotationPitch();
    }

    public void rotateDown()
    {
        float x = head.localRotation.eulerAngles.x;
        x += rotationSpeed;
        Vector3 temp = head.localRotation.eulerAngles;
        temp.x = x;
        head.localRotation = Quaternion.Euler(temp.x, temp.y, temp.z);
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
