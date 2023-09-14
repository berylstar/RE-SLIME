using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ESCScript : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject pick;
    public Text textBGM, textEFFECT;

    private Stack<int> uiStack = new Stack<int>();
    private List<int> maxIndex = new List<int>() { 2, 3, 2 };
    private int pickIndex = 0;

    private void OnEnable()
    {
        ActivePanel(0);
    }

    private void OnUpDown(InputValue value)
    {
        if (GameController.Pause(PauseType.ESC))
            return;

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
        if (GameController.Pause(PauseType.ESC))
            return;

        ButtonClick();
    }

    private void OnClose()
    {
        if (GameController.Pause(PauseType.ESC))
            return;

        StartCoroutine(ExitCo());
    }

    IEnumerator ExitCo()
    {
        uiStack.Clear();
        Time.timeScale = 1f;
        SoundManager.I.PauseBGM();

        yield return GameController.delay_frame;

        GameController.pause.Pop();
        UIScript.I.panelESC.SetActive(false);
    }

    public void ButtonClick()
    {
        switch (uiStack.Peek())
        {
            case 0:
                switch (pickIndex)
                {
                    case 0:
                        StartCoroutine(ExitCo());
                        break;

                    case 1:
                        textBGM.text = SoundManager.I.ReturnText("BGM");
                        textEFFECT.text = SoundManager.I.ReturnText("EFFECT");

                        ActivePanel(1);
                        break;

                    default:
                        ActivePanel(2);
                        break;
                }
                break;
            
            case 1:
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

            case 2:
                switch (pickIndex)
                {
                    case 0:
                        ActivePanel(-1);
                        break;

                    default:
                        Time.timeScale = 1f;
                        SoundManager.I.PauseBGM();
                        SoundManager.I.PlayBGM("BGM/Title");
                        GameController.Restart();
                        SceneManager.LoadScene("TitleScene");
                        break;
                }
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

    // PointerEnter ������ �Լ�
    public void SetIndex(int i)
    {
        pickIndex = i;
        pick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, pickIndex * -100, 0f);

        SoundManager.I.PlayEffect("EFFECT/UIMove");
    }
}
