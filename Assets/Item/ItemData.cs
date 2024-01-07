using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 종류
public enum ItemType
{
    gun,      //총
    key,        //열쇠
    life,	   //생명
}

public class ItemData : MonoBehaviour
{
    public ItemType type;           //아이템의 종류
    public int count = 1;           //아이템 수

    public int arrangeId = 0;       //식별을 위한 값

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //접촉 (물리)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (type == ItemType.key)
            {
                //열쇠
                ItemKeeper.hasKeys += 1;
            }
            else if (type == ItemType.gun)
            {
                GunItemShoot gunScript = collision.gameObject.GetComponent<GunItemShoot>();
                ItemKeeper.hasGunItem += count;
                if (gunScript != null)
                {
                    gunScript.EnableGun();
                }
            }
            /*
            else if (type == ItemType.gun)//총
            {
                gunShoot shoot = collision.gameObject.GetComponent<GunShoot>();
                ItemKeeper.hasArrows += count;
            }
            */
            else if (type == ItemType.life)
            {
                //생명
                if (PlayerController.hp < 3)
                {
                    //HP가 3이하면 추가
                    PlayerController.hp++;
                    //HP 갱신
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                }
            }
            //++++ 아이템 획득 연출 ++++
            //충돌 판정 비활성
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                circleCollider.enabled = false;
            }

            // 아이템의 Rigidbody2D 가져오기
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            if (itemBody != null)
            {
                // 중력 적용
                itemBody.gravityScale = 2.5f;
                // 위로 튀어오르는 연출
                itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
                // 0.5초 후에 제거
                Destroy(gameObject, 0.5f);

                // 배치 Id 기록
                SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
            }
        }
    }
}

