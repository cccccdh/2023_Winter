using UnityEngine;

public class AmmoController : MonoBehaviour
{
    // ���� ������Ʈ�� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || (collision.gameObject.CompareTag("Wall")))
        {
            ObjectPool.ReturnObjectAmmo(this);
        }
    }
}
