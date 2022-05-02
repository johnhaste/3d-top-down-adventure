using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    private GameObject camB; 

    void Start(){
        camB = GameObject.FindGameObjectWithTag("Camera2");
        camB.SetActive(false);
    }
    
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
