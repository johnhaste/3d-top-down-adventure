using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeA : MonoBehaviour
{

    private Animator myAnimator;
    public ParticleSystem fxHit;
    public int HP = 3;
    private bool isDead;
    public GameObject slimeBody;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        skinnedMeshRenderer = slimeBody.GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region MY_METHODS

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
