using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private bool isBlocked = false;
    private BoxCollider2D blockCollider;
    [SerializeField] private float detectionRadius = 10f; // �˻� ������

    void Start()
    {
        // Block�� Collider2D ������Ʈ ��������
        blockCollider = GetComponent<BoxCollider2D>();

        // �ʱ� ����: ó������ ȭ�鿡�� �����, Collider ��Ȱ��ȭ
        SetBlockState(false);
    }

    void Update()
    {
        // �ֺ��� Enemy �±׸� ���� ���Ͱ� �ִ��� Ȯ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // �ֺ��� Enemy�� ������ ���� ȭ�鿡 ǥ���ϰ� Collider�� Ȱ��ȭ�Ͽ� �÷��̾ ������� ���ϰ� ��
                SetBlockState(true);
                return; // ���� Ȱ��ȭ�Ǹ� �� �̻� Ȯ���� �ʿ䰡 �����Ƿ� ��ȯ
            }
        }

        // �ֺ��� Enemy�� ������ ���� ����� Collider�� ��Ȱ��ȭ�Ͽ� �÷��̾ ����� �� �ְ� ��
        SetBlockState(false);
    }

    // ���� ȭ�� ǥ�� �� Collider Ȱ��ȭ ���θ� �����ϴ� �޼���
    void SetBlockState(bool blocked)
    {
        isBlocked = blocked;

        // SpriteRenderer�� ������ �ִٸ� ȭ�鿡 ǥ�� ���θ� ����
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = blocked;
        }

        // Collider2D�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�Ͽ� �÷��̾ ����� �� ���� ��
        if (blockCollider != null)
        {
            blockCollider.enabled = blocked;
        }
    }
}
