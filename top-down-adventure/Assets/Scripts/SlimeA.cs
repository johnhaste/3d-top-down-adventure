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
    public const float idleWaitTime = 3f;
    public const float patrolWaitTime = 5f;

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
    }

    #region MY_METHODS

    void StateManager(){
        switch(state){
            case enemyState.IDLE:
                break;
            case enemyState.ALERT:
                break;
            case enemyState.EXPLORE:
                break;
            case enemyState.FOLLOW:
                break;
            case enemyState.FURY:
                break;
            case enemyState.PATROL:
                break;
        }
    }

    void ChangeState(enemyState newState){
        
        StopAllCoroutines();
        state = newState;

        switch(state){
            case enemyState.IDLE:
                destination = transform.position;
                agent.destination = destination;
                StartCoroutine("IDLE");
                break;
            case enemyState.ALERT:
                break;
            case enemyState.EXPLORE:
                break;
            case enemyState.FOLLOW:
                break;
            case enemyState.FURY:
                break;
            case enemyState.PATROL:
                idWaypoint = Random.Range(0, _GameManager.slimeWayPoints.Length);
                destination = _GameManager.slimeWayPoints[idWaypoint].position;
                agent.destination = destination;
                StartCoroutine("PATROL");
                break;
        }
    }

    //Stay still or walk?
    IEnumerator IDLE(){
        yield return new WaitForSeconds(idleWaitTime);
        StayStill(50);
    }

    IEnumerator PATROL(){
        yield return new WaitForSeconds(patrolWaitTime);
        StayStill(30);
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

    void GetHit(int amount){

        //If it's dead, does'nt get hit
        if(isDead){return;}
        
        if(HP > 1){
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

    #endregion
}
