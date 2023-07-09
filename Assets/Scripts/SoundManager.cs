using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager I = null;
    private AudioSource AS;

    [Header("BGM")]
    //public AudioClip bgm_Story;
    public AudioClip bgm_Title;
    public AudioClip bgm_ZeroFloor;
    public AudioClip[] bgm_InLevel;     // 탑 하부, 공동묘지, 라바랜드, 탑 상부, 최종보스 라운드
    public AudioClip bgm_Boss;
    public AudioClip bgm_FinalBoss;

    [Header("EFFECT")]
    public AudioClip s_UISpace;
    public AudioClip s_Punch;
    public AudioClip s_PlayerMove;

    private void Awake()
    {
        // 싱글톤
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        AS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        AS.Play();
    }

    public void PlayBGM(int idx)
    {
        if      (idx == 0) AS.clip = bgm_Title;
        // else if (idx == 1) AS.clip = bgm_ZeroFloor;
        else if (idx == 2) AS.clip = bgm_ZeroFloor;
        else if (idx == 3) AS.clip = bgm_InLevel[0];
        else if (idx == 4) AS.clip = bgm_InLevel[1];
        else if (idx == 5) AS.clip = bgm_InLevel[2];
        else if (idx == 6) AS.clip = bgm_InLevel[3];
        else if (idx == 7) AS.clip = bgm_Boss;
        else if (idx == 8) AS.clip = bgm_FinalBoss;

        AS.Play();
    }
}
