using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager I = null;

    [Header("BGM")]
    public AudioSource bgm;
    public AudioClip bgm_Title;
    public AudioClip bgm_ZeroFloor;
    public AudioClip[] bgm_InLevel;     // 탑 하부, 공동묘지, 라바랜드, 탑 상부, 최종보스 라운드
    public AudioClip bgm_Boss;
    public AudioClip bgm_FinalBoss;
    public float bgmVolume = 0.5f;

    [Header("EFFECT")]
    public AudioSource effector;
    public AudioClip e_UIPick;
    public AudioClip e_UIMove;
    public float effectVolume = 0.5f;

    private void Awake()
    {
        // 싱글톤
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        bgm.Play();
    }

    public void PlayBGM(int idx)
    {
        if      (idx == 0) bgm.clip = bgm_Title;
        // else if (idx == 1) AS.clip = bgm_ZeroFloor;
        else if (idx == 2) bgm.clip = bgm_ZeroFloor;
        else if (idx == 3) bgm.clip = bgm_InLevel[0];
        else if (idx == 4) bgm.clip = bgm_InLevel[1];
        else if (idx == 5) bgm.clip = bgm_InLevel[2];
        else if (idx == 6) bgm.clip = bgm_InLevel[3];
        else if (idx == 7) bgm.clip = bgm_Boss;
        else if (idx == 8) bgm.clip = bgm_FinalBoss;

        bgm.Play();
    }

    public void PlayEffect(string w)
    {
        if      (w == "e_UIMove") effector.clip = e_UIMove;
        else if (w == "e_UIPick") effector.clip = e_UIPick;

        effector.Play();
    }

    public void ChangeVolume(string type)
    {
        if (type == "BGM")
        {
            bgm.volume = bgmVolume;
        }
        else if (type == "EFFECT")
        {
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
