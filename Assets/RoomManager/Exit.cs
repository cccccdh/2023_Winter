using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//출입구 위치
public enum ExitDirection
{
    right,  //오른쪽
    left,   //왼쪽
    down,   //아래쪽
    up,     //위쪽
}
public class Exit : MonoBehaviour
{
    public string sceneName = "";   //이동할 씬 이름
    public int doorNumber = 0;      //문 번호
    public ExitDirection direction = ExitDirection.down;//문의 위치

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(doorNumber == 10)
        {
            if (collision.gameObject.tag == "Player")
            {
                // 플레이어가 보석을 7개 이상 가지고 있는지 확인
                if (ItemKeeper.hasKeys >= 7)
                {
                    ItemKeeper.hasKeys -= 7;    // 보석 개수 감소
                    Destroy(this.gameObject);   // 문을 열기


                    string nowScene = PlayerPrefs.GetString("LastScene");
                    SaveDataManager.SaveArrangeData(nowScene); // 배치데이터 저장
                    RoomManager.ChangeScene(sceneName, doorNumber);
                }
            }
        }
        if (doorNumber == 100)
        {
            //BGM 정지
            SoundManager.soundManager.StopBgm();
            //SE 재생 (게임 클리어)
            SoundManager.soundManager.SEPlay(SEType.GameClear);
            //게임 클리어
            GameObject.FindObjectOfType<UIManager>().GameClear();
        }else if (doorNumber == 10)
        {
            if (collision.gameObject.tag == "Player")
            {
                // 플레이어가 보석을 7개 이상 가지고 있는지 확인
                if (ItemKeeper.hasKeys >= 0)
                {
                    ItemKeeper.hasKeys -= 7;    // 보석 개수 감소
                    Destroy(this.gameObject);   // 문을 열기


                    string nowScene = PlayerPrefs.GetString("LastScene");
                    SaveDataManager.SaveArrangeData(nowScene); // 배치데이터 저장
                    RoomManager.ChangeScene(sceneName, doorNumber);
                }
            }
        }
        else
        {
            string nowScene = PlayerPrefs.GetString("LastScene");
            SaveDataManager.SaveArrangeData(nowScene); // 배치데이터 저장
            RoomManager.ChangeScene(sceneName, doorNumber);
        }
    }
}

