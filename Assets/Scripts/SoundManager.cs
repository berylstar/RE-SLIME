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
    public float effectVolume = 1.0f;

    private void Awake()
    {
        // ΩÃ±€≈Ê
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        bgm.volume = bgmVolume;
        effector.volume = effectVolume;

        bgm.volume = 0.05f;
        effector.volume = 0.1f;

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
        switch (type)
        {
            case "BGM":
                if (bgmVolume > 0)
                    bgmVolume -= 0.125f;
                else
                    bgmVolume = 0.5f;

                bgm.volume = bgmVolume;
                break;

            case "EFFECT":
                if (effectVolume > 0)
                    effectVolume -= 0.25f;
                else
                    effectVolume = 1f;

                effector.volume = effectVolume;
                break;
        }
    }

    public void Mute()
    {
        bgmVolume = 0;
        effectVolume = 0;

        bgm.volume = bgmVolume;
        effector.volume = effectVolume;
    }

    public string ReturnText(string type)
    {
        if (type == "BGM")
            return $"BGM:{bgmVolume * 200}%";
        else
            return $"SFX:{effectVolume * 100}%";
    }
}
