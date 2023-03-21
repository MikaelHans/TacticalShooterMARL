using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int moveSpeed;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        transform.eulerAngles = transform.eulerAngles + new Vector3(1,0,0);
    }

    public void rotateDown()
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(-1, 0, 0);
    }

    public void rotateLeft()
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, 1, 0);
    }

    public void rotateRight()
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, -1, 0);
    }

    
}
