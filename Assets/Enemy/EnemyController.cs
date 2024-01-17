using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // HP
    public int hp = 3;
    // 이동속도
    public float speed = 0.5f;
    // 반응 거리
    public float reactionDistance = 4.0f;
    // 애니메이션 이름
    public string idleAnime = "EnemyIdle";      // 정지
    public string upAnime = "EnemyUp";          // 위
    public string downAnime = "EnemyDown";      // 아래
    public string rightAnime = "EnemyRight";    // 오른쪽
    public string leftAnime = "EnemyLeft";      // 왼쪽
    public string deadAnime = "EnemyDead";      // 사망
    // 현재 애니메이션
    string nowAnimation = "";
    // 이전 애니메이션
    string oldAnimation = "";

    float axisH;                // 가로 축 값
    float axisV;                // 세로 축 값
    Rigidbody2D rbody;

    bool isActive = false;      // 활성 여부

    public int arrangeId = 0;   // 배치 식별에 사용

    void Start()
    {
        // Rigidbody2D 가져오기
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Player 게임 오브젝트 가져오기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (isActive)
            {
                // 플레이어와의 각도 구하기
                float dx = player.transform.position.x - transform.position.x;
                float dy = player.transform.position.y - transform.position.y;
                float rad = Mathf.Atan2(dy, dx);
                float angle = rad * Mathf.Rad2Deg;
                // 이동 각도에 따른 애니메이션 설정
                if (angle > -45.0f && angle <= 45.0f)
                {
                    nowAnimation = rightAnime;
                }
                else if (angle > 45.0f && angle <= 135.0f)
                {
                    nowAnimation = upAnime;
                }
                else if (angle >= -135.0f && angle <= -45.0f)
                {
                    nowAnimation = downAnime;
                }
                else
                {
                    nowAnimation = leftAnime;
                }
                // 이동할 벡터 만들기
                axisH = Mathf.Cos(rad) * speed;
                axisV = Mathf.Sin(rad) * speed;
            }
            else
            {
                // 플레이어와의 거리 확인
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if(dist < reactionDistance)
                {
                    isActive = true;        // 활성으로 설정
                }
                else
                {
                    rbody.velocity = Vector2.zero;
                }
            }
        } 
        else if(isActive)
        {
            isActive = false;
            rbody.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if(isActive && hp > 0)
        {
            // 이동
            rbody.velocity = new Vector2(axisH, axisV);
            if(nowAnimation != oldAnimation)
            {
                // 애니메이션 변경하기
                oldAnimation = nowAnimation;
                Animator animator = GetComponent<Animator>();
                animator.Play(nowAnimation);
            }
        }
    }
    public bool IsMoving()
    {
        return isActive && hp > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ammo")
        {
            if(isActive)
            {
                // 데미지
                hp--;
                if (hp <= 0)
                {
                    // 사망
                    // ===================
                    // 사망 연출
                    // ===================
                    // 충돌 판정 비활성
                    GetComponent<CircleCollider2D>().enabled = false;
                    // SE 재생
                    SoundManager.soundManager.SEPlay(SEType.enemyDead);
                    // 이동 정지
                    rbody.velocity = new Vector2(0, 0);
                    // 애니메이션 변경
                    Animator animator = GetComponent<Animator>();
                    animator.Play(deadAnime);
                    // 0.5초 후에 제거
                    Destroy(gameObject, 0.5f);
                    //배치 Id 기록
                    SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
                }
            }
        }
        else if (collision.gameObject.tag == "gunitem")
        {
            if (isActive)
            {
                // 데미지
                hp-=3;
                if (hp <= 0)
                {
                    // gunitem 태그가 붙어있는 오브젝트에 맞으면 한 번에 사망
                    // ===================
                    // 사망 연출
                    // ===================
                    // 충돌 판정 비활성
                    GetComponent<CircleCollider2D>().enabled = false;
                    // SE 재생
                    SoundManager.soundManager.SEPlay(SEType.enemyDead);
                    // 이동 정지
                    rbody.velocity = new Vector2(0, 0);
                    // 애니메이션 변경
                    Animator animator = GetComponent<Animator>();
                    animator.Play(deadAnime);
                    // 0.5초 후에 제거
                    Destroy(gameObject, 0.5f);
                    // 배치 Id 기록
                    SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
                }
            }
            
        }
    }
}
