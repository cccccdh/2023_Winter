using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int arrangeId = 0;       //��ġ ID
    public string objTag = "";      //��ġ�� ������Ʈ�� �±�
}

[System.Serializable]
public class SaveDataList
{
    public SaveData[] saveDatas;    //SaveData �迭
}