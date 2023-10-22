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

    private readonly Stack<int> uiStack = new Stack<int>();
    private readonly List<int> maxIndex = new List<int>() { 2, 3, 2 };
    private int pickIndex = 0;

    private InputManager _input;
 
    private void OnEnable()
    {
        _input = InputManager.Instance;

        _input.MenuState.OnMove = UpDownMenuIndex;
        _input.MenuState.OnSpace = ButtonClick;
        _input.MenuState.OnESC = () => { StartCoroutine(ExitCo()); };
        _input.StateEnqueue(_input.MenuState);

        ActivePanel(0);
    }

    private void UpDownMenuIndex(Vector2 input)
    {
        if (input == Vector2.up)
        {
            pickIndex += (pickIndex > 0) ? -1 : 0;
        }
        else if (input == Vector2.down)
        {
            pickIndex += (pickIndex < maxIndex[uiStack.Peek()]) ? 1 : 0;
        }
        else
            return;

        SetIndex(pickIndex);
    }

    //private void OnUpDown(InputValue value)
    //{
    //    if (GameController.situation.Peek() != SituationType.ESC)
    //        return;

    //    float v = value.Get<float>();

    //    if (v > 0)
    //        pickIndex += (pickIndex < maxIndex[uiStack.Peek()]) ? 1 : 0;
    //    else if (v < 0)
    //        pickIndex += (pickIndex > 0) ? -1 : 0;
    //    else
    //        return;

    //    SetIndex(pickIndex);
    //}

    //private void OnPick()
    //{
    //    if (GameController.situation.Peek() != SituationType.ESC)
    //        return;

    //    ButtonClick();
    //}

    //private void OnClose()
    //{
    //    if (GameController.situation.Peek() != SituationType.ESC)
    //        return;

    //    StartCoroutine(ExitCo());
    //}

    IEnumerator ExitCo()
    {
        _input.StateDequeue();
        uiStack.Clear();
        Time.timeScale = 1f;
        SoundManager.I.PauseBGM();

        yield return GameController.delay_frame;

        GameController.situation.Pop();
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
                        _input.StateDequeue();
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

    // PointerEnter 감지용 함수
    public void SetIndex(int i)
    {
        pickIndex = i;
        pick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, pickIndex * -100, 0f);

        SoundManager.I.PlayEffect("EFFECT/UIMove");
    }
}
