using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;    // �Ѿ� �ӵ�
    public float shootDelay = 0.25f;    // �߻� ����
    public GameObject gunPrefab;        // ���� ������
    public GameObject bulletPrefab;     // ȭ���� ������

    bool inAttack = false;              // ���� �� ����
    GameObject gunObj;                  // ���� ���� ������Ʈ

    void Start()
    {
        // Ȱ�� �÷��̾� ĳ���Ϳ� ��ġ
        Vector3 pos = transform.position;
        gunObj = Instantiate(gunPrefab, pos, Quaternion.identity);
        gunObj.transform.SetParent(transform);  // �÷��̾� ĳ���͸� ���� �θ�� ����
    }

    void Update()
    {
        if ((Input.GetButtonDown("Fire3")))
        {
            // (���� Shift) ���� Ű�� ����
            Attack();
        }

        if ((Input.GetKeyDown(KeyCode.Z)))
        {
            // (A) ���� Ű�� ����
            Attack();
        }

        // ���� ȸ���� �켱����
        float gunZ = -1;    // ���� Z ��(ĳ���ͺ��� ������ ����)

        PlayerController plmv = GetComponent<PlayerController>();
        if(plmv.angleZ > 30 && plmv.angleZ < 150)
        {
            // �� ����
            gunZ = 1;       // ���� Z ��(ĳ���ͺ��� �ڷ� ����)
        }

        // ���� ȸ��
        if(plmv.angleZ <= -90 || plmv.angleZ > 90)
        {
            gunObj.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            gunObj.GetComponent<SpriteRenderer>().flipY = false;
        }
        gunObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);

        // ���� �켱 ����
        gunObj.transform.position = new Vector3(transform.position.x, transform.position.y, gunZ);
    }

    public void Attack()
    {
        // ���� ���� �ƴ�
        if(inAttack == false)
        {
            inAttack = true;        // ���� ������ ����
            
            // �Ѿ� �߻�
            PlayerController playerCnt = GetComponent<PlayerController>();
            float angleZ = playerCnt.angleZ;    // ȸ�� ����

            // �Ѿ��� ���� ������Ʈ �����(���� �������� ȸ��)
            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject bulletObj = Instantiate(bulletPrefab, transform.position, r);

            // �Ѿ��� �߻��� ���� ����
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // �Ѿ˿� ���� ���ϱ�
            Rigidbody2D body = bulletObj.GetComponent<Rigidbody2D>();
            body.AddForce(v, ForceMode2D.Impulse);

            // ���� ���� �ƴ����� ����
            Invoke("StopAttack", shootDelay);
        }
    }

    public void StopAttack()
    {
        inAttack = false;       // ���� ���� �ƴ����� ����
    }
}
