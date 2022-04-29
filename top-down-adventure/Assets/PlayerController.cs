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

        //Get the direction of Z and X
        direction = new Vector3(horizontal, 0f, vertical);
        //Makes the character move using the CharacterController Componente
        //Multiplies by speed and deltatime(So FPS won't affect speed)
        myCharacterController.Move(direction * movementSpeed * Time.deltaTime);
    }
}
