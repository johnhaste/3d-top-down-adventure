using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeA : MonoBehaviour
{
    //Config
    private GameManager _GameManager;
    private AudioPlayer _AudioPlayer;

    //Animation and Particles
    private Animator myAnimator;
    public ParticleSystem fxHit;
    

    //Enemy Status
    public int HP = 3;
    public GameObject[] slimeHearts;
    private bool isDead;
    public enemyState state;

    //Enemy Body
    public GameObject slimeBody;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    //AI
    public Transform[] slimeWayPoints;
    private NavMeshAgent agent;
    private Vector3 destination;
    private int idWaypoint;
    private bool isWalking;
    private bool isAlert;
    private bool isAttacking;
    private bool isPlayerVisible;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        skinnedMeshRenderer = slimeBody.GetComponent<SkinnedMeshRenderer>();
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        _AudioPlayer = FindObjectOfType(typeof(AudioPlayer)) as AudioPlayer;
        agent = GetComponent<NavMeshAgent>();

        ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();

        if(agent.desiredVelocity.magnitude >= 0.1f){
            isWalking = true;
        }else{
            isWalking = false;
        }

        myAnimator.SetBool("isWalking", isWalking);
        myAnimator.SetBool("isAlert", isAlert);
        
    }

    #region MY_METHODS

    void StateManager(){

        //If player is dead get back to idle
        if(_GameManager.gameState == GameState.DEAD && (state == enemyState.FOLLOW || state == enemyState.FURY || state == enemyState.ALERT)){
            ChangeState(enemyState.IDLE);
        }

        switch(state){
            case enemyState.ALERT:
                LookAt();
                break;
            case enemyState.FOLLOW:
                LookAt();
                destination = _GameManager.playerTransform.position;
                agent.destination = destination;
                if(agent.remainingDistance <= agent.stoppingDistance){
                    Attack();
                }
                break;
            case enemyState.FURY:
                LookAt();
                destination = _GameManager.playerTransform.position;
                agent.destination = destination;
                if(agent.remainingDistance <= agent.stoppingDistance){
                    Attack();
                }
                break;
            case enemyState.PATROL:
                break;
        }
    }

    void ChangeState(enemyState newState){
        //print(newState);
        StopAllCoroutines();
        state = newState;
        isAlert = false;

        switch(state){
            case enemyState.IDLE:
                agent.stoppingDistance = 0;
                destination = transform.position;
                agent.destination = destination;
                StartCoroutine("IDLE");
                break;
            case enemyState.ALERT:
                agent.stoppingDistance = 0;
                destination = transform.position;
                agent.destination = destination;
                isAlert = true;
                StartCoroutine("ALERT");
                break;
            case enemyState.PATROL:
                //Randomizes the destination, picks one and makes agent move towards it
                agent.stoppingDistance = 0;
                idWaypoint = Random.Range(0,slimeWayPoints.Length);
                destination = slimeWayPoints[idWaypoint].position;
                agent.destination = destination;
                StartCoroutine("PATROL");
                break;
            case enemyState.FOLLOW:
                agent.stoppingDistance = _GameManager.slimeDistanceToAttack;
                //StartCoroutine("FOLLOW");
                break;
             case enemyState.FURY:  
                destination = transform.position;
                agent.stoppingDistance = _GameManager.slimeDistanceToAttack;
                agent.destination = destination;              
                break;
            case enemyState.DEAD:
                destination = transform.position;
                agent.destination = destination;  
                break;
        }
    }

    //Stay still or walk?
    IEnumerator IDLE(){
        yield return new WaitForSeconds(_GameManager.slimeIdleWaitTime);
        StayStill(15);
    }

    //Checks if the slime has reached their destination or player
    IEnumerator PATROL(){
        yield return new WaitUntil( ()=>agent.remainingDistance <= 0);
        StayStill(20);
    }

    IEnumerator ALERT(){
        yield return new WaitForSeconds(_GameManager.slimeAlertTime);
        if(isPlayerVisible){
            ChangeState(enemyState.FOLLOW);
        }else{
            StayStill(10);
        }
    }

    IEnumerator ATTACK(){
        yield return new WaitForSeconds(_GameManager.slimeAttackWaitTime);
        isAttacking = false;
    }

    void StayStill(int oddsToStayStill){
        if(Rand() <= oddsToStayStill){
            ChangeState(enemyState.IDLE);
        }else{
            ChangeState(enemyState.PATROL);
        }
    }

    int Rand(){
        int rand = Random.Range(0, 100);
        return rand;
    }

    void Attack(){
        if(!isAttacking && isPlayerVisible){
            isAttacking = true;
            myAnimator.SetTrigger("Attack");
            _AudioPlayer.SlimeAttacks();
        }
    }

    void AttackIsDone(){
        StartCoroutine("ATTACK");
    }

    void LookAt(){
        Vector3 lookDirection = (_GameManager.playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _GameManager.slimeLookAtSpeed * Time.deltaTime);
    }


    void GetHit(int amount){

        //If it's dead, does'nt get hit
        if(isDead){return;}

        
         _AudioPlayer.SlimeGetsHit();
        
        if(HP > 1){
            HP--;
            ChangeState(enemyState.FURY);
            myAnimator.SetTrigger("GetHit");
            StartCoroutine("Flash");
            fxHit.Emit(10);
            Destroy(slimeHearts[HP].gameObject);
        }else{
            Destroy(slimeHearts[0].gameObject);
            ChangeState(enemyState.DEAD);
            myAnimator.SetTrigger("Die");
            StartCoroutine("Died");
        }

        
    }

    IEnumerator Flash(){
        yield return new WaitForSeconds(0.2f);
        skinnedMeshRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        skinnedMeshRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        skinnedMeshRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        skinnedMeshRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        skinnedMeshRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        skinnedMeshRenderer.enabled = true;
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Died(){
        isDead = true;
        StartCoroutine("Flash");
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);

        //Checks if will or won't drop a gem
        if(_GameManager.CalculateOdds(_GameManager.dropOdds)){
            Instantiate(_GameManager.gemPreFab, new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z) , _GameManager.gemPreFab.transform.rotation);
        }
        
    }

    private void OnTriggerEnter(Collider other){
        
        if(_GameManager.gameState != GameState.GAMEPLAY){return;}

        if(other.gameObject.tag == "Player"){
            isPlayerVisible = true;
            
            if (state == enemyState.IDLE || state == enemyState.PATROL){
                ChangeState(enemyState.ALERT);
            }else if(state == enemyState.FOLLOW){
                StopCoroutine("FOLLOW");
                ChangeState(enemyState.FOLLOW);
            }
        }
    
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            isPlayerVisible = false;
        }
    }

    #endregion
}
