using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
     //Config
    private GameManager _GameManager;
    private GameObject camB; 

    void Start(){
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        camB = GameObject.FindGameObjectWithTag("Camera2");
        if(camB != null){
            camB.SetActive(false);
        }
    }
    
    private void OnTriggerEnter(Collider other){

        switch(other.gameObject.tag){
            case "CamTrigger":
                camB.SetActive(true);
                break;
            case "Collectable":
                _GameManager.earnGems(1);
                Destroy(other.gameObject);
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
