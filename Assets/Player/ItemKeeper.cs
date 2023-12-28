using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKeeper : MonoBehaviour
{
    public static int hasKeys = 0;      // ���� ��
    
    void Start()
    {
        hasKeys = PlayerPrefs.GetInt("Keys");
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public static void SaveItem()
    {
        PlayerPrefs.SetInt("Keys", hasKeys);
    }
}
