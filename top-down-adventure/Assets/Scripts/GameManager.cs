using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public enum enemyState{
    IDLE, ALERT, EXPLORE, PATROL, FOLLOW, FURY, DEAD
}

public enum GameState{
    GAMEPLAY, DEAD
}

public class GameManager : MonoBehaviour
{

    public GameState gameState;

    [Header("Info Player")]
    public Transform player;
    private int gems;

    [Header("UI")]
    public Text txtGem;

    [Header("Slime IA")]
    public Transform[] slimeWayPoints;
    public float slimeIdleWaitTime = 5f;
    public float slimeAttackWaitTime = 1f;
    public float slimeDistanceToAttack = 2.3f;
    public float slimeAlertTime = 1f;
    public float slimeLookAtSpeed = 1f;

    [Header("Rain Manager")]
    public PostProcessVolume postB;
    public ParticleSystem rainParticle;
    private ParticleSystem.EmissionModule rainModule;
    public int rainRateOverTime;
    public int rainIncrement;
    public float rainIncrementDelay;

    [Header("Drop Item")]
    public GameObject gemPreFab;
    public int dropOdds = 50;

    private void Start(){
        rainModule = rainParticle.emission;
        txtGem.text = gems.ToString();
    }

    public void earnGems(int amount){
        gems += amount;
        txtGem.text = gems.ToString();
    }

    public bool CalculateOdds(int p){
        int temp = Random.Range(0,100);
        bool result = temp <= p ? true: false;
        return result;
    }

    public void OnOffRain(bool isRaining){
        StopCoroutine("RainManager");
        StopCoroutine("PostBManager");
        StartCoroutine("RainManager", isRaining);
        StartCoroutine("PostBManager", isRaining);
    }

    IEnumerator RainManager(bool isRaining){
        switch(isRaining){
            case true:

                //Increase rain over time
                for(float r = rainModule.rateOverTime.constant; r < rainRateOverTime; r+= rainIncrement){
                    rainModule.rateOverTime = r;
                    yield return new WaitForSeconds(rainIncrementDelay);
                }

                rainModule.rateOverTime = rainRateOverTime;
                break;
            case false:

                //Decrease rain over time
                for(float r = rainModule.rateOverTime.constant; r > 0 ; r -= rainIncrement){
                    rainModule.rateOverTime = r;
                    yield return new WaitForSeconds(rainIncrementDelay);
                }

                rainModule.rateOverTime = 0;
                break;
        }

    }

    IEnumerator PostBManager(bool isRaining){
        switch(isRaining){
            case true:

                //Increase darkness over time
                for(float w = postB.weight; w < 1; w += 1 * Time.deltaTime){
                    postB.weight = w;
                    yield return new WaitForEndOfFrame();
                }

                postB.weight = 1;
                break;
            case false:

               //Increase brightness over time
               for(float w = postB.weight; w > 0; w -= 1 * Time.deltaTime){
                    postB.weight = w;
                    yield return new WaitForEndOfFrame();
                }

                postB.weight = 0;
                break;
        }
    }

    public void ChangeGameState(GameState newState){
        gameState = newState;
    }


}
