using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItemShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;    // �Ѿ� �ӵ�
    public float shootDelay = 0.25f;    // �߻� ����
    public GameObject GunitemPrefab;   // ���� ������
    public GameObject ammoPrefab;       // ȭ���� ������

    bool inAttack = false;              // ���� �� ����
    GameObject gunitemObj;              // ���� ���� ������Ʈ
    bool hasGunItem = false;            // �� �������� ������ �ִ��� ����

    void Start()
    {
        // ���� �÷��̾� ĳ���Ϳ� ��ġ
        Vector3 pos = transform.position;
        gunitemObj = Instantiate(GunitemPrefab, pos, Quaternion.identity);
        gunitemObj.transform.SetParent(transform);  // �÷��̾� ĳ���͸� ���� �θ�� ����
        gunitemObj.SetActive(false);  // ó������ ��Ȱ��ȭ

        hasGunItem = ItemKeeper.hasGunItem > 0;
        gunitemObj.SetActive(hasGunItem);
    }

    void Update()
    {
        if (hasGunItem)
        {
            // �� �������� ������ ���� ���� ���� ���
            if (Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.Z))
            {
                Attack();
            }
            if ((Input.GetKeyDown(KeyCode.Z)))
            {
                // ���� Ű�� ����
                Attack();
            }
            if ((Input.GetKeyDown(KeyCode.Space)))
            {
                PlayerController player = GetComponent<PlayerController>();
                player.speed += 2.0f;
            }

            // ���� ȸ���� �켱����
            float gunZ = -5;    // ���� Z ��(ĳ���ͺ��� ������ ����)

            PlayerController plmv = GetComponent<PlayerController>();
            if (plmv.angleZ > 30 && plmv.angleZ < 150)
            {
                // �� ����
                gunZ = 5;       // ���� Z ��(ĳ���ͺ��� �ڷ� ����)
            }

            // ���� ȸ��
            if (plmv.angleZ <= -90 || plmv.angleZ > 90)
            {
                gunitemObj.GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                gunitemObj.GetComponent<SpriteRenderer>().flipY = false;
            }
            gunitemObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);

            // ���� �켱 ����
            gunitemObj.transform.position = new Vector3(transform.position.x, transform.position.y, gunZ);

            if (ItemKeeper.hasGunItem > 0)
            {
                // �Ѿ��� 1�� �̻��̸� Ȱ��ȭ
                EnableGun();
            }
            else
            {
                // �Ѿ��� 0���� ��Ȱ��ȭ
                DisableBullet();
            }
        }

    }

    public void EnableGun()
    {
        // �� �������� ȹ���� �� ȣ��Ǵ� �޼���
        hasGunItem = true;
        gunitemObj.SetActive(true);
    }

    public void DisableBullet()
    {
        // �������� �Ա� ������ �Ѿ� �߻� ��Ȱ��ȭ
        hasGunItem = false;
        gunitemObj.SetActive(false);

    }

    public void Attack()
    {
        // �� �������� ������ �ְ�, ���� ���� �ƴ�
        if (hasGunItem && !inAttack)
        {
            gunitemObj.SetActive(false);
            ItemKeeper.hasGunItem -= 1;   //�Ѿ� �Ҹ�
            inAttack = true;        // ���� ������ ����

            // �Ѿ� �߻�
            PlayerController playerCnt = GetComponent<PlayerController>();
            float angleZ = playerCnt.angleZ;    // ȸ�� ����

            // �Ѿ��� ���� ������Ʈ �����(���� �������� ȸ��)
            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject ammoObj = Instantiate(ammoPrefab, transform.position, r);

            // �Ѿ��� �߻��� ���� ����
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // �Ѿ˿� ���� ���ϱ�
            Rigidbody2D body = ammoObj.GetComponent<Rigidbody2D>();
            body.AddForce(v, ForceMode2D.Impulse);

            // SE ���
            SoundManager.soundManager.SEPlay(SEType.ShootSniper);

            // ���� ���� �ƴ����� ����
            Invoke("StopAttack", shootDelay);
        }
    }

    public void StopAttack()
    {
        inAttack = false;       // ���� ���� �ƴ����� ����
        if (ItemKeeper.hasGunItem <= 0)
        {
            DisableBullet();
        }
    }
}