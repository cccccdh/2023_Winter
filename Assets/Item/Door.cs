using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour
{
    public int arrangeId = 0;       // �ĺ��� ���Ǵ� ��

    // Start is called before the first frame update
    void Start()
    {
        // �ʱ�ȭ �ڵ� (�ʿ�� �߰�)
    }

    // Update is called once per frame
    void Update()
    {
        // ������Ʈ �ڵ� (�ʿ�� �߰�)
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // �÷��̾ ������ 7�� �̻� ������ �ִ��� Ȯ��
            if (ItemKeeper.hasKeys >= 0)
            {
                ItemKeeper.hasKeys -= 7;    // ���� ���� ����
                Destroy(this.gameObject);   // ���� ����

                //��ġ Id ���
                SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
            }
        }
    }
}
