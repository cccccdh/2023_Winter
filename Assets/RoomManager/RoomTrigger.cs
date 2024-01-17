using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject block;

    public GameObject Enemy;

    void Start()
    {
        block.SetActive(false);
    }

    void Update()
    {
        if (Enemy.transform.childCount == 0)
        {
            block.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            block.SetActive(true);
        }

    }
}
