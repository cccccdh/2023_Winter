using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BGM 종류
public enum BGMType
{
    None,       // 없음
    Title,      // 타이틀
    InGame,     // 게임 중
    InBoss,     // 보스전
}

// SE 종류
public enum SEType
{
    GameClear, GameOver, Shootdefault, ShootSniper,
    bossAttack, bossDead, enemyDead, hurt,
    heart, jewel, gun
}

public class SoundManager : MonoBehaviour
{
    public AudioClip bgmInTitle;        // 타이틀 BGM
    public AudioClip bgmInGame;         // 게임 중 BGM
    public AudioClip bgmInBoss;         // 보스전 BGM

    public AudioClip meGameClear;       // 게임 클리어
    public AudioClip meGameOver;        // 게임 오버
    public AudioClip seShootdefault;    // 총 쏘기
    public AudioClip seShootSniper;     // 소총 쏘기

    public AudioClip sebossAttack;
    public AudioClip sebossDead;
    public AudioClip seenemyDead;
    public AudioClip sehurt;

    public AudioClip seHeart;
    public AudioClip seJewel;
    public AudioClip seGun;

    public static SoundManager soundManager;            // 첫 SoundManager를 갖는 변수

    public static BGMType plyingBGM = BGMType.None;     // 재생 중인 BGM

    public float bgmVolume = 1.0f;                      // BGM 볼륨
    public float seVolume = 1.0f;                       // SE 볼륨

    private void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;        // static 변수에 자기 자신을 저장
            // 씬이 바뀌어도 게임 오브젝트를 파기하지 않음
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);        // 게임 오브젝트 파기
        }
    }

    private void Start()
    {
        // 저장된 볼륨 설정 불러오기
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        // 초기 볼륨 설정
        GetComponent<AudioSource>().volume = bgmVolume;
    }
    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        GetComponent<AudioSource>().volume = bgmVolume;
    }

    public void SetSeVolume(float volume)
    {
        seVolume = volume;
    }

    // BGM 설정
    public void PlayBgm(BGMType type)
    {
        if (type != plyingBGM)
        {
            plyingBGM = type;
            AudioSource audio = GetComponent<AudioSource>();
            if (type == BGMType.Title)
            {
                audio.clip = bgmInTitle;        // 타이틀
            }
            else if (type == BGMType.InGame)
            {
                audio.clip = bgmInGame;         // 게임 중
            }
            else if (type == BGMType.InBoss)
            {
                audio.clip = bgmInBoss;          // 보스전
            }
            audio.volume = bgmVolume;
            audio.Play();
        }
    }

    // BGM 정지
    public void StopBgm()
    {
        GetComponent<AudioSource>().Stop();
        plyingBGM = BGMType.None;
    }

    // SE 재생
    public void SEPlay(SEType type)
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (type == SEType.GameClear)
        {
            audio.PlayOneShot(meGameClear, seVolume);       // 게임 클리어
        }
        else if (type == SEType.GameOver)
        {
            audio.PlayOneShot(meGameOver, seVolume);        // 게임 오버
        }
        else if (type == SEType.Shootdefault)
        {
            audio.PlayOneShot(seShootdefault, seVolume);    // 총 쏘기
        }
        else if (type == SEType.ShootSniper)
        {
            audio.PlayOneShot(seShootSniper, seVolume);     // 소총 쏘기
        }
        else if (type == SEType.bossAttack)
        {
            audio.PlayOneShot(sebossAttack, seVolume);      // 보스 공격
        }
        else if (type == SEType.bossDead)
        {
            audio.PlayOneShot(sebossDead, seVolume);        // 보스 죽음
        }
        else if (type == SEType.enemyDead)
        {
            audio.PlayOneShot(seenemyDead, seVolume);       // 적 사망
        }
        else if (type == SEType.hurt)
        {
            audio.PlayOneShot(sehurt, seVolume);            // 캐릭터 피격
        }
        else if (type == SEType.heart)
        {
            audio.PlayOneShot(seHeart, seVolume);           // 하트 획득
        }
        else if (type == SEType.jewel)
        {
            audio.PlayOneShot(seJewel, seVolume);           // 쥬얼 획득
        }
        else if (type == SEType.gun)
        {
            audio.PlayOneShot(seGun, seVolume);             // 소총 획득
        }
    }


    private void OnApplicationQuit()
    {
        // 볼륨 설정 저장
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SEVolume", seVolume);
        PlayerPrefs.Save();
    }
}
