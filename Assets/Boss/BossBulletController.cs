using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    public float deleteTime = 3.0f;    // ���� �ð�

    void Start()
    {
        Destroy(gameObject, deleteTime);    // ���� �ð� �� �����ϱ�
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
