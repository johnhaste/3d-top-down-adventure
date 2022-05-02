using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    //Config
    private GameManager _GameManager;
    public bool isRaining;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            print("Change");
            _GameManager.OnOffRain(isRaining);
        }
    }
}
