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

        SetBlockState(false);   // 화면에서 숨기기
    }

    void Update()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>(); // 모든 Enemy 몬스터 가져오기

        bool anyMovingEnemy = false;  // 주변에 Enemy 몬스터가 하나라도 움직이는지

        foreach (EnemyController enemy in enemies)
        {
            if (enemy.IsMoving())
            {
                anyMovingEnemy = true;
                break; // 하나라도 움직이는 몬스터가 있으면 종료
            }
        }
        SetBlockState(anyMovingEnemy);  // 주변에 움직이는 몬스터 있으면 벽을 활성화, 없으면 비활성화
    }
    void SetBlockState(bool blocked)
    {
        isBlocked = blocked;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer를 가지고 있다면 화면에 표시 여부를 설정
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = blocked;
        }
        if (blockCollider != null)  // Collider2D를 써서 플레이어가 통과하거나 통과할 수 없게
        {
            blockCollider.enabled = blocked;
        }
    }
}
