using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public float deleteTime = 2;    // ���� �ð�

    void Start()
    {
        Destroy(gameObject, deleteTime);    // ���� �ð� �� �����ϱ�
    }

    // ���� ������Ʈ�� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ���Ϳ� ������ �ٷ� �Ѿ� ����
            Destroy(gameObject);
        }

        // ������ ���� ������Ʈ�� �ڽ����� �����ϱ�
        transform.SetParent(collision.transform);

        // �浹 ������ ��Ȱ��
        GetComponent<CircleCollider2D>().enabled = false;

        // ���� �ùķ��̼��� ��Ȱ��
        GetComponent<Rigidbody2D>().simulated = false;
    }

    void Update()
    {

    }
}
