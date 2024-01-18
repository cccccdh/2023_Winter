using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public float deleteTime = 2;    // 제거 시간

    void Start()
    {
        Destroy(gameObject, deleteTime);    // 일정 시간 후 제거하기
    }

    // 게임 오브젝트에 접총
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 몬스터에 닿으면 바로 총알 제거
            Destroy(gameObject);
        }
        // 접촉한 게임 오브젝트의 자식으로 설정하기
        transform.SetParent(collision.transform);

        // 충돌 판정을 비활성
        GetComponent<CircleCollider2D>().enabled = false;

        // 물리 시뮬레이션을 비활성
        GetComponent<Rigidbody2D>().simulated = false;
    }

    void Update()
    {

    }
}
