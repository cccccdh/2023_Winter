using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private bool isBlocked = false;
    private BoxCollider2D blockCollider;

    void Start()
    {
        blockCollider = GetComponent<BoxCollider2D>();

        SetBlockState(false);   // ȭ�鿡�� �����
    }

    void Update()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>(); // ��� Enemy ���� ��������

        bool anyMovingEnemy = false;  // �ֺ��� Enemy ���Ͱ� �ϳ��� �����̴���

        foreach (EnemyController enemy in enemies)
        {
            if (enemy.IsMoving())
            {
                anyMovingEnemy = true;
                break; // �ϳ��� �����̴� ���Ͱ� ������ ����
            }
        }
        SetBlockState(anyMovingEnemy);  // �ֺ��� �����̴� ���� ������ ���� Ȱ��ȭ, ������ ��Ȱ��ȭ
    }
    void SetBlockState(bool blocked)
    {
        isBlocked = blocked;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer�� ������ �ִٸ� ȭ�鿡 ǥ�� ���θ� ����
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = blocked;
        }
        if (blockCollider != null)  // Collider2D�� �Ἥ �÷��̾ ����ϰų� ����� �� ����
        {
            blockCollider.enabled = blocked;
        }
    }
}
