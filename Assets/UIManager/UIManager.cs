using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    int hasKeys = 0;                    //열쇠 수
    int hasGunItem = 0;                 // 총 아이템 소지 수
    int hp = 0;                         //플레이어의 HP
    public GameObject gunText;          //화살의 수를 표시하는 Text
    public GameObject keyText;          //열쇠 수를 표시하는Text
    public GameObject hpImage;          //HP의 수를 표시하는 Image
    public Sprite life3Image;           //HP3 이미지
    public Sprite life2Image;           //HP2 이미지
    public Sprite life1Image;           //HP1 이미지
    public Sprite life0Image;           //HP0 이미지
    public GameObject mainImage;        // 이미지를 가지는 GameObject
    public GameObject RetryButton;      // 리셋 버튼
    public GameObject HomeButton;      // 리셋 버튼
    public GameObject BlackImg;         // 바탕 이미지
    public GameObject closeButton;      // 닫기 버튼
    public Sprite gameOverSpr;          // GAME OVER 이미지
    public Sprite gameClearSpr;         // GAME CLEAR 이미지
    public GameObject inputPanel;       //버추얼 패드와 공격 버튼을 배치한 조작 패널

    public string retrySceneName = "";  //재시도하는 씬 이름

    private RewardedAd _rewardedAd;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-6455459678109195/6220583281"; // TestId ca-app-pub-3940256099942544/5224354917
#elif UNITY_IPHONE
    private string _adUnitId = "unused";
#else
    private string _adUnitId = "unused";
#endif

    // Start is called before the first frame update
    void Start()
    {
        UpdateItemCount();  //아이템 수 갱신
        UpdateHP();         //HP갱신
        //이미지를 숨기기
        Invoke("InactiveImage", 1.0f);
        RetryButton.SetActive(false);  //버튼 숨기기
        HomeButton.SetActive(false);

        // Initialize the Mobile Ads SDK.
        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        Debug.Log("준비 안됨.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        Debug.Log("준비 됨.");
                        break;
                }
            }
        });

        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadRewardedAd();
        });
    }
   
    // Update is called once per frame
    void Update()
    {
        UpdateItemCount();  //아이템 수 갱심
        UpdateHP();         //HP갱신
    }

    //아이템 수 갱신
    
    void UpdateItemCount()
    {
        if (hasGunItem  != ItemKeeper.hasGunItem) 
        {
            gunText.GetComponent<Text>().text = ItemKeeper.hasGunItem.ToString();
            hasGunItem = ItemKeeper.hasGunItem;
        }
        //열쇠
        if (hasKeys != ItemKeeper.hasKeys)
        {
            keyText.GetComponent<Text>().text = ItemKeeper.hasKeys.ToString();
            hasKeys = ItemKeeper.hasKeys;
        }
    }


    //HP갱신
    void UpdateHP()
    {
        //Player 가져오기
        if (PlayerController.gameState != "gameend")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (PlayerController.hp != hp)
                {
                    hp = PlayerController.hp;
                    if (hp <= 0)
                    {
                        hpImage.GetComponent<Image>().sprite = life0Image;
                        //플레이어 사망!!
                        RetryButton.SetActive(true);    //버튼 표시
                        HomeButton.SetActive(true);
                        mainImage.SetActive(true);      //이미지 표시
                                                        // 이미지 설정
                        mainImage.GetComponent<Image>().sprite = gameOverSpr;
                        inputPanel.SetActive(false);      //조작 UI 숨기기
                        PlayerController.gameState = "gameend";   //게임 종료
                    }
                    else if (hp == 1)
                    {
                        hpImage.GetComponent<Image>().sprite = life1Image;
                    }
                    else if (hp == 2)
                    {
                        hpImage.GetComponent<Image>().sprite = life2Image;
                    }
                    else if(hp == 3)
                    {
                        hpImage.GetComponent<Image>().sprite = life3Image;
                    }
                }
            }
        }
    }

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("보상된 광고를 로드하는 중.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("보상된 광고가 광고를 로드하지 못했습니다 " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("응답이 가득 담긴 보상 광고 : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                RegisterReloadHandler(_rewardedAd);
            });
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("보상 광고 전체 화면 콘텐츠 닫힘.");

            // Reload the ad so that we can show another as soon as possible.
            //LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("보상된 광고가 전체 화면 콘텐츠를 열지 못했습니다 " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            //LoadRewardedAd();
        };
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                HandleOnAdClosed();
            });
        }
    }

    //재시도
    public void Retry()
    {
        ShowRewardedAd();
    }

    public void HandleOnAdClosed()
    {
        //HP 되돌리기
        PlayerPrefs.SetInt("PlayerHP", 3);
        //BGM 초기화
        SoundManager.plyingBGM = BGMType.None;
        //게임 중으로 설정
        SceneManager.LoadScene(retrySceneName);   //씬 이동
    }

    //이미지 숨기기
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //게임 클리어
    
    public void GameClear()
    {
        //이미지 표시
        mainImage.SetActive(true);
        mainImage.GetComponent<Image>().sprite = gameClearSpr;//「GAME CLEAR」 설정
        //조작 UI 숨기기
        inputPanel.SetActive(false);
        //게임 클리어로 표시
        PlayerController.gameState = "gameclear";
        //3초뒤에 게임 타이틀로 돌아 가기
        Invoke("GoToTitle", 3.0f);
    }
    //타이틀 화면으로 이동
    public void GoToTitle()
    {
        PlayerPrefs.DeleteKey("LastScene");     //저장 씬을 제거
        SceneManager.LoadScene("Title");        //타이틀로 돌아가기
    }
    
    public void SoundSetting()
    {
        BlackImg.SetActive(true);
    }

    public void CloseBlackImg()
    {
        BlackImg.SetActive(false);
    }
}
