using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public GameObject worldblock; // �ν����Ϳ��� ������ ������Ʈ
    private bool activated = false; // ������Ʈ�� Ȱ��ȭ ���¸� ����

    void Start()
    {
        // ���� ���� �� ����� Ȱ��ȭ ���¸� �ҷ���
        bool activated = PlayerPrefs.GetInt(worldblock.name + "_Activated", 0) == 1;
        worldblock.SetActive(activated);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            // �÷��̾ Ʈ���Ÿ� ������ Ȱ��ȭ ���¸� �����ϰ� worldblock�� Ȱ��ȭ��
            PlayerPrefs.SetInt(worldblock.name + "_Activated", 1);
            worldblock.SetActive(true);
            PlayerPrefs.Save();
            activated = true; // Ȱ��ȭ ���� ������Ʈ
        }
    }
}