using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 이동 속도
    public float speed = 3.0f;

    // 애니메이션 이름
    public string upAnime = "PlayerUp";         // 위
    public string downAnime = "PlayerDown";     // 아래
    public string rightAnime = "PlayerRight";   // 오른쪽
    public string leftAnime = "PlayerLeft";     // 왼쪽
    public string deadAnime = "PlayerDead";     // 사망

    // 현재 애니메이션
    string nowAnimation = "";

    // 이전 애니메이션
    string oldAnimation = "";

    float axisH;                        // 가로축 값(-1.0 ~ 1.0)
    float axisV;                        // 세로축 값(-1.0 ~ 1.0)
    public float angleZ = -90.0f;       // 회전각

    Rigidbody2D rbody;                  // Rigidbody2D
    bool isMoving = false;              // 이동 중인지 여부

    //대미지 처리
    public static int hp = 3; //플레이어 HP
    public static string gameState; // 게임 상태
    bool inDamage = false; // 대미지 받는 중인지 여부

    void Start()
    {
        //RIgidbody2d 가져오기
        rbody = GetComponent<Rigidbody2D>();
        //애니메이션
        oldAnimation = downAnime;
        //게임 상태를 플레이 중으로 변경
        gameState = "playing";
        //hp 갱신
        hp = PlayerPrefs.GetInt("PlayerHP");

    }

    void Update()
    {
        //게임 중이 아니거나 대미지를 받는 중에는 아무것도 하지 않음
        if (gameState != "playing" || inDamage)
        {
            return;
        }

        if (isMoving == false)
        {
            axisH = Input.GetAxis("Horizontal");    // 좌우 키 입력
            axisV = Input.GetAxis("Vertical");      // 상하 키 입력
        }

        // 키 입력으로 이동 각도 구하기
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);

        // 이동 각도에서 방향과 애니메이션 변경
        if (angleZ >= -45 && angleZ < 45)
        {
            // 오른쪽
            nowAnimation = rightAnime;
        }else if(angleZ >= 45 && angleZ <= 135)
        {
            // 위쪽
            nowAnimation = upAnime;
        }
        else if (angleZ >= -135 && angleZ <= -45)
        {
            // 아래쪽
            nowAnimation = downAnime;
        }
        else
        {
            // 왼쪽
            nowAnimation = leftAnime;
        }
        
        // 애니메이션 변경하기
        if(nowAnimation != oldAnimation)
        {
            oldAnimation = nowAnimation;
            GetComponent<Animator>().Play(nowAnimation);
        }
    }

    void FixedUpdate()
    {
        //게임 중이 아니면 아무것도 하지 않음
        if(gameState != "playing")
        {
            return;
        }
        if (inDamage)
        {
            //대미지를 받는 중에는 점멸시키기
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                //스프라이트 표시
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //스프라이트 비표시
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return; // 대미지를 받는 중에는 조작할 수 없게 하기
        }
        // 이동 속도 변경하기
        rbody.velocity = new Vector2(axisH, axisV) * speed;
    }

    public void SetAxis(float h, float v)
    {
        axisH = h;
        axisV = v;
        if (axisH == 0 && axisV == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    // p1에서 p2까지의 각도 계산
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if(axisH != 0 || axisV != 0)
        {
            // 이동 중이면 각도를 변경
            // p1 과 p2 차이 구하기 (원점을 0으로 하기 위해)
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;

            // 아크 탄젠트 함수로 각도(라디안) 구하기
            float rad = Mathf.Atan2(dy, dx);

            // 라디안 각으로 변환하여 반환
            angle = rad * Mathf.Rad2Deg;
        }else
        {
            // 정지 중이면 이전 각도를 유지
            angle = angleZ;
        }

        return angle;
    }

    //접촉(물리)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
    }

    // 데미지
    void GetDamage(GameObject enemy)
    {
        if (gameState == "playing")
        {
            hp--;   //HP감소
            // SE 재생
            SoundManager.soundManager.SEPlay(SEType.hurt);
            //HP 갱신
            PlayerPrefs.SetInt("PlayerHP", hp);
            if (hp > 0)
            {
                //이동 중지
                rbody.velocity = new Vector2(0, 0);
                //적 캐릭터의 반대 방향으로 히트백 
                Vector3 toPos = (transform.position - enemy.transform.position).normalized;
                rbody.AddForce(new Vector2(toPos.x * 4,
                                           toPos.y * 4),
                                           ForceMode2D.Impulse);
                //데미지 받는 중으로 설정
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                //게임 오버
                GameOver();
            }
        }
    }

    //데미지 받기 끝
    void DamageEnd()
    {
        //데미지 받는 중이 아님으로 설정
        inDamage = false;
        //스프라이트 되돌리기
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // 게임 오버
    void GameOver()
    {
        Debug.Log("게임 오버");
        gameState = "gameover";
        //=====================
        // 게임 오버 연출
        //=====================
        // 플레이어 충돌 판정 비활성화
        GetComponent<CircleCollider2D>().enabled = false;
        // 이동 중지
        rbody.velocity = new Vector2(0, 0);
        // 중력을 적용해 플레이어를 위로 튀어 오르게 하는 연출
        rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        // 애니메이션 변경하기
        GetComponent<Animator>().Play(deadAnime);
        //1초 후에 플레이어 캐릭터 제거하기
        Destroy(gameObject, 1.0f);
        // BGM 정지
        SoundManager.soundManager.StopBgm();
        // SE 재생(게임 오버)
        SoundManager.soundManager.SEPlay(SEType.GameOver);
    }
}
