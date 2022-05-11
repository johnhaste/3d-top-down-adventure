using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGrass : MonoBehaviour
{
    public ParticleSystem fxHit;
    private bool isCut;
    private BoxCollider boxCollider;
    private AudioPlayer _AudioPlayer;

    void Start(){
        boxCollider = GetComponent<BoxCollider>();
        _AudioPlayer = FindObjectOfType(typeof(AudioPlayer)) as AudioPlayer;
    }

    void GetHit(int amount){
        
        if(!isCut){
            isCut = true;
            _AudioPlayer.GrassGetsHit();
            boxCollider.isTrigger = true;
            transform.localScale = new Vector3(1f,1f,1f);
            fxHit.Emit(10);
        }
        
    }
}
