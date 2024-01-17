using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BGM ����
public enum BGMType
{
    None,       // ����
    Title,      // Ÿ��Ʋ
    InGame,     // ���� ��
    InBoss,     // ������
}

// SE ����
public enum SEType
{
    GameClear,GameOver,Shootdefault,ShootSniper,
    bossAttack,bossDead,enemyDead,hurt,
    heart,jewel,gun
}

public class SoundManager : MonoBehaviour
{
    public AudioClip bgmInTitle;        // Ÿ��Ʋ BGM
    public AudioClip bgmInGame;         // ���� �� BGM
    public AudioClip bgmInBoss;         // ������ BGM

    public AudioClip meGameClear;       // ���� Ŭ����
    public AudioClip meGameOver;        // ���� ����
    public AudioClip seShootdefault;    // �� ���
    public AudioClip seShootSniper;     // ���� ���

    public AudioClip sebossAttack;
    public AudioClip sebossDead;
    public AudioClip seenemyDead;
    public AudioClip sehurt;

    public AudioClip seHeart;
    public AudioClip seJewel;
    public AudioClip seGun;

    public static SoundManager soundManager;            // ù SoundManager�� ���� ����

    public static BGMType plyingBGM = BGMType.None;     // ��� ���� BGM

    private void Awake()
    {
        if(soundManager == null)
        {
            soundManager = this;        // static ������ �ڱ� �ڽ��� ����
            // ���� �ٲ� ���� ������Ʈ�� �ı����� ����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);        // ���� ������Ʈ �ı�
        }
    }
    // BGM ����
    public void PlayBgm(BGMType type)
    {
        if(type != plyingBGM)
        {
            plyingBGM = type;
            AudioSource audio = GetComponent<AudioSource>();
            if(type == BGMType.Title)
            {
                audio.clip = bgmInTitle;        // Ÿ��Ʋ
            }
            else if(type == BGMType.InGame)
            {
                audio.clip = bgmInGame;         // ���� ��
            }
            else if(type == BGMType.InBoss)
            {
                audio.clip= bgmInBoss;          // ������
            }
            audio.Play();
        }
    }

    // BGM ����
    public void StopBgm()
    {
        GetComponent<AudioSource>().Stop();
        plyingBGM = BGMType.None;
    }

    // SE ���
    public void SEPlay(SEType type)
    {
        if(type == SEType.GameClear)
        {
            GetComponent<AudioSource>().PlayOneShot(meGameClear);       // ���� Ŭ����
        }
        else if(type == SEType.GameOver)
        {
            GetComponent<AudioSource>().PlayOneShot(meGameOver);        // ���� ����
        }
        else if(type == SEType.Shootdefault)
        {
            GetComponent<AudioSource>().PlayOneShot(seShootdefault);    // �� ���
        }
        else if (type == SEType.ShootSniper)
        {
            GetComponent<AudioSource>().PlayOneShot(seShootSniper);     // �� ���
        }
        else if (type == SEType.bossAttack)
        {
            GetComponent<AudioSource>().PlayOneShot(sebossAttack);      // ���� ����
        }
        else if (type == SEType.bossDead)
        {
            GetComponent<AudioSource>().PlayOneShot(sebossDead);        // ���� ����
        }
        else if (type == SEType.enemyDead)
        {
            GetComponent<AudioSource>().PlayOneShot(seenemyDead);       // �� ���
        }
        else if (type == SEType.hurt)
        {
            GetComponent<AudioSource>().PlayOneShot(sehurt);            // ĳ���� �ǰ�
        }
        else if (type == SEType.heart)
        {
            GetComponent<AudioSource>().PlayOneShot(seHeart);           // ��Ʈ ȹ��
        }
        else if (type == SEType.jewel)
        {
            GetComponent<AudioSource>().PlayOneShot(seJewel);           // ��� ȹ��
        }
        else if (type == SEType.gun)
        {
            GetComponent<AudioSource>().PlayOneShot(seGun);             // ���� ȹ��
        }
    }
}
