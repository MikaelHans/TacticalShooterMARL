using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterController characterController;
    EquipmentManager equipmentManager;
    public int moveSpeed, walkSpeed;
    Movement movement;
    Character character;
    void Start()
    {
        equipmentManager=GetComponentInParent<EquipmentManager>();
        characterController = GetComponent<CharacterController>();
        movement = GetComponent<Movement>();
        character = GetComponent<Character>();  
    }

    // Update is called once per frame
    void Update()
    {
        //movement.left();

        if (Input.GetMouseButtonDown(0))
        {
            equipmentManager.fire();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            equipmentManager.swapTo(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            equipmentManager.swapTo(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            equipmentManager.swapTo(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            equipmentManager.swapTo(3);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if(character.bombInRange == 1)
            {
                equipmentManager.defuse(character.bombRef);
            }            
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (character.bombInRange == 1)
            {
                equipmentManager.stopDefusing(character.bombRef);
            }
        }
    }
}
