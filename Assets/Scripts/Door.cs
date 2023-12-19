using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangeId = 0;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // 열쇠를 가지고 있으면
            if(ItemKeeper.hasKeys > 0)
            {
                ItemKeeper.hasKeys--;       // 열쇠 하나 감소
                Destroy(this.gameObject);   // 문 열기
            }
        }
    }
}
