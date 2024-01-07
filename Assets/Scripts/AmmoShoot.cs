using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AmmoShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;    // 총알 속도
    public float shootDelay = 0.25f;    // 발사 간격
    public GameObject gunfrontPrefab;   // 총의 프리팹
    public GameObject ammoPrefab;       // 화살의 프리팹

    bool inAttack = false;              // 공격 중 여부
    GameObject gunObj;                  // 총의 게임 오브젝트

    void Start()
    {
        // 총을 플레이어 캐릭터에 배치
        Vector3 pos = transform.position;
        gunObj = Instantiate(gunfrontPrefab, pos, Quaternion.identity);
        gunObj.transform.SetParent(transform);  // 플레이어 캐릭터를 총의 부모로 설정
    }

    void Update()
    {
        if ((Input.GetButtonDown("Fire3")))
        {
            // (왼쪽 Shift) 공격 키가 눌림
            Attack();
        }

        if ((Input.GetKeyDown(KeyCode.Z)))
        {
            // (A) 공격 키가 눌림
            Attack();
        }

        // 총의 회전과 우선순위
        float gunZ = -1;    // 총의 Z 값(캐릭터보다 앞으로 설정)

        PlayerController plmv = GetComponent<PlayerController>();
        if (plmv.angleZ > 30 && plmv.angleZ < 150)
        {
            // 위 방향
            gunZ = 1;       // 총의 Z 값(캐릭터보다 뒤로 설정)
        }

        // 총의 회전
        if (plmv.angleZ <= -90 || plmv.angleZ > 90)
        {
            gunObj.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            gunObj.GetComponent<SpriteRenderer>().flipY = false;
        }
        gunObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);

        // 총의 우선 순위
        gunObj.transform.position = new Vector3(transform.position.x, transform.position.y, gunZ);
    }

    public void Attack()
    {
        // 공격 중이 아님
        if (inAttack == false)
        {
            inAttack = true;        // 공격 중으로 설정

            // 총알 발사
            PlayerController playerCnt = GetComponent<PlayerController>();
            float angleZ = playerCnt.angleZ;    // 회전 각도

            // 총알의 게임 오브젝트 만들기(진행 방향으로 회전)
            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject ammoObj = Instantiate(ammoPrefab, transform.position, r);

            // 총알을 발사할 벡터 생성
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // 총알에 힘을 가하기
            Rigidbody2D body = ammoObj.GetComponent<Rigidbody2D>();
            body.AddForce(v, ForceMode2D.Impulse);

            // 공격 중이 아님으로 설정
            Invoke("StopAttack", shootDelay);
        }
    }

    public void StopAttack()
    {
        inAttack = false;       // 공격 중이 아님으로 설정
    }
}
