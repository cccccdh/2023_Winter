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
    GameClear,GameOver,Shootdefault,ShootSniper,
    bossAttack,bossDead,enemyDead,hurt,
    heart,jewel,gun
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

    private void Awake()
    {
        if(soundManager == null)
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
    // BGM 설정
    public void PlayBgm(BGMType type)
    {
        if(type != plyingBGM)
        {
            plyingBGM = type;
            AudioSource audio = GetComponent<AudioSource>();
            if(type == BGMType.Title)
            {
                audio.clip = bgmInTitle;        // 타이틀
            }
            else if(type == BGMType.InGame)
            {
                audio.clip = bgmInGame;         // 게임 중
            }
            else if(type == BGMType.InBoss)
            {
                audio.clip= bgmInBoss;          // 보스전
            }
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
        if(type == SEType.GameClear)
        {
            GetComponent<AudioSource>().PlayOneShot(meGameClear);       // 게임 클리어
        }
        else if(type == SEType.GameOver)
        {
            GetComponent<AudioSource>().PlayOneShot(meGameOver);        // 게임 오버
        }
        else if(type == SEType.Shootdefault)
        {
            GetComponent<AudioSource>().PlayOneShot(seShootdefault);    // 총 쏘기
        }
        else if (type == SEType.ShootSniper)
        {
            GetComponent<AudioSource>().PlayOneShot(seShootSniper);     // 총 쏘기
        }
        else if (type == SEType.bossAttack)
        {
            GetComponent<AudioSource>().PlayOneShot(sebossAttack);      // 보스 공격
        }
        else if (type == SEType.bossDead)
        {
            GetComponent<AudioSource>().PlayOneShot(sebossDead);        // 보스 죽음
        }
        else if (type == SEType.enemyDead)
        {
            GetComponent<AudioSource>().PlayOneShot(seenemyDead);       // 적 사망
        }
        else if (type == SEType.hurt)
        {
            GetComponent<AudioSource>().PlayOneShot(sehurt);            // 캐릭터 피격
        }
        else if (type == SEType.heart)
        {
            GetComponent<AudioSource>().PlayOneShot(seHeart);           // 하트 획득
        }
        else if (type == SEType.jewel)
        {
            GetComponent<AudioSource>().PlayOneShot(seJewel);           // 쥬얼 획득
        }
        else if (type == SEType.gun)
        {
            GetComponent<AudioSource>().PlayOneShot(seGun);             // 소총 획득
        }
    }
}
