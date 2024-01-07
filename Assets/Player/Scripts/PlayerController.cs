using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �̵� �ӵ�
    public float speed = 3.0f;

    // �ִϸ��̼� �̸�
    public string upAnime = "PlayerUp";         // ��
    public string downAnime = "PlayerDown";     // �Ʒ�
    public string rightAnime = "PlayerRight";   // ������
    public string leftAnime = "PlayerLeft";     // ����
    // ���� �ȸ���
    // public string deadAnime = "PlayerDead";  // ���

    // ���� �ִϸ��̼�
    string nowAnimation = "";

    // ���� �ִϸ��̼�
    string oldAnimation = "";

    float axisH;                        // ������ ��(-1.0 ~ 1.0)
    float axisV;                        // ������ ��(-1.0 ~ 1.0)
    public float angleZ = -90.0f;       // ȸ����

    Rigidbody2D rbody;                  // Rigidbody2D
    bool isMoving = false;              // �̵� ������ ����

    //����� ó��
    public static int hp = 3; //�÷��̾� HP
    public static string gameState; // ���� ����
    bool inDamage = false; // ����� �޴� ������ ����

    void Start()
    {
        //RIgidbody2d ��������
        rbody = GetComponent<Rigidbody2D>();
        //�ִϸ��̼�
        oldAnimation = downAnime;
        //���� ���¸� �÷��� ������ ����
        gameState = "playing";
    }

    void Update()
    {
        //���� ���� �ƴϰų� ������� �޴� �߿��� �ƹ��͵� ���� ����
        if (gameState != "playing" || inDamage)
        {
            return;
        }

        if (isMoving == false)
        {
            axisH = Input.GetAxis("Horizontal");    // �¿� Ű �Է�
            axisV = Input.GetAxis("Vertical");      // ���� Ű �Է�
        }

        // Ű �Է����� �̵� ���� ���ϱ�
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);

        // �̵� �������� ����� �ִϸ��̼� ����
        if (angleZ >= -45 && angleZ < 45)
        {
            // ������
            nowAnimation = rightAnime;
        }else if(angleZ >= 45 && angleZ <= 135)
        {
            // ����
            nowAnimation = upAnime;
        }
        else if (angleZ >= -135 && angleZ <= -45)
        {
            // �Ʒ���
            nowAnimation = downAnime;
        }
        else
        {
            // ����
            nowAnimation = leftAnime;
        }
        
        // �ִϸ��̼� �����ϱ�
        if(nowAnimation != oldAnimation)
        {
            oldAnimation = nowAnimation;
            GetComponent<Animator>().Play(nowAnimation);
        }
    }

    void FixedUpdate()
    {
        //���� ���� �ƴϸ� �ƹ��͵� ���� ����
        if(gameState != "playing")
        {
            return;
        }
        if (inDamage)
        {
            //������� �޴� �߿��� �����Ű��
            float val = Mathf.Sin(Time.time * 50);
            Debug.Log(val);
            if (val > 0)
            {
                //��������Ʈ ǥ��
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //��������Ʈ ��ǥ��
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return; // ������� �޴� �߿��� ������ �� ���� �ϱ�
        }
        // �̵� �ӵ� �����ϱ�
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
    //p1���� p2������ ���� ���
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if(axisH != 0 || axisV != 0)
        {
            // �̵� ���̸� ������ ����
            // p1 �� p2 ���� ���ϱ� (������ 0���� �ϱ� ����)
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;

            // ��ũ ź��Ʈ �Լ��� ����(����) ���ϱ�
            float rad = Mathf.Atan2(dy, dx);

            // ���� ������ ��ȯ�Ͽ� ��ȯ
            angle = rad * Mathf.Rad2Deg;
        }else
        {
            // ���� ���̸� ���� ������ ����
            angle = angleZ;
        }

        return angle;
    }

    //����(����)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
    }

    // �����
    void GetDamage(GameObject enemy)
    {
        if(gameState == "Playing")
        {
            hp--; // HP ����
            if (hp > 0)
            {
                // �̵� ����
                rbody.velocity = new Vector2(0, 0);
                // �� ĳ���� �ݴ� �������� ��Ʈ��
                Vector3 toPos = (transform.position - enemy.transform.position).normalized;
                rbody.AddForce(new Vector2(toPos.x *4, toPos.y *4), ForceMode2D.Impulse);
                // ������� �޴� ������ ����
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                // ���� ����
                GameOver();
            }
        }
    }

    // ����� �ޱ� ��
    void DamageEnd()
    {
        // ������� �޴� ���� �ƴ����� ����
        inDamage = false;
        //��������Ʈ �ǵ�����
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // ���� ����
    void GameOver()
    {
        Debug.Log("���� ����");
        gameState = "gameover";
        //=====================
        // ���� ���� ����
        //=====================
        // �÷��̾� �浹 ���� ��Ȱ��ȭ
        GetComponent<CircleCollider2D>().enabled = false;
        // �̵� ����
        rbody.velocity = new Vector2(0, 0);
        // �߷��� ������ �÷��̾ ���� Ƣ�� ������ �ϴ� ����
        rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        // �ִϸ��̼� �����ϱ�
        //GetComponent<Animator>().Play(deadAnime);
        //1�� �Ŀ� �÷��̾� ĳ���� �����ϱ�
        Destroy(gameObject, 1.0f);
    }
}
