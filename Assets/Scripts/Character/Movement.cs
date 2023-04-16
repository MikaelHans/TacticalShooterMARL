using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed, rotationSpeed, minPitch, maxPitch;
    public Transform head;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        transform.eulerAngles = transform.eulerAngles + new Vector3(rotationSpeed, 0,0);
        clampRotationPitch();
    }

    public void rotateDown()
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(-rotationSpeed, 0, 0);
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
