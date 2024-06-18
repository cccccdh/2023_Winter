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
    public Image hp_bar;                // ü�� �� UI

    public int hp;                      // ü��
    public int maxHp;                   // �ִ� ü��
    public float speed;                 // �̵� �ӵ�
    public float reactionDistance;      // ���� �Ÿ�

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
        if (isActive && hp > 0)
        {
            // �̵�
            rbody.velocity = new Vector2(axisH, axisV);
            ani.SetBool("Walk", isActive);
        }
    }

    private void HandleActiveState(GameObject player)
    {
        // �÷��̾���� ���� ���ϱ�
        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        float rad = Mathf.Atan2(dy, dx);
        float angle = rad * Mathf.Rad2Deg;
        // �̵� ������ ���� ��������Ʈ ����
        if (angle > -90.0f && angle <= 90.0f)
        {
            spriterender.flipX = true;
        }
        else
        {
            spriterender.flipX = false;
        }
        // �̵��� ���� �����
        axisH = Mathf.Cos(rad) * speed;
        axisV = Mathf.Sin(rad) * speed;
    }

    // �÷��̾���� �Ÿ� üũ
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

    // �׾����� ������ �Լ�
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
