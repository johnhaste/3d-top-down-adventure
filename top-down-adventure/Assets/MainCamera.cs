using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    private GameObject player;
    private Vector3 cameraInitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraInitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x - cameraInitialPosition.x,
                                         cameraInitialPosition.y,
                                         player.transform.position.z - 7f);
    }
}
