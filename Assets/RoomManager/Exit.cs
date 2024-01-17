using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//���Ա� ��ġ
public enum ExitDirection
{
    right,  //������
    left,   //����
    down,   //�Ʒ���
    up,     //����
}
public class Exit : MonoBehaviour
{
    public string sceneName = "";   //�̵��� �� �̸�
    public int doorNumber = 0;      //�� ��ȣ
    public ExitDirection direction = ExitDirection.down;//���� ��ġ

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(doorNumber == 10)
        {
            if (collision.gameObject.tag == "Player")
            {
                // �÷��̾ ������ 7�� �̻� ������ �ִ��� Ȯ��
                if (ItemKeeper.hasKeys >= 7)
                {
                    ItemKeeper.hasKeys -= 7;    // ���� ���� ����
                    Destroy(this.gameObject);   // ���� ����


                    string nowScene = PlayerPrefs.GetString("LastScene");
                    SaveDataManager.SaveArrangeData(nowScene); // ��ġ������ ����
                    RoomManager.ChangeScene(sceneName, doorNumber);
                }
            }
        }
        if (doorNumber == 100)
        {
            //BGM ����
            SoundManager.soundManager.StopBgm();
            //SE ��� (���� Ŭ����)
            SoundManager.soundManager.SEPlay(SEType.GameClear);
            //���� Ŭ����
            GameObject.FindObjectOfType<UIManager>().GameClear();
        }else if (doorNumber == 10)
        {
            if (collision.gameObject.tag == "Player")
            {
                // �÷��̾ ������ 7�� �̻� ������ �ִ��� Ȯ��
                if (ItemKeeper.hasKeys >= 0)
                {
                    ItemKeeper.hasKeys -= 7;    // ���� ���� ����
                    Destroy(this.gameObject);   // ���� ����


                    string nowScene = PlayerPrefs.GetString("LastScene");
                    SaveDataManager.SaveArrangeData(nowScene); // ��ġ������ ����
                    RoomManager.ChangeScene(sceneName, doorNumber);
                }
            }
        }
        else
        {
            string nowScene = PlayerPrefs.GetString("LastScene");
            SaveDataManager.SaveArrangeData(nowScene); // ��ġ������ ����
            RoomManager.ChangeScene(sceneName, doorNumber);
        }
    }
}

