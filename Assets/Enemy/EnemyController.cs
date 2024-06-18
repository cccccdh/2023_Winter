using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    // HP
    public int hp = 3;
    // 최대 체력
    public int maxHp = 3;
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
    // hp bar
    public Image hp_bar;
    
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
        // 체력 초기화
        UpdateHpBar();
    }

    void Update()
    {
        // Player 게임 오브젝트 가져오기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (isActive)
            {
                HandleActiveState(player);
            }
            else
            {
                CheckReactionDistance(player);
            }
        } 
        else if(isActive)
        {
            Deactivate();
        }
    }

    private void HandleActiveState(GameObject player)
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

    private void CheckReactionDistance(GameObject player)
    {
        // Check distance to player
        float dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist < reactionDistance)
        {
            isActive = true; // Set active state
        }
        else
        {
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
    private void Deactivate()
    {
        isActive = false;
        rbody.velocity = Vector2.zero;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ammo" || collision.gameObject.tag == "gunitem")
        {
            HandleCollision(collision);
        }
    }

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

    private void Die(string tag)
    {
        // Disable collision detection
        GetComponent<CircleCollider2D>().enabled = false;
        // Play sound effect
        SoundManager.soundManager.SEPlay(SEType.enemyDead);
        // Stop movement
        rbody.velocity = Vector2.zero;
        // Change animation
        Animator animator = GetComponent<Animator>();
        animator.Play(deadAnime);
        // Remove after 0.5 seconds
        Destroy(gameObject, 0.5f);
        // Record arrangement ID
        SaveDataManager.SetArrangeId(arrangeId, tag);
    }
}
