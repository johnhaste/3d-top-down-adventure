using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeA : MonoBehaviour
{

    private Animator myAnimator;
    public ParticleSystem fxHit;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region MY_METHODS

    void GetHit(int amount){
        
        myAnimator.SetTrigger("GetHit");
        fxHit.Emit(10);
        
    }

    #endregion
}
