using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//출입구 위치
public enum ExitDirection
{
    right,  //오른쪽
    left,   //왼쪽
    down,   //아래쪽
    up,     //위쪽
}
public class Door : MonoBehaviour
{
    public int arrangeId = 0;       // 식별에 사용되는 값
    public string sceneName = "";   //이동할 씬 이름
    public int doorNumber = 0;      //문 번호
    public ExitDirection direction = ExitDirection.down;//문의 위치


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

                RoomManager.ChangeScene(sceneName, doorNumber);

                //배치 Id 기록
                SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
            }
        }
    }
}
