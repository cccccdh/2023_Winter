using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataList arrangeDataList;    //��ġ ������

    // Start is called before the first frame update
    void Start()
    {
        //SaveDataList �ʱ�ȭ 
        arrangeDataList = new SaveDataList();
        arrangeDataList.saveDatas = new SaveData[] { };
        //�� �̸� �ҷ�����
        string stageName = PlayerPrefs.GetString("LastScene");
        //�� �̸��� Ű���Ͽ� ���� ������ �о����
        string data = PlayerPrefs.GetString(stageName);
        if (data != "")
        {
            //--- ���� �� �����Ͱ� ������ ���  ---
            //JSON���� SaveDataList�� ��ȯ�ϱ�
            arrangeDataList = JsonUtility.FromJson<SaveDataList>(data);
            for (int i = 0; i < arrangeDataList.saveDatas.Length; i++)
            {
                SaveData savedata = arrangeDataList.saveDatas[i]; //�迭���� ��������
                //�±׷� ���� ������Ʈ ã��
                string objTag = savedata.objTag;
                GameObject[] objects = GameObject.FindGameObjectsWithTag(objTag);
                for (int ii = 0; ii < objects.Length; ii++)
                {
                    GameObject obj = objects[ii]; //�迭���� GameObject ��������
                    //GameObject�� �±� Ȯ���ϱ�
                    if (objTag == "Door")       //��
                    {
                        Door door = obj.GetComponent<Door>();
                        if (door.arrangeId == savedata.arrangeId)
                        {
                            Destroy(obj);  //arrangeId�� ������ ����
                        }
                    }
                    else if (objTag == "ItemBox")   //���� ����
                    {
                        ItemBox box = obj.GetComponent<ItemBox>();
                        if (box.arrangeId == savedata.arrangeId)
                        {
                            box.isClosed = false;   //arrangeIdd�� ������ ����
                            box.GetComponent<SpriteRenderer>().sprite = box.openImage;
                        }
                    }
                    else if (objTag == "Item")      //������
                    {
                        ItemData item = obj.GetComponent<ItemData>();
                        if (item.arrangeId == savedata.arrangeId)
                        {
                            Destroy(obj);   //arrangeId�� ������ ����
                        }
                    }
                    else if (objTag == "Enemy")      //��
                    {
                        EnemyController enemy = obj.GetComponent<EnemyController>();
                        if (enemy.arrangeId == savedata.arrangeId)
                        {
                            Destroy(obj);   //arrangeId�� ������ ����
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //��ġ Id ����
    public static void SetArrangeId(int arrangeId, string objTag)
    {
        if (arrangeId == 0 || objTag == "")
        {
            //������� ����
            return;
        }
        //�߰� �ϱ� ���� �ϳ� ���� SaveData �迭 �����
        SaveData[] newSavedatas = new SaveData[arrangeDataList.saveDatas.Length + 1];
        //������ ����
        for (int i = 0; i < arrangeDataList.saveDatas.Length; i++)
        {
            newSavedatas[i] = arrangeDataList.saveDatas[i];
        }
        //SaveData �����
        SaveData savedata = new SaveData();
        savedata.arrangeId = arrangeId; //Id�� ���
        savedata.objTag = objTag;       //�±� ���
        //SaveData �߰�
        newSavedatas[arrangeDataList.saveDatas.Length] = savedata;
        arrangeDataList.saveDatas = newSavedatas;
    }

    //��ϵ� ������ ����
    public static void SaveArrangeData(string stageName)
    {
        if (arrangeDataList.saveDatas != null && stageName != "")
        {
            //SaveDataList�� JSON �����ͷ� ��ȯ
            string saveJson = JsonUtility.ToJson(arrangeDataList);
            //�� �̸��� Ű���Ͽ� ����
            PlayerPrefs.SetString(stageName, saveJson);
        }
    }
}
