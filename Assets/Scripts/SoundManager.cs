using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager I = null;

    [Header("BGM")]
    public AudioSource bgm;
    public float bgmVolume = 0.5f;
    private bool bgmPlaying = true;

    [Header("EFFECT")]
    public AudioSource effector;
    public float effectVolume = 0.5f;

    private void Awake()
    {
        // ΩÃ±€≈Ê
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        //bgm_Title = Resources.Load<AudioClip>("BGM/Title");
        //bgm_ZeroFloor = Resources.Load<AudioClip>("BGM/ZeroFloor");
        //bgm_Stage1 = Resources.Load<AudioClip>("BGM/Stage1");
        //bgm_Stage2 = Resources.Load<AudioClip>("BGM/Stage2");
        //bgm_Stage3 = Resources.Load<AudioClip>("BGM/Stage3");
        //bgm_Stage4 = Resources.Load<AudioClip>("BGM/Stage4");
        //bgm_Boss = Resources.Load<AudioClip>("BGM/Boss");
    }

    private void Start()
    {
        bgm.Play();
    }

    public void PlayBGM(string path)
    {
        if (bgm.clip == Resources.Load<AudioClip>(path))
            return;

        bgm.clip = Resources.Load<AudioClip>(path);
        bgm.Play();
    }

    public void PauseBGM()
    {
        if (bgmPlaying)
            bgm.Pause();
        else
            bgm.UnPause();

        bgmPlaying = !bgmPlaying;
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
