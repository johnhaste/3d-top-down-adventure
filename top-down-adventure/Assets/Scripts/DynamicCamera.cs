using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject camB; 
    
    private void OnTriggerEnter(Collider other){
        
        switch(other.gameObject.tag){
            case "CamTrigger":
                camB.SetActive(true);
                break;
        }

    }

    private void OnTriggerExit(Collider other){

        switch(other.gameObject.tag){
            case "CamTrigger":
                camB.SetActive(false);
                break;
        }

    }
}
