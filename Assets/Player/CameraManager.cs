using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            //플레이어의 위치와 연동시킴
            transform.position =  new Vector3(player.transform.position.x,
                            player.transform.position.y, -10);
        }
    }
}