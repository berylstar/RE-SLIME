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
    private DialogueData nowDialogue = null;
    private int indexD = 0;
    private bool onTexting = false;

    [Header("PanelShop")]
    public GameObject panelShop;

    [Header("PanelBox")]
    public GameObject panelBox;

    [Header("PanelInven")]
    public GameObject panelForInven;
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
        GameController.situation.Push(SituationType.ESC);
        panelESC.SetActive(true);
        Time.timeScale = 0;
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

    #region DIALOGUE

    private void OnNext()
    {
        if (GameController.situation.Peek() != SituationType.DIALOGUE || onTexting)
            return;

        if (indexD < nowDialogue.dialogues.Count - 1)
        {
            indexD += 1;
            ShowDialogue(indexD);
        }
        else
        {
            EndDialogue();
        }
    }

    private void OnSkip()
    {
        if (GameController.situation.Peek() != SituationType.DIALOGUE)
            return;

        EndDialogue();
    }

    public void StartDialogue(DialogueData data)
    {
        nowDialogue = data;

        panelDialogue.SetActive(true);

        stackAssists.Push("[C] 대화 스킵");
        GameController.situation.Push(SituationType.DIALOGUE);

        indexD = 0;
        ShowDialogue(indexD);
    }

    public void ShowDialogue(int idx)
    {
        imageCharacter.sprite = nowDialogue.dialogues[idx].img;
        textTalker.text = nowDialogue.dialogues[idx].talker;
        StartCoroutine(TextAnimation(textDialogue, nowDialogue.dialogues[idx].talk));
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

    private void EndDialogue()
    {
        switch (nowDialogue.name)
        {
            case "Tutorial_1":
                GameController.tutorial[0] = true;
                GameController.coin += 5;
                break;

            case "Tutorial_Inventory":
                GameController.tutorial[1] = true;
                StairScript.I.Open();
                break;

            case "Recoder":
                DataManager.I.SaveData();
                break;

            case "Demon_1":
            case "Demon_2":
                BoardManager.I.bossDemon.GetComponent<BossDemon>().enabled = true;
                break;
        }

        StartCoroutine(CloseDialogue());
    }

    IEnumerator CloseDialogue()
    {
        panelDialogue.SetActive(false);
        nowDialogue = null;

        yield return GameController.delay_frame;

        stackAssists.Pop();
        GameController.situation.Pop();
    }

    #endregion
}
