using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
     //Config
    private GameManager _GameManager;
    private AudioPlayer _AudioPlayer;
    private GameObject camB; 

    void Start(){
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        _AudioPlayer = FindObjectOfType(typeof(AudioPlayer)) as AudioPlayer;
        float fullSpeed = GetComponent<PlayerController>().currentMovementSpeed;
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
                _GameManager.EarnGems(1);
                _AudioPlayer.RupeePickup();
                Destroy(other.gameObject);
                break;
            case "Grass":
                if(other.gameObject.transform.localScale.x > 1){
                    GetComponent<PlayerController>().currentMovementSpeed = 1f;
                }
                break;

        }

    }

    private void OnTriggerExit(Collider other){

        switch(other.gameObject.tag){
            case "CamTrigger":
                camB.SetActive(false);
                break;
            case "Grass":
                GetComponent<PlayerController>().currentMovementSpeed = GetComponent<PlayerController>().maxMovementSpeed;
                break;
        }

    }
}
