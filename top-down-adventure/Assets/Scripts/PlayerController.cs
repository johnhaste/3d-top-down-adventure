using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Config
    private GameManager _GameManager;

    private CharacterController myCharacterController;
    private Animator myAnimator;

    [Header("Config Player")]
    public int HP = 3;
    public float movementSpeed = 3f;
    private Vector3 direction;
    private bool isWalking;

    //Input
    float horizontal;
    float vertical;

    [Header("Attack Config")]
    public ParticleSystem fxAttack;
    public Transform hitBox;
    public LayerMask hitMask;
    public Collider[] hitInfo;
    [Range(0.1f,1f)]
    public float hitRange = 0.5f;
    private bool isAttacking;
    public int damageAmount;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        myCharacterController = GetComponent<CharacterController>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_GameManager.gameState != GameState.GAMEPLAY){return;}
        Inputs();
        MoveCharacter(); 
        UpdateAnimator();       
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "TakeDamage"){
            print("Damage");
            GetHit(1);
        }
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
        isAttacking = true;
        myAnimator.SetTrigger("Attack");
        fxAttack.Emit(1);
        
        //Will only hit objects inside the hitMask
        hitInfo = Physics.OverlapSphere(hitBox.position, hitRange, hitMask);

        //Loop and hit every object
        foreach(Collider c in hitInfo){
            c.gameObject.SendMessage("GetHit", damageAmount, SendMessageOptions.DontRequireReceiver);
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

    void GetHit(int amount){
        HP -= amount;
        if(HP>0){
            myAnimator.SetTrigger("GetHit");
        }else{
            _GameManager.ChangeGameState(GameState.DEAD);
            myAnimator.SetTrigger("Die");  
        }
    }

    #endregion

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitBox.position, hitRange);
    }
}
