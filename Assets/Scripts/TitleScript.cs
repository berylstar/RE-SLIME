using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleScript : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject pick;
    public Text textBGM, textEFFECT, textSlot;
    public Button buttonLoad;

    private Stack<int> uiStack = new Stack<int>();
    private readonly List<int> maxIndex = new List<int>() { 2, 3, 2, 3 };
    private int pickIndex = 0;
    private int slot = 0;

    private void OnEnable()
    {
        ActivePanel(0);
    }

    private void OnUpDown(InputValue value)
    {
        float v = value.Get<float>();

        if (v > 0)
            pickIndex += (pickIndex < maxIndex[uiStack.Peek()]) ? 1 : 0;
        else if (v < 0)
            pickIndex += (pickIndex > 0) ? -1 : 0;
        else
            return;

        SetIndex(pickIndex);
    }

    private void OnPick()
    {
        ButtonClick();
    }

    private void OnClose()
    {
        if (uiStack.Peek() <= 0)
            return;

        ActivePanel(-1);
    }

    public void ButtonClick()
    {
        switch(uiStack.Peek())
        {
            case 0:
                switch (pickIndex)
                {
                    case 0:
                        ActivePanel(1);
                        break;

                    case 1:
                        textBGM.text = SoundManager.I.ReturnText("BGM");
                        textEFFECT.text = SoundManager.I.ReturnText("EFFECT");

                        ActivePanel(3);
                        break;

                    default:
                        Application.Quit();
                        break;
                }
                break;

            case 1:
                switch (pickIndex)
                {
                    case 0:
                    case 1:
                    case 2:
                        buttonLoad.interactable = File.Exists(DataManager.I.ReturnPath(pickIndex));
                        slot = pickIndex;
                        textSlot.text = $"< SLOT {pickIndex + 1} >";

                        ActivePanel(2);
                        break;

                    default:
                        ActivePanel(-1);
                        break;
                }
                break;

            case 2:

                switch (pickIndex)
                {
                    case 0:
                        if (!buttonLoad.interactable)
                            return;

                        DataManager.I.LoadData(slot);
                        SceneManager.LoadScene("MainScene");
                        break;

                    case 1:
                        DataManager.I.NewData(slot);
                        SceneManager.LoadScene("MainScene");
                        break;

                    default:
                        ActivePanel(-1);
                        break;
                }
                break;

            case 3:
                switch (pickIndex)
                {
                    case 0:
                        SoundManager.I.ChangeVolume("BGM");
                        textBGM.text = SoundManager.I.ReturnText("BGM");
                        break;

                    case 1:
                        SoundManager.I.ChangeVolume("EFFECT");
                        textEFFECT.text = SoundManager.I.ReturnText("EFFECT");
                        break;

                    case 2:
                        SoundManager.I.Mute();
                        textBGM.text = "BGM:0%";
                        textEFFECT.text = "SFX:0%";
                        break;

                    default:
                        ActivePanel(-1);
                        break;
                }
                break;

            default:
                break;
        }

        SoundManager.I.PlayEffect("EFFECT/UIPick");
    }

    private void ActivePanel(int idx)
    {
        if (idx < 0)
            uiStack.Pop();
        else
            uiStack.Push(idx);

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }    

        pickIndex = 0;
        pick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);

        panels[uiStack.Peek()].SetActive(true);
    }

    // PointerEnter 감지용 함수
    public void SetIndex(int i)
    {
        pickIndex = i;
        pick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, pickIndex * -100, 0f);

        SoundManager.I.PlayEffect("EFFECT/UIMove");
    }
}
