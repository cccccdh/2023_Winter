using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public GameObject worldblock; // 인스펙터에서 지정한 오브젝트
    private bool activated = false; // 오브젝트의 활성화 상태를 추적

    void Start()
    {
        // 게임 시작 시 저장된 활성화 상태를 불러옴
        bool activated = PlayerPrefs.GetInt(worldblock.name + "_Activated", 0) == 1;
        worldblock.SetActive(activated);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            // 플레이어가 트리거를 밟으면 활성화 상태를 저장하고 worldblock을 활성화함
            PlayerPrefs.SetInt(worldblock.name + "_Activated", 1);
            worldblock.SetActive(true);
            PlayerPrefs.Save();
            activated = true; // 활성화 상태 업데이트
        }
    }
}