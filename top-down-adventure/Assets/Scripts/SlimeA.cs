using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeA : MonoBehaviour
{
    //Config
    private GameManager _GameManager;

    //Animation and Particles
    private Animator myAnimator;
    public ParticleSystem fxHit;
    

    //Enemy Status
    public int HP = 3;
    private bool isDead;
    public enemyState state;

    //Enemy Body
    public GameObject slimeBody;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    //AI
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
        switch(state){
            case enemyState.ALERT:
                LookAt();
                break;
            case enemyState.FOLLOW:
                LookAt();
                destination = _GameManager.player.position;
                agent.destination = destination;
                if(agent.remainingDistance <= agent.stoppingDistance){
                    Attack();
                }
                break;
            case enemyState.FURY:
                destination = _GameManager.player.position;
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
                idWaypoint = Random.Range(0, _GameManager.slimeWayPoints.Length);
                destination = _GameManager.slimeWayPoints[idWaypoint].position;
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
        }
    }

    //Stay still or walk?
    IEnumerator IDLE(){
        yield return new WaitForSeconds(_GameManager.slimeIdleWaitTime);
        StayStill(30);
    }

    //Checks if the slime has reached their destination or player
    IEnumerator PATROL(){
        yield return new WaitUntil( ()=>agent.remainingDistance <= 0);
        StayStill(30);
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
        }
    }

    void AttackIsDone(){
        StartCoroutine("ATTACK");
    }

    void LookAt(){
        Vector3 lookDirection = (_GameManager.player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _GameManager.slimeLookAtSpeed * Time.deltaTime);
    }


    void GetHit(int amount){

        //If it's dead, does'nt get hit
        if(isDead){return;}
        
        if(HP > 1){
            ChangeState(enemyState.FURY);
            myAnimator.SetTrigger("GetHit");
            fxHit.Emit(10);
            HP--;
        }else{
            myAnimator.SetTrigger("Die");
            StartCoroutine("Died");
        }
        
    }

    IEnumerator Died(){
        isDead = true;
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
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            isPlayerVisible = true;
            if (state == enemyState.IDLE || state == enemyState.PATROL){
                ChangeState(enemyState.ALERT);
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
