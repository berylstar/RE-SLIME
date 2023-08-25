using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public GameObject[] panel;
    public GameObject pick;
    public Text textBGM, textEFFECT, textSlot;
    public Button buttonLoad;

    private int menuIndex = 0;
    private List<int> mmi = new List<int>() { 2, 3, 2, 3 };
    private int pickIndex = 0;
    private int slot = 0;

    private void Start()
    {
        ActivePanel(0);
        menuIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) SetIndex(Mathf.Min(pickIndex + 1, mmi[menuIndex]));
        if (Input.GetKeyDown(KeyCode.UpArrow)) SetIndex(Mathf.Max(pickIndex - 1, 0));

        pick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, pickIndex * -100, 0f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && (menuIndex > 0))
            ActivePanel(--menuIndex);
    }

    public void ButtonClick()
    {
        if (menuIndex == 0)
        {
            if (pickIndex == 0)
            {
                ActivePanel(1);
            }
            else if (pickIndex == 1)
            {
                textBGM.text = "BGM:" + (int)(SoundManager.I.bgmVolume * 200) + "%";
                textEFFECT.text = "SFX:" + (int)(SoundManager.I.effectVolume * 100) + "%";
                ActivePanel(3);
            }
            else
            {
                Application.Quit();
            }
        }
        else if (menuIndex == 1)
        {
            if (pickIndex <= 2)
            {
                buttonLoad.interactable = File.Exists(DataManager.I.ReturnPath(pickIndex));
                slot = pickIndex;
                textSlot.text = "< SLOT " + (pickIndex + 1).ToString() + " >";

                ActivePanel(2);
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
                if (!buttonLoad.interactable)
                    return;

                DataManager.I.LoadData(slot);
                SceneManager.LoadScene("MainScene");
            }
            else if (pickIndex == 1)
            {
                DataManager.I.NewData(slot);
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                ActivePanel(1);
            }
        }
        else if (menuIndex == 3)
        {
            if (pickIndex == 0)
            {
                SoundManager.I.ChangeVolume("BGM");
                textBGM.text = "BGM:" + (int)(SoundManager.I.bgmVolume * 200) + "%";
            }
            else if (pickIndex == 1)
            {
                SoundManager.I.ChangeVolume("EFFECT");
                textEFFECT.text = "SFX:" + (int)(SoundManager.I.effectVolume * 100) + "%";
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
