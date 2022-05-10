using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGrass : MonoBehaviour
{
    public ParticleSystem fxHit;
    private bool isCut;
    private BoxCollider boxCollider;

    void Start(){
        boxCollider = GetComponent<BoxCollider>();
    }

    void GetHit(int amount){
        
        if(!isCut){
            isCut = true;
            boxCollider.isTrigger = true;
            transform.localScale = new Vector3(1f,1f,1f);
            fxHit.Emit(10);
        }
        
    }
}
