using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private bool isBlocked = false;
    private BoxCollider2D blockCollider;
    [SerializeField] private float detectionRadius = 10f; // 검색 반지름

    void Start()
    {
        // Block의 Collider2D 컴포넌트 가져오기
        blockCollider = GetComponent<BoxCollider2D>();

        // 초기 설정: 처음에는 화면에서 숨기고, Collider 비활성화
        SetBlockState(false);
    }

    void Update()
    {
        // 주변에 Enemy 태그를 가진 몬스터가 있는지 확인
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // 주변에 Enemy가 있으면 벽을 화면에 표시하고 Collider를 활성화하여 플레이어가 통과하지 못하게 함
                SetBlockState(true);
                return; // 벽이 활성화되면 더 이상 확인할 필요가 없으므로 반환
            }
        }

        // 주변에 Enemy가 없으면 벽을 숨기고 Collider를 비활성화하여 플레이어가 통과할 수 있게 함
        SetBlockState(false);
    }

    // 벽의 화면 표시 및 Collider 활성화 여부를 설정하는 메서드
    void SetBlockState(bool blocked)
    {
        isBlocked = blocked;

        // SpriteRenderer를 가지고 있다면 화면에 표시 여부를 설정
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = blocked;
        }

        // Collider2D를 활성화 또는 비활성화하여 플레이어가 통과할 수 없게 함
        if (blockCollider != null)
        {
            blockCollider.enabled = blocked;
        }
    }
}
