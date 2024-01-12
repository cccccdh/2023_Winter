using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    public float deleteTime = 3.0f;    // 제거 시간

    void Start()
    {
        Destroy(gameObject, deleteTime);    // 일정 시간 후 제거하기
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
