using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Renderer rendererComponent;
    private int activeMonsterCount = 0;

    void Start()
    {
        rendererComponent = GetComponent<Renderer>();
        HideBlock();
    }

    // 벽을 화면에 보이게 하는 함수
    public void ShowBlock()
    {
        rendererComponent.enabled = true;
    }

    // 벽을 화면에서 숨기는 함수
    public void HideBlock()
    {
        rendererComponent.enabled = false;
    }

    // 현재 화면에 보이는지 여부를 반환하는 함수
    public bool IsVisible()
    {
        return rendererComponent.enabled;
    }

    // 몬스터가 활성화될 때 호출되는 함수
    public void MonsterActivated()
    {
        activeMonsterCount++;
        if (!IsVisible() && activeMonsterCount > 0)
        {
            ShowBlock();
        }
    }

    // 몬스터가 비활성화될 때 호출되는 함수
    public void MonsterDeactivated()
    {
        activeMonsterCount--;
        if (activeMonsterCount <= 0)
        {
            HideBlock();
        }
    }
}
