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
            // ���踦 ������ ������
            if(ItemKeeper.hasKeys > 0)
            {
                ItemKeeper.hasKeys--;       // ���� �ϳ� ����
                Destroy(this.gameObject);   // �� ����
            }
        }
    }
}
