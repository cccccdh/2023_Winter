using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ����
public enum ItemType
{
    gun,      //��
    key,        //����
    life,	   //����
}

public class ItemData : MonoBehaviour
{
    public ItemType type;           //�������� ����
    public int count = 1;           //������ ��

    public int arrangeId = 0;       //�ĺ��� ���� ��

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //���� (����)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (type == ItemType.key)
            {
                //����
                ItemKeeper.hasKeys += 1;
            }
            else if (type == ItemType.gun)
            {
                GunItemShoot gunScript = collision.gameObject.GetComponent<GunItemShoot>();
                ItemKeeper.hasGunItem += count;
                if (gunScript != null)
                {
                    gunScript.EnableGun();
                }
            }
            /*
            else if (type == ItemType.gun)//��
            {
                gunShoot shoot = collision.gameObject.GetComponent<GunShoot>();
                ItemKeeper.hasArrows += count;
            }
            */
            else if (type == ItemType.life)
            {
                //����
                if (PlayerController.hp < 3)
                {
                    //HP�� 3���ϸ� �߰�
                    PlayerController.hp++;
                    //HP ����
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                }
            }
            //++++ ������ ȹ�� ���� ++++
            //�浹 ���� ��Ȱ��
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                circleCollider.enabled = false;
            }

            // �������� Rigidbody2D ��������
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            if (itemBody != null)
            {
                // �߷� ����
                itemBody.gravityScale = 2.5f;
                // ���� Ƣ������� ����
                itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
                // 0.5�� �Ŀ� ����
                Destroy(gameObject, 0.5f);

                // ��ġ Id ���
                SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
            }
        }
    }
}

