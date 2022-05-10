using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    //Config
    private GameManager _GameManager;
    public ParticleSystem fxHit;
    private bool isCut;
    int grassDropOdds = 25;

    void Start(){
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    void GetHit(int amount){
        
        if(!isCut){
            isCut = true;
            transform.localScale = new Vector3(1f,1f,1f);
            fxHit.Emit(10);
            //Checks if will or won't drop a gem
            if(_GameManager.CalculateOdds(grassDropOdds)){
                Instantiate(_GameManager.gemPreFab, new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z) , _GameManager.gemPreFab.transform.rotation);
            }
        }
        
    }
}
