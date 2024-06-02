using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController_2 : MonoBehaviour
{
    

    void Update()
    {
        transform.Translate(Vector3.down * 4 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
