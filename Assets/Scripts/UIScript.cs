using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    private readonly WaitForSeconds textDelay = new WaitForSeconds(0.01f);
    public static UIScript I = null;

    [Header("PanelStatus")]
    public Text textFloor;
    public Text textLife, textCoin;
    public Text textPlayerHP, textPlayerAP, textPlayerDP, textPlayerSpeed, textAssist;
    public Stack<string> stackAssists = new Stack<string>();

    [Header("PanelDie")]
    public GameObject panelDie;
    public Text textDie, textBackTo;

    [Header("PanelNextFloor")]
    public GameObject panelNextFloor;

    [Header("PanelBoss")]
    public GameObject panelBoss;

    [Header("PanelESC")]
    public GameObject panelESC;

    [Header("PanelDialogue")]
    public GameObject panelDialogue;
    public Image imageCharacter;
    public Text textTalker, textDialogue;
    public bool onTexting = false;

    [Header("PanelShop")]
    public GameObject panelShop;

    [Header("PanelBox")]
    public GameObject panelBox;

    [Header("PanelRecode")]
    public GameObject PanelRecorder;

    [Header("PanelInvenInfo")]
    public GameObject panelInvenInfo;
    public Text textName, textGrade, textPrice, textEffect;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        SoundManager.I.PlayBGM("BGM/ZeroFloor");
    }

    private void Update()
    {
        textLife.text = "SLIME\nx " + GameController.playerLife;
        textFloor.text = "FLOOR\n" + GameController.floor + " F";
        textCoin.text = "COIN\nx " + GameController.coin;

        textPlayerHP.text = GameController.playerHP + " / " + GameController.playerMaxHP;
        textPlayerAP.text = GameController.playerAP.ToString();
        textPlayerDP.text = GameController.playerDP.ToString();
        textPlayerSpeed.text = GameController.playerSpeed.ToString();

        if (stackAssists.Count > 0)
            textAssist.text = stackAssists.Peek();
        else
            textAssist.text = "";
    }

    public void EnterESC()
    {
        GameController.pause.Push(PauseType.ESC);
        panelESC.SetActive(true);
        Time.timeScale = 0;
        SoundManager.I.PauseBGM();
    }

    public void ExitESC()
    {
        GameController.pause.Pop();
        panelESC.SetActive(false);
        Time.timeScale = 1;
        SoundManager.I.PauseBGM();
    }

    public void ShowDiePanel(int life)
    {
        if (life > 1)
        {
            textDie.text = "YOU DIE";
            textBackTo.text = "BACK TO 0 FLOOR...";
        }
        else
        {
            textDie.text = "YOU FAIL";
            textBackTo.text = "CONTINUE...?";
        }

        panelDie.SetActive(true);
    }

    public void ShowDialogue(Sprite character, string who, string talk)
    {
        if (GameController.Pause(PauseType.DIALOGUE))
            GameController.pause.Push(PauseType.DIALOGUE);

        panelDialogue.SetActive(true);
        imageCharacter.sprite = character;
        textTalker.text = who;
        StartCoroutine(TextAnimation(textDialogue, talk));
    }

    // 한글자씩 보이는 텍스트 효과
    IEnumerator TextAnimation(Text text, string talk)
    {
        string write = "";
        onTexting = true;

        for (int i = 0; i < talk.Length; i++)
        {
            write += talk[i];
            text.text = write;
            yield return textDelay;
        }
        onTexting = false;
    }

    public void EndDialogue()
    {
        GameController.pause.Pop();
        panelDialogue.SetActive(false);
    }

    public void TextBlink(string message)
    {
        StartCoroutine(TextBlinkCo(message));
    }

    IEnumerator TextBlinkCo(string message)
    {
        stackAssists.Push(message);

        yield return GameController.delay_3s;

        stackAssists.Pop();
    }
}
