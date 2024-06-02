using UnityEngine;

public class BossController : MonoBehaviour
{
    // 체력
    public int hp = 20;

    // 반응 거리
    public float reactionDistance = 10.0f;

    public GameObject bulletPrefab;         // 총알
    public GameObject[] bossBullets;
    public float shootSpeed = 5.0f;         // 총알 속도

    // 공격 중인지 여부
    bool isAttack = false;

    void Update()
    {
        if (hp > 0)
        {
            // Player 게임 오브젝트 가져오기
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // 플레이어와의 거리 확인
                Vector3 plpos = player.transform.position;
                float dist = Vector2.Distance(transform.position, plpos);
                if(dist <= reactionDistance && isAttack == false)
                {
                    // 범위 안 & 공격 중이 아니면 공격 애니메이션
                    isAttack = true;
                    // 애니메이션 변경
                    GetComponent<Animator>().Play("BossAttack");
                }
                else if( dist > reactionDistance && isAttack)
                {
                    isAttack = false;
                    // 애니메이션 변경
                    GetComponent<Animator>().Play("BossIdle");
                }
            }
            else
            {
                isAttack = false;
                // 애니메이션 변경
                GetComponent<Animator>().Play("BossAttack");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hp <= 0)
        {
            // 사망
            // 충돌 판정 비활성
            GetComponent<CircleCollider2D>().enabled = false;
            // SE 재생
            SoundManager.soundManager.SEPlay(SEType.bossDead);
            // 애니메이션 변경
            GetComponent<Animator>().Play("BossDead");
            // 1초 뒤에 제거ㅏ
            Destroy(gameObject, 1);
        }
        if (collision.gameObject.tag == "Ammo")
        {
            hp--;
        }
        if(collision.gameObject.tag == "gunitem")
        {
            hp -= 3;
        }
    }

    void AttackPatton()
    {
        int rnd = Random.Range(0, 2);
        switch (rnd)
        {
            case 0:
                Attack_1();
                break;
            case 1: 
                Attack_2(); 
                break;
        }
    }

    // 공격 1
    void Attack_1()
    {
        // 발사 위치 오브젝트 가져오기
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;

        // 총알을 발사할 벡터 만들기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;

            // 아크 탄젠트2 함수로 각도(라디안) 구하기
            float rad = Mathf.Atan2(dy, dx);

            // 라디안을 각도로 변환
            float angle = rad * Mathf.Deg2Rad;

            // Prefab으로 총알 오브젝트 만들기 (진행 방향으로 회전)
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // 발사
            Rigidbody2D rbody  = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);

            // SE 재생
            SoundManager.soundManager.SEPlay(SEType.bossAttack);
        }
    }

    // 공격 2
    void Attack_2()
    {
        float yPos = transform.position.y + 2f;
        foreach (GameObject obj in bossBullets)
        {
            if (obj != null)
            {
                obj.transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
                obj.SetActive(true);
            }
        }
    }
}
