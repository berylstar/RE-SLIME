using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ESCScript : MonoBehaviour
{
    public GameObject[] panel;
    public GameObject pick;
    public Text textBGM, textEFFECT;

    private int menuIndex = 0;
    private List<int> mmi = new List<int>() { 2, 3, 2 };
    private int pickIndex = 0;

    private void Start()
    {
        ActivePanel(0);
        menuIndex = 0;
    }

    private void Update()
    {
        if (!GameController.esc)
            return;

        if (Input.GetKeyDown(KeyCode.DownArrow)) SetIndex(Mathf.Min(pickIndex + 1, mmi[menuIndex]));
        if (Input.GetKeyDown(KeyCode.UpArrow)) SetIndex(Mathf.Max(pickIndex - 1, 0));

        pick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, pickIndex * -100, 0f);

        if (Input.GetKeyDown(KeyCode.Space))
            ButtonClick();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivePanel(0);
            menuIndex = 0;
            UIScript.I.ToggleESCPanel();
        }
            
    }

    public void ButtonClick()
    {
        if (menuIndex == 0)
        {
            if (pickIndex == 0)
            {
                ActivePanel(0);
                menuIndex = 0;
                UIScript.I.ToggleESCPanel();
            }
            else if (pickIndex == 1)
            {
                textBGM.text = "BGM:" + (int)(SoundManager.I.bgmVolume * 200) + "%";
                textEFFECT.text = "SFX:" + (int)(SoundManager.I.effectVolume * 200) + "%";
                ActivePanel(1);
            }
            else
            {
                ActivePanel(2);
            }
        }
        else if (menuIndex == 1)
        {
            if (pickIndex == 0)
            {
                SoundManager.I.ChangeVolume("BGM");
                textBGM.text = "BGM:" + (int)(SoundManager.I.bgmVolume * 200) + "%";
            }
            else if (pickIndex == 1)
            {
                SoundManager.I.ChangeVolume("EFFECT");
                textEFFECT.text = "SFX:" + (int)(SoundManager.I.effectVolume * 200) + "%";
            }
            else if (pickIndex == 2)
            {
                SoundManager.I.Mute();
                textBGM.text = "BGM:0%";
                textEFFECT.text = "SFX:0%";
            }
            else
            {
                ActivePanel(0);
            }
        }
        else if (menuIndex == 2)
        {
            if (pickIndex == 0)
            {
                ActivePanel(0);
            }
            else if (pickIndex == 1)
            {
                UIScript.I.ToggleESCPanel();
                SoundManager.I.PlayBGM("BGM/Title");
                GameController.Restart();
                SceneManager.LoadScene("TitleScene");
            }
        }

        SoundManager.I.PlayEffect("EFFECT/UIPick");
    }

    private void ActivePanel(int idx)
    {
        for (int i = 0; i < panel.Length; i++)
        {
            panel[i].SetActive(false);
        }

        menuIndex = idx;
        pickIndex = 0;
        panel[idx].SetActive(true);
    }

    // PointerEnter 감지용 함수
    public void SetIndex(int i)
    {
        pickIndex = i;
        SoundManager.I.PlayEffect("EFFECT/UIMove");
    }
}
