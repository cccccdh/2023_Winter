using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || (collision.gameObject.CompareTag("Wall")))
        {
            ObjectPool.ReturnObjectBullet(this);
        }
    }
}
