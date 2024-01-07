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

    // ���� ȭ�鿡 ���̰� �ϴ� �Լ�
    public void ShowBlock()
    {
        rendererComponent.enabled = true;
    }

    // ���� ȭ�鿡�� ����� �Լ�
    public void HideBlock()
    {
        rendererComponent.enabled = false;
    }

    // ���� ȭ�鿡 ���̴��� ���θ� ��ȯ�ϴ� �Լ�
    public bool IsVisible()
    {
        return rendererComponent.enabled;
    }

    // ���Ͱ� Ȱ��ȭ�� �� ȣ��Ǵ� �Լ�
    public void MonsterActivated()
    {
        activeMonsterCount++;
        if (!IsVisible() && activeMonsterCount > 0)
        {
            ShowBlock();
        }
    }

    // ���Ͱ� ��Ȱ��ȭ�� �� ȣ��Ǵ� �Լ�
    public void MonsterDeactivated()
    {
        activeMonsterCount--;
        if (activeMonsterCount <= 0)
        {
            HideBlock();
        }
    }
}
