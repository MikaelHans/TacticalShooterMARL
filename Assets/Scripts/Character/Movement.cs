using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float runSpeed, walkSpeed, rotationSpeed, minPitch, maxPitch, mouseSensitivity;
    float moveSpeed;
    public Transform head;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        SetMoveSpeedToRun();
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
        discreteActions[1] = 0;
        discreteActions[2] = 0;
        continuousActions[0] = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        continuousActions[1] = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //continuousRotationX(Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime);
        //continuousRotationY(Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime);
    }

    public void SetMoveSpeedToRun()
    {
        moveSpeed = runSpeed;
    }

    public void SetMoveSpeedToWalk()
    {
        moveSpeed = walkSpeed;
    }

    public void clampRotationPitch()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        // Clamp the pitch angle
        float clampedPitch = Mathf.Clamp(rotation.x, minPitch, maxPitch);

        // Apply the clamped pitch angle back to the rotation
        rotation.x = clampedPitch;

        // Apply the updated rotation to the game object
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void forward()
    {
        characterController.Move(transform.forward * Time.deltaTime * moveSpeed);
    }

    public void backward()
    {

        characterController.Move(-(transform.forward) * Time.deltaTime * moveSpeed);
    }

    public void right()
    {
        characterController.Move(transform.right * Time.deltaTime * moveSpeed);
    }

    public void left()
    {
        characterController.Move((-transform.right) * Time.deltaTime * moveSpeed);
    }

    public void rotateUP()
    {
        head.eulerAngles = head.eulerAngles + new Vector3(-rotationSpeed, 0,0);
        clampRotationPitch();
    }

    public void continuousRotationX(float rotation)
    {
        float xRotation = -rotation;
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);
        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //head.eulerAngles += new Vector3(rotation, 0, 0);
        //clampRotationPitch();
    }

    public void continuousRotationY(float rotation)
    {
        transform.Rotate(Vector3.left * rotation);
        //clampRotationPitch();
    }

    public void rotateDown()
    {
        head.eulerAngles = head.eulerAngles + new Vector3(rotationSpeed, 0, 0);
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
