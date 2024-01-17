using System.Collections;
using System.Collections.Generic;
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
            else
            {
                // �÷��̾���� �Ÿ� Ȯ��
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if(dist < reactionDistance)
                {
                    isActive = true;        // Ȱ������ ����
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ammo")
        {
            if(isActive)
            {
                // ������
                hp--;
                if (hp <= 0)
                {
                    // ���
                    // ===================
                    // ��� ����
                    // ===================
                    // �浹 ���� ��Ȱ��
                    GetComponent<CircleCollider2D>().enabled = false;
                    // SE ���
                    SoundManager.soundManager.SEPlay(SEType.enemyDead);
                    // �̵� ����
                    rbody.velocity = new Vector2(0, 0);
                    // �ִϸ��̼� ����
                    Animator animator = GetComponent<Animator>();
                    animator.Play(deadAnime);
                    // 0.5�� �Ŀ� ����
                    Destroy(gameObject, 0.5f);
                    //��ġ Id ���
                    SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
                }
            }
        }
        else if (collision.gameObject.tag == "gunitem")
        {
            if (isActive)
            {
                // ������
                hp-=3;
                if (hp <= 0)
                {
                    // gunitem �±װ� �پ��ִ� ������Ʈ�� ������ �� ���� ���
                    // ===================
                    // ��� ����
                    // ===================
                    // �浹 ���� ��Ȱ��
                    GetComponent<CircleCollider2D>().enabled = false;
                    // SE ���
                    SoundManager.soundManager.SEPlay(SEType.enemyDead);
                    // �̵� ����
                    rbody.velocity = new Vector2(0, 0);
                    // �ִϸ��̼� ����
                    Animator animator = GetComponent<Animator>();
                    animator.Play(deadAnime);
                    // 0.5�� �Ŀ� ����
                    Destroy(gameObject, 0.5f);
                    // ��ġ Id ���
                    SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
                }
            }
            
        }
    }
}
