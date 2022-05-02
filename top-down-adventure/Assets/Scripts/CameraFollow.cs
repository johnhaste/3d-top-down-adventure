using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private PlayerController player;

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<PlayerController>();
        transform.position = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);        
    }
}
