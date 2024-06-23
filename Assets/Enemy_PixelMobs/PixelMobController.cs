using UnityEngine;
using UnityEngine.UI;


public class PixelMobController : MonoBehaviour
{
    public Image hp_bar;                // ü�� �� UI

    PixelMobStats stats;
    Animator ani;
    Rigidbody2D rbody;
    SpriteRenderer spriterender;

    float axisH;                // ���� �� ��
    float axisV;                // ���� �� ��    
    bool isActive = false;      // �̵� Ȱ�� ����
    bool isAttacking = false;   // ���� ����

    public int arrangeId = 0;   // ��ġ �ĺ��� ���

    void Start()
    {
        stats = GetComponent<PixelMobStats>();
        Init();
    }   

    void Init()
    {
        ani = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
        UpdateHpBar();
    }

    void Update()
    {
        // Player ���� ������Ʈ ��������
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
        if (isActive && stats.hp > 0)
        {
            rbody.velocity = new Vector2(axisH, axisV);
            ani.SetBool("Walk", isActive);
        }
    }

    private void HandleActiveState(GameObject player)
    {
        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        float rad = Mathf.Atan2(dy, dx);
        float angle = rad * Mathf.Rad2Deg;

        spriterender.flipX = angle > -90.0f && angle <= 90.0f;

        axisH = Mathf.Cos(rad) * stats.speed;
        axisV = Mathf.Sin(rad) * stats.speed;
    }

    // �÷��̾���� �Ÿ� üũ
    private void CheckReactionDistance(GameObject player)
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist < stats.reactionDistance)
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

    // Idle �Լ�
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

    // ������ �޴� �Լ�
    private void HandleCollision(Collision2D collision)
    {
        if (isActive)
        {
            int damage = collision.gameObject.tag == "gunitem" ? 3 : 1;
            stats.hp -= damage;
            UpdateHpBar();

            if (stats.hp <= 0)
            {
                Die(collision.gameObject.tag);
            }
        }
    }

    private void UpdateHpBar()
    {
        if (hp_bar != null)
        {
            hp_bar.fillAmount = (float)stats.hp / stats.maxHp;
        }
    }

    // �׾����� ������ �Լ�
    private void Die(string tag)
    {
        PlayerController.hp++;
        // Disable collision detection
        GetComponent<CircleCollider2D>().enabled = false;
        // Play sound effect
        SoundManager.soundManager.SEPlay(SEType.enemyDead);
        // Stop movement
        rbody.velocity = Vector2.zero;
        // Remove after 0.5 seconds
        gameObject.SetActive(false);
        // Record arrangement ID
        SaveDataManager.SetArrangeId(arrangeId, tag);
    }
}
