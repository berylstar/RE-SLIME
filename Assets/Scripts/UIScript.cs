using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public static UIScript I = null;

    [Header("PanelStatus")]
    public Text textFloor;
    public Text textLife, textCoin;
    public Text textPlayerHP, textPlayerAP, textPlayerDP, textPlayerSpeed, textPlayerTimeDamage;

    [Header("PanelDie")]
    public GameObject panelDie;
    public Text textDie, textBackTo;

    [Header("PanelNextFloor")]
    public GameObject panelNextFloor;

    [Header("PanelESC")]
    public GameObject panelESC;
    public GameObject escPick;
    private int escIndex = 0;

    [Header("PanelDialogue")]
    public GameObject panelDialogue;
    public Image imageCharacter;
    public Text textTalker, textDialogue;

    [Header("PanelShop")]
    public GameObject panelShop;

    [Header("PanelBox")]
    public GameObject panelBox;

    private void Awake()
    {
        I = this;
    }

    private void Update()
    {
        textLife.text = "SLIME\nx " + GameController.playerLife;
        textFloor.text = "FLOOR\n" + GameController.floor + " F";
        textCoin.text = "COIN\nx " + GameController.coin;

        textPlayerHP.text = "HP : " + GameController.playerHP + " / " + GameController.playerMaxHP;
        textPlayerAP.text = "AP : " + GameController.playerAP;
        textPlayerDP.text = "DP : " + GameController.playerDP;
        textPlayerSpeed.text = "SPEED : " + GameController.playerSpeed;
        //textPlayerTimeDamage.text = "TimeDamage : " + GameController.playerTimeDamage;


        // ESC
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleESCPanel();

        if (GameController.esc)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) escIndex = Mathf.Max(escIndex - 1, 0);
            if (Input.GetKeyDown(KeyCode.DownArrow)) escIndex = Mathf.Min(escIndex + 1, 2);

            if (Input.GetKeyDown(KeyCode.Space))
                ESCSelect(escIndex);

            escPick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, escIndex * -100, 0f);
        }
    }

    private void ToggleESCPanel()
    {
        GameController.esc = !GameController.esc;
        panelESC.SetActive(GameController.esc);
        Time.timeScale = (GameController.esc) ? 0 : 1;

        escIndex = 0;
        escPick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    public void ESCSelect(int idx)
    {
        if (idx == 0)
        {
            ToggleESCPanel();
        }
        else if (idx == 1)
        {
            print("OPTION");
        }
        else if (idx == 2)
        {
            ToggleESCPanel();
            GameController.Restart();
        }
    }

    // ESC panel의 버튼에서 pointerEnter용 함수
    public void SetESCIndex(int idx)
    {
        escIndex = idx;
    }

    public void ShowDiePanel(int life)
    {
        if (life > 0)
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
        GameController.inDiaglogue = true;
        panelDialogue.SetActive(true);
        imageCharacter.sprite = character;
        textTalker.text = who;
        textDialogue.text = talk;
    }

    public void EndDialogue()
    {
        GameController.inDiaglogue = false;
        panelDialogue.SetActive(false);
    }
}
