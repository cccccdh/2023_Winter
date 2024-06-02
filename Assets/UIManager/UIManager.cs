using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    int hasKeys = 0;                    //���� ��
    int hasGunItem = 0;                 // �� ������ ���� ��
    int hp = 0;                         //�÷��̾��� HP
    public GameObject gunText;          //ȭ���� ���� ǥ���ϴ� Text
    public GameObject keyText;          //���� ���� ǥ���ϴ�Text
    public GameObject hpImage;          //HP�� ���� ǥ���ϴ� Image
    public Sprite life3Image;           //HP3 �̹���
    public Sprite life2Image;           //HP2 �̹���
    public Sprite life1Image;           //HP1 �̹���
    public Sprite life0Image;           //HP0 �̹���
    public GameObject mainImage;        // �̹����� ������ GameObject
    public GameObject RetryButton;      // ���� ��ư
    public GameObject HomeButton;      // ���� ��ư
    public GameObject BlackImg;         // ���� �̹���
    public GameObject closeButton;      // �ݱ� ��ư
    public Sprite gameOverSpr;          // GAME OVER �̹���
    public Sprite gameClearSpr;         // GAME CLEAR �̹���
    public GameObject inputPanel;       //���߾� �е�� ���� ��ư�� ��ġ�� ���� �г�

    public string retrySceneName = "";  //��õ��ϴ� �� �̸�

    private RewardedAd _rewardedAd;


    #if UNITY_ANDROID
        string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    #endif

    // Start is called before the first frame update
    void Start()
    {
        UpdateItemCount();  //������ �� ����
        UpdateHP();         //HP����
        //�̹����� �����
        Invoke("InactiveImage", 1.0f);
        RetryButton.SetActive(false);  //��ư �����
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
                        Debug.Log("�غ� �ȵ�.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        Debug.Log("�غ� ��.");
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
        UpdateItemCount();  //������ �� ����
        UpdateHP();         //HP����
    }

    //������ �� ����
    
    void UpdateItemCount()
    {
        if (hasGunItem  != ItemKeeper.hasGunItem) 
        {
            gunText.GetComponent<Text>().text = ItemKeeper.hasGunItem.ToString();
            hasGunItem = ItemKeeper.hasGunItem;
        }
        //����
        if (hasKeys != ItemKeeper.hasKeys)
        {
            keyText.GetComponent<Text>().text = ItemKeeper.hasKeys.ToString();
            hasKeys = ItemKeeper.hasKeys;
        }
    }


    //HP����
    void UpdateHP()
    {
        //Player ��������
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
                        //�÷��̾� ���!!
                        RetryButton.SetActive(true);    //��ư ǥ��
                        HomeButton.SetActive(true);
                        mainImage.SetActive(true);      //�̹��� ǥ��
                                                        // �̹��� ����
                        mainImage.GetComponent<Image>().sprite = gameOverSpr;
                        inputPanel.SetActive(false);      //���� UI �����
                        PlayerController.gameState = "gameend";   //���� ����
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

        Debug.Log("����� ���� �ε��ϴ� ��.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("����� ���� ���� �ε����� ���߽��ϴ� " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("������ ���� ��� ���� ���� : "
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
            Debug.Log("���� ���� ��ü ȭ�� ������ ����.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("����� ���� ��ü ȭ�� �������� ���� ���߽��ϴ� " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
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

    //��õ�
    public void Retry()
    {
        ShowRewardedAd();        
    }
    
    public void HandleOnAdClosed()
    {
        //HP �ǵ�����
        PlayerPrefs.SetInt("PlayerHP", 3);
        //BGM �ʱ�ȭ
        SoundManager.plyingBGM = BGMType.None;
        //���� ������ ����
        SceneManager.LoadScene(retrySceneName);   //�� �̵�
    }

    //�̹��� �����
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //���� Ŭ����
    
    public void GameClear()
    {
        //�̹��� ǥ��
        mainImage.SetActive(true);
        mainImage.GetComponent<Image>().sprite = gameClearSpr;//��GAME CLEAR�� ����
        //���� UI �����
        inputPanel.SetActive(false);
        //���� Ŭ����� ǥ��
        PlayerController.gameState = "gameclear";
        //3�ʵڿ� ���� Ÿ��Ʋ�� ���� ����
        Invoke("GoToTitle", 3.0f);
    }
    //Ÿ��Ʋ ȭ������ �̵�
    public void GoToTitle()
    {
        PlayerPrefs.DeleteKey("LastScene");     //���� ���� ����
        SceneManager.LoadScene("Title");        //Ÿ��Ʋ�� ���ư���
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
