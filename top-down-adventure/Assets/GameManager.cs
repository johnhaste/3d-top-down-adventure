using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyState{
    IDLE, ALERT, EXPLORE, PATROL, FOLLOW, FURY
}

public class GameManager : MonoBehaviour
{

    [Header("Player")]
    public Transform player;

    [Header("Slime IA")]
    public Transform[] slimeWayPoints;
    public float slimeIdleWaitTime = 5f;
    public float slimeDistanceToAttack = 2.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
