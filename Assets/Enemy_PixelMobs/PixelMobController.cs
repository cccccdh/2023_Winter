using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum MobType
{
    Eyeball,
    Count,
}

public class PixelMobController : MonoBehaviour
{
    public MobType mob;
    public Image hp_bar;                // 체력 바 UI

    public int hp;                      // 체력
    public int maxHp;                   // 최대 체력
    public float speed;                 // 이동 속도
    public float reactionDistance;      // 반응 거리

    Animator ani;
    Rigidbody2D rbody;
    SpriteRenderer spriterender;

    float axisH;                // 가로 축 값
    float axisV;                // 세로 축 값    
    bool isActive = false;      // 이동 활성 여부
    bool isAttacking = false;   // 공격 여부
    public int arrangeId = 0;   // 배치 식별에 사용

    void Start()
    {
        Init();
        SetMobStatus();        
    }

    private void SetMobStatus()
    {
        if (mob == MobType.Eyeball)
        {
            hp = 3;
            hp = maxHp;
            speed = 0.5f;
            reactionDistance = 6.0f;
            UpdateHpBar();
        }
        else if(mob == MobType.Count)
        {
            hp = 4;
            hp = maxHp;
            speed = 0.2f;
            reactionDistance = 6.0f;
            UpdateHpBar();
        }
    }

    void Init()
    {
        ani = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        // Player 게임 오브젝트 가져오기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (isActive)
            {
                if (!isAttacking)
                {
                    HandleActiveState(player);
                }
                CheckAttackDistance(player);
            }
            else
            {
                CheckReactionDistance(player);
            }
        }
        else if (isActive)
        {
            Deactivate();
        }        
    }

    void FixedUpdate()
    {
        if (isActive && hp > 0)
        {
            // 이동
            rbody.velocity = new Vector2(axisH, axisV);
            ani.SetBool("Walk", isActive);
        }
    }

    private void HandleActiveState(GameObject player)
    {
        // 플레이어와의 각도 구하기
        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        float rad = Mathf.Atan2(dy, dx);
        float angle = rad * Mathf.Rad2Deg;
        // 이동 각도에 따른 스프라이트 설정
        if (angle > -90.0f && angle <= 90.0f)
        {
            spriterender.flipX = true;
        }
        else
        {
            spriterender.flipX = false;
        }
        // 이동할 벡터 만들기
        axisH = Mathf.Cos(rad) * speed;
        axisV = Mathf.Sin(rad) * speed;
    }

    // 플레이어와의 거리 체크
    private void CheckReactionDistance(GameObject player)
    {        
        float dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist < reactionDistance)
        { 
            isActive = true; 
        }
        else
        {
            rbody.velocity = Vector2.zero;
        }
    }

    private void CheckAttackDistance(GameObject player)
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist <= 1f)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                ani.SetTrigger("Attack");
                rbody.velocity = Vector2.zero;
                Invoke("EndAttack", 1.0f);
            }
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    // Idle 함수
    private void Deactivate()
    {
        isActive = false;
        rbody.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ammo" || collision.gameObject.tag == "gunitem")
        {
            HandleCollision(collision);
        }
    }

    // 데미지 받는 함수
    private void HandleCollision(Collision2D collision)
    {
        if (isActive)
        {
            int damage = collision.gameObject.tag == "gunitem" ? 3 : 1;
            hp -= damage;
            UpdateHpBar();

            if (hp <= 0)
            {
                Die(collision.gameObject.tag);
            }
        }
    }

    private void UpdateHpBar()
    {
        if (hp_bar != null)
        {
            hp_bar.fillAmount = (float)hp / maxHp;
        }
    }

    // 죽었을때 나오는 함수
    private void Die(string tag)
    {
        // Disable collision detection
        GetComponent<CircleCollider2D>().enabled = false;
        // Play sound effect
        SoundManager.soundManager.SEPlay(SEType.enemyDead);
        // Stop movement
        rbody.velocity = Vector2.zero;
        // Remove after 0.5 seconds
        Destroy(gameObject, 0.5f);
        // Record arrangement ID
        SaveDataManager.SetArrangeId(arrangeId, tag);
    }
}
