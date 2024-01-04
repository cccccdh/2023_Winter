using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float deleteTime = 3.0f;         // 제거할 시간 지정


    void Start()
    {
        Destroy(gameObject, deleteTime);    // 제거 설정
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);                // 접촉이 발생하면 즉시 제거
    }
}
