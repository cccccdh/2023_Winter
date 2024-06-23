using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    // ü��
    public int hp = 50;
    int maxHp = 50;

    // ���� �Ÿ�
    public float reactionDistance = 10.0f;
    public Image hp_bar;

    public GameObject bulletPrefab;         // �Ѿ�
    public GameObject[] bossBullets;
    public GameObject[] miniBosses;
    public float shootSpeed = 5.0f;         // �Ѿ� �ӵ�


    // ���� ������ ����
    bool isAttack = false;

    // �̴Ϻ��� ���� ����
    bool[] miniBossSpawned = new bool[7];
    public void Awake()
    {
        UpdateHpBar();
    }

    void Update()
    {
        if (hp > 0)
        {
            // Player ���� ������Ʈ ��������
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // �÷��̾���� �Ÿ� Ȯ��
                Vector3 plpos = player.transform.position;
                float dist = Vector2.Distance(transform.position, plpos);
                if (dist <= reactionDistance && isAttack == false)
                {
                    // ���� �� & ���� ���� �ƴϸ� ���� �ִϸ��̼�
                    isAttack = true;
                    // �ִϸ��̼� ����
                    GetComponent<Animator>().Play("BossAttack");
                }
                else if (dist > reactionDistance && isAttack)
                {
                    isAttack = false;
                    // �ִϸ��̼� ����
                    GetComponent<Animator>().Play("BossIdle");
                }
            }
            else
            {
                isAttack = false;
                // �ִϸ��̼� ����
                GetComponent<Animator>().Play("BossIdle");
            }

            // MiniBoss ���� üũ
            SpawnMiniBoss(hp);
        }
    }

    private void SpawnMiniBoss(int bosshp)
    {
        int[] thresholds = { 49, 42, 35, 28, 21, 14, 7 };
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (bosshp <= thresholds[i] && !miniBossSpawned[i])
            {
                miniBosses[i].SetActive(true);  // �̴� ������ Ȱ��ȭ
                miniBosses[i].GetComponent<PixelMobStats>().maxHp = 10;
                miniBosses[i].GetComponent<PixelMobStats>().hp = 10;
                miniBosses[i].GetComponent<PixelMobStats>().reactionDistance = 10;
                miniBossSpawned[i] = true;    // ��ȯ ���¸� true�� ����
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hp <= 0)
        {
            // ���
            // �浹 ���� ��Ȱ��
            GetComponent<CircleCollider2D>().enabled = false;
            // SE ���
            SoundManager.soundManager.SEPlay(SEType.bossDead);
            // �ִϸ��̼� ����
            GetComponent<Animator>().Play("BossDead");
            // 1�� �ڿ� ����
            Invoke("DisableBoss", 1.0f);
        }
        else
        {
            if (collision.gameObject.tag == "Ammo")
            {
                hp--;
                UpdateHpBar();
            }
            if (collision.gameObject.tag == "gunitem")
            {
                hp -= 3;
                UpdateHpBar();
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

    void DisableBoss()
    {
        this.gameObject.SetActive(false);
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

    // ���� 1
    void Attack_1()
    {
        // �߻� ��ġ ������Ʈ ��������
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;

        // �Ѿ��� �߻��� ���� �����
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;

            // ��ũ ź��Ʈ2 �Լ��� ����(����) ���ϱ�
            float rad = Mathf.Atan2(dy, dx);

            // ������ ������ ��ȯ
            float angle = rad * Mathf.Rad2Deg;

            // Prefab���� �Ѿ� ������Ʈ ����� (���� �������� ȸ��)
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // �߻�
            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);

            // SE ���
            SoundManager.soundManager.SEPlay(SEType.bossAttack);
        }
    }

    // ���� 2
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
