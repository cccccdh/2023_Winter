using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKeeper : MonoBehaviour
{
    public static int hasKeys = 0;      // ���� ��
    public static int hasGunItem = 0;
    
    void Start()
    {
        hasKeys = PlayerPrefs.GetInt("Keys");
        hasGunItem = PlayerPrefs.GetInt("gun");
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public static void SaveItem()
    {
        PlayerPrefs.SetInt("Keys", hasKeys);
        PlayerPrefs.SetInt("gun", hasGunItem);
    }
}
