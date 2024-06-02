using UnityEngine;

public class AmmoController : MonoBehaviour
{
    // 게임 오브젝트에 접총
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || (collision.gameObject.CompareTag("Wall")))
        {
            ObjectPool.ReturnObjectAmmo(this);
        }
    }
}
