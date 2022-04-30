using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController myCharacterController;
    private Animator myAnimator;

    [Header("Config Player")]
    public float movementSpeed = 3f;

    private Vector3 direction;
    private bool isWalking;

    //Input
    float horizontal;
    float vertical;

    [Header("Attack Config")]
    public ParticleSystem fxAttack;
    [SerializeField] private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        myCharacterController = GetComponent<CharacterController>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        MoveCharacter(); 
        UpdateAnimator();       
    }

    #region MY_METHODS

    void Inputs(){
        //Get input according to the input manager
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //In the input manager is the CTRL or MOUSE LEFT BUTTON
        if(Input.GetButtonDown("Fire1") && !isAttacking){
            Attack();
        }

        //Get the direction of Z and X (Normalize so diagonal won't be faster)
        direction = new Vector3(horizontal, 0f, vertical).normalized;
    }

    void Attack(){
        if(!isAttacking){
            isAttacking = true;
            myAnimator.SetTrigger("Attack");
            fxAttack.Emit(1);
        }
        
    }

    void AttackIsDone(){
        isAttacking = false;
    }

    void MoveCharacter(){
        //If it's moving
        if(direction.magnitude > 0.1){
            //Gets Tangent of the 2 positions x and z (0 to 1f) and Convert Rad to Degrees
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //Updates Y rotation
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            //Updates animation
            isWalking = true;
        }else{
            isWalking = false;
        }

        //Makes the character move using the CharacterController Componente
        //Multiplies by speed and deltatime(So FPS won't affect speed)
        myCharacterController.Move(direction * movementSpeed * Time.deltaTime);
    }

    void UpdateAnimator(){
        myAnimator.SetBool("isWalking", isWalking);
    }

    #endregion
}
