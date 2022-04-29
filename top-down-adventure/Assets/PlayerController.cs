using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController myCharacterController;

    [Header("Config Player")]
    public float movementSpeed = 3f;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        myCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get input according to the input manager
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Get the direction of Z and X (Normalize so diagonal won't be faster)
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        //If it's moving
        if(direction.magnitude > 0.1){
            //Gets Tangent of the 2 positions x and z (0 to 1f) and Convert Rad to Degrees
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //Updates Y rotation
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }

        //Makes the character move using the CharacterController Componente
        //Multiplies by speed and deltatime(So FPS won't affect speed)
        myCharacterController.Move(direction * movementSpeed * Time.deltaTime);
    }
}
