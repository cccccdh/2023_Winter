using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ա� ��ġ
public enum ExitDirection
{
    right,  //������
    left,   //����
    down,   //�Ʒ���
    up,     //����
}
public class Door : MonoBehaviour
{
    public int arrangeId = 0;       // �ĺ��� ���Ǵ� ��
    public string sceneName = "";   //�̵��� �� �̸�
    public int doorNumber = 0;      //�� ��ȣ
    public ExitDirection direction = ExitDirection.down;//���� ��ġ


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

                RoomManager.ChangeScene(sceneName, doorNumber);

                //��ġ Id ���
                SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
            }
        }
    }
}
