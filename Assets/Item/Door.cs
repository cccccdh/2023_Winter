using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour
{
    public int arrangeId = 0;       // 식별에 사용되는 값

    // Start is called before the first frame update
    void Start()
    {
        // 초기화 코드 (필요시 추가)
    }

    // Update is called once per frame
    void Update()
    {
        // 업데이트 코드 (필요시 추가)
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 플레이어가 보석을 7개 이상 가지고 있는지 확인
            if (ItemKeeper.hasKeys >= 0)
            {
                ItemKeeper.hasKeys -= 7;    // 보석 개수 감소
                Destroy(this.gameObject);   // 문을 열기

                //배치 Id 기록
                SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
            }
        }
    }
}
