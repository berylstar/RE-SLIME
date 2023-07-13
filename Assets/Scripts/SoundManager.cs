using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager I = null;

    [Header("BGM")]
    public AudioSource bgm;
    private AudioClip bgm_Title;
    private AudioClip bgm_ZeroFloor;
    private AudioClip bgm_Stage1;     // 탑 하부, 공동묘지, 라바랜드, 탑 상부, 최종보스 라운드
    private AudioClip bgm_Stage2;
    private AudioClip bgm_Stage3;
    private AudioClip bgm_Stage4;
    private AudioClip bgm_Boss;
    private AudioClip bgm_FinalBoss;
    public float bgmVolume = 0.5f;

    [Header("EFFECT")]
    public AudioSource effector;
    public float effectVolume = 0.5f;

    private void Awake()
    {
        // 싱글톤
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        bgm_Title = Resources.Load<AudioClip>("BGM/Title");
        bgm_ZeroFloor = Resources.Load<AudioClip>("BGM/ZeroFloor");
        bgm_Stage1 = Resources.Load<AudioClip>("BGM/Stage1");
        bgm_Stage2 = Resources.Load<AudioClip>("BGM/Stage2");
        bgm_Stage3 = Resources.Load<AudioClip>("BGM/Stage3");
        bgm_Stage4 = Resources.Load<AudioClip>("BGM/Stage4");
        bgm_Boss = Resources.Load<AudioClip>("BGM/Boss");
    }

    private void Start()
    {
        bgm.Play();
    }

    public void PlayBGM(int idx)
    {
        if (idx == 0) bgm.clip = bgm_Title;
        // else if (idx == 1) AS.clip = bgm_ZeroFloor;
        else if (idx == 2) bgm.clip = bgm_ZeroFloor;
        else if (idx == 3) bgm.clip = bgm_Stage1;
        else if (idx == 4) bgm.clip = bgm_Stage2;
        else if (idx == 5) bgm.clip = bgm_Stage3;
        else if (idx == 6) bgm.clip = bgm_Stage4;
        else if (idx == 7) bgm.clip = bgm_Boss;
        else if (idx == 8) bgm.clip = bgm_FinalBoss;

        bgm.Play();
    }

    public void PlayEffect(string path)
    {
        effector.clip = Resources.Load<AudioClip>(path);
        effector.Play();
    }

    public void ChangeVolume(string type)
    {
        if (type == "BGM")
        {
            if (bgmVolume > 0)
                bgmVolume -= 0.125f;
            else
                bgmVolume = 0.5f;

            bgm.volume = bgmVolume;
        }
        else if (type == "EFFECT")
        {
            if (effectVolume > 0)
                effectVolume -= 0.125f;
            else
                effectVolume = 0.5f;

            effector.volume = effectVolume;
        }
    }

    public void Mute()
    {
        bgmVolume = 0;
        effectVolume = 0;

        bgm.volume = bgmVolume;
        effector.volume = effectVolume;
    }
}
