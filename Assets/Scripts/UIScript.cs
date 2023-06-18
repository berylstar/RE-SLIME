using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelDie;
    public GameObject panelNextFloor;

    [Header("UI Text")]
    public Text textFloor;
    public Text textLife, textCoin;
    public Text textPlayerHP, textPlayerAP, textPlayerDP, textPlayerSpeed, textPlayerTimeDamage;

    [Header("ESC")]
    public GameObject panelESC;
    public GameObject escPick;
    private int escIndex = 0;

    [Header("Dialogue")]
    public GameObject panelDialogue;
    public Image imageCharacter;
    public Text textTalker, textDialogue;

    [Header("CoffinShop")]
    public GameObject panelShop;

    [Header("TreasureBox")]
    public GameObject panelBox;

    private void Update()
    {
        textLife.text = "SLIME\nx " + GameController.playerLife;
        textFloor.text = "FLOOR\n" + GameController.floor + " F";
        textCoin.text = "COIN\nx " + GameController.coin;

        textPlayerHP.text = "HP : " + GameController.playerHP + " / " + GameController.playerMaxHP;
        textPlayerAP.text = "AP : " + GameController.playerAP;
        textPlayerDP.text = "DP : " + GameController.playerDP;
        textPlayerSpeed.text = "SPEED : " + GameController.playerSpeed;
        textPlayerTimeDamage.text = "TimeDamage : " + GameController.playerTimeDamage;

        if (Input.GetKeyDown(KeyCode.Escape))
            ESC();

        if (GameController.esc)
            InESCKeyInput();
    }

    private void InESCKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) escIndex = Mathf.Max(escIndex - 1, 0);
        if (Input.GetKeyDown(KeyCode.DownArrow)) escIndex = Mathf.Min(escIndex + 1, 2);

        if (Input.GetKeyDown(KeyCode.Space))
            ESCSelect(escIndex);

        escPick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, escIndex * -100, 0f);
    }

    private void ESC()
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
            ESC();
        }
        else if (idx == 1)
        {
            print("OPTION");
        }
        else if (idx == 2)
        {
            ESC();
            GameController.Restart();
        }
    }

    public void SetESCIndex(int idx)
    {
        escIndex = idx;
    }

    public void Dialogue(Sprite character, string who, string talk)
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
