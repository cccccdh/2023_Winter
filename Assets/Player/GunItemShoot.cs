using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItemShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;    // 총알 속도
    public float shootDelay = 0.25f;    // 발사 간격
    public GameObject GunitemPrefab;   // 총의 프리팹
    public GameObject ammoPrefab;       // 화살의 프리팹

    bool inAttack = false;              // 공격 중 여부
    GameObject gunitemObj;              // 총의 게임 오브젝트
    bool hasGunItem = false;            // 총 아이템을 가지고 있는지 여부

    void Start()
    {
        // 총을 플레이어 캐릭터에 배치
        Vector3 pos = transform.position;
        gunitemObj = Instantiate(GunitemPrefab, pos, Quaternion.identity);
        gunitemObj.transform.SetParent(transform);  // 플레이어 캐릭터를 총의 부모로 설정
        gunitemObj.SetActive(false);  // 처음에는 비활성화

        hasGunItem = ItemKeeper.hasGunItem > 0;
        gunitemObj.SetActive(hasGunItem);
    }

    void Update()
    {
        if (hasGunItem)
        {
            // 총 아이템을 가지고 있을 때만 공격 허용
            if (Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.Z))
            {
                Attack();
            }
            if ((Input.GetKeyDown(KeyCode.Z)))
            {
                // (A) 공격 키가 눌림
                Attack();
            }
            if ((Input.GetKeyDown(KeyCode.Space)))
            {
                PlayerController player = GetComponent<PlayerController>();
                player.speed += 2.0f;
            }

            // 총의 회전과 우선순위
            float gunZ = -2;    // 총의 Z 값(캐릭터보다 앞으로 설정)

            PlayerController plmv = GetComponent<PlayerController>();
            if (plmv.angleZ > 30 && plmv.angleZ < 150)
            {
                // 위 방향
                gunZ = 4;       // 총의 Z 값(캐릭터보다 뒤로 설정)
            }

            // 총의 회전
            if (plmv.angleZ <= -90 || plmv.angleZ > 90)
            {
                gunitemObj.GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                gunitemObj.GetComponent<SpriteRenderer>().flipY = false;
            }
            gunitemObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);

            // 총의 우선 순위
            gunitemObj.transform.position = new Vector3(transform.position.x, transform.position.y, gunZ);

            if (ItemKeeper.hasGunItem > 0)
            {
                // 총알이 1개 이상이면 활성화
                EnableGun();
            }
            else
            {
                // 총알이 0개면 비활성화
                DisableBullet();
            }
        }
    }

    public void EnableGun()
    {
        // 총 아이템을 획득할 때 호출되는 메서드
        hasGunItem = true;
        gunitemObj.SetActive(true);
    }

    public void DisableBullet()
    {
        // 아이템을 먹기 전에는 총알 발사 비활성화
        hasGunItem = false;
        gunitemObj.SetActive(false);

    }

    public void Attack()
    {
        // 총 아이템을 가지고 있고, 공격 중이 아님
        if (hasGunItem && !inAttack)
        {
            gunitemObj.SetActive(false);
            ItemKeeper.hasGunItem -= 1;   //총알 소모
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
        if (ItemKeeper.hasGunItem <= 0)
        {
            DisableBullet();
        }
    }
}