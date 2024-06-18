using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // HP
    public int hp = 3;
    // �̵��ӵ�
    public float speed = 0.5f;
    // ���� �Ÿ�
    public float reactionDistance = 4.0f;
    // �ִϸ��̼� �̸�
    public string idleAnime = "EnemyIdle";      // ����
    public string upAnime = "EnemyUp";          // ��
    public string downAnime = "EnemyDown";      // �Ʒ�
    public string rightAnime = "EnemyRight";    // ������
    public string leftAnime = "EnemyLeft";      // ����
    public string deadAnime = "EnemyDead";      // ���
    // ���� �ִϸ��̼�
    string nowAnimation = "";
    // ���� �ִϸ��̼�
    string oldAnimation = "";

    float axisH;                // ���� �� ��
    float axisV;                // ���� �� ��
    Rigidbody2D rbody;

    bool isActive = false;      // Ȱ�� ����

    public int arrangeId = 0;   // ��ġ �ĺ��� ���

    void Start()
    {
        // Rigidbody2D ��������
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Player ���� ������Ʈ ��������
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
        // �÷��̾���� ���� ���ϱ�
        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        float rad = Mathf.Atan2(dy, dx);
        float angle = rad * Mathf.Rad2Deg;
        // �̵� ������ ���� �ִϸ��̼� ����
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
        // �̵��� ���� �����
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
            // �̵�
            rbody.velocity = new Vector2(axisH, axisV);
            if(nowAnimation != oldAnimation)
            {
                // �ִϸ��̼� �����ϱ�
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

            if (hp <= 0)
            {
                Die(collision.gameObject.tag);
            }
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
