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
    GameClear, GameOver, Shootdefault, ShootSniper,
    bossAttack, bossDead, enemyDead, hurt,
    heart, jewel, gun
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

    public float bgmVolume = 1.0f;                      // BGM ����
    public float seVolume = 1.0f;                       // SE ����

    private void Awake()
    {
        if (soundManager == null)
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

    private void Start()
    {
        // ����� ���� ���� �ҷ�����
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        // �ʱ� ���� ����
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

    // BGM ����
    public void PlayBgm(BGMType type)
    {
        if (type != plyingBGM)
        {
            plyingBGM = type;
            AudioSource audio = GetComponent<AudioSource>();
            if (type == BGMType.Title)
            {
                audio.clip = bgmInTitle;        // Ÿ��Ʋ
            }
            else if (type == BGMType.InGame)
            {
                audio.clip = bgmInGame;         // ���� ��
            }
            else if (type == BGMType.InBoss)
            {
                audio.clip = bgmInBoss;          // ������
            }
            audio.volume = bgmVolume;
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
        AudioSource audio = GetComponent<AudioSource>();
        if (type == SEType.GameClear)
        {
            audio.PlayOneShot(meGameClear, seVolume);       // ���� Ŭ����
        }
        else if (type == SEType.GameOver)
        {
            audio.PlayOneShot(meGameOver, seVolume);        // ���� ����
        }
        else if (type == SEType.Shootdefault)
        {
            audio.PlayOneShot(seShootdefault, seVolume);    // �� ���
        }
        else if (type == SEType.ShootSniper)
        {
            audio.PlayOneShot(seShootSniper, seVolume);     // ���� ���
        }
        else if (type == SEType.bossAttack)
        {
            audio.PlayOneShot(sebossAttack, seVolume);      // ���� ����
        }
        else if (type == SEType.bossDead)
        {
            audio.PlayOneShot(sebossDead, seVolume);        // ���� ����
        }
        else if (type == SEType.enemyDead)
        {
            audio.PlayOneShot(seenemyDead, seVolume);       // �� ���
        }
        else if (type == SEType.hurt)
        {
            audio.PlayOneShot(sehurt, seVolume);            // ĳ���� �ǰ�
        }
        else if (type == SEType.heart)
        {
            audio.PlayOneShot(seHeart, seVolume);           // ��Ʈ ȹ��
        }
        else if (type == SEType.jewel)
        {
            audio.PlayOneShot(seJewel, seVolume);           // ��� ȹ��
        }
        else if (type == SEType.gun)
        {
            audio.PlayOneShot(seGun, seVolume);             // ���� ȹ��
        }
    }


    private void OnApplicationQuit()
    {
        // ���� ���� ����
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SEVolume", seVolume);
        PlayerPrefs.Save();
    }
}
