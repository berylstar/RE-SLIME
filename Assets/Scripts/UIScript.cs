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
        textPlayerSpeed.text = "SPEED : " + GameController.playerSpeed / 20;        // 기본 스피드 20
        textPlayerTimeDamage.text = "TimeDamage : " + GameController.playerTimeDamage;

        if (Input.GetKeyDown(KeyCode.Escape))
            ESC();

        if (GameController.esc)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))   escIndex -= 1;
            if (Input.GetKeyDown(KeyCode.DownArrow)) escIndex += 1;

            if      (escIndex < 0) escIndex = 2;
            else if (escIndex > 2) escIndex = 0;

            if (Input.GetKeyDown(KeyCode.Space))
                ESCSelect();

            escPick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -escIndex * 100, 0f);
        }
    }

    private void ESC()
    {
        if (!GameController.esc)
        {
            panelESC.SetActive(true);
            Time.timeScale = 0;
            GameController.esc = true;

            escIndex = 0;
            escPick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            panelESC.SetActive(false);
            Time.timeScale = 1;
            GameController.esc = false;
        }
    }

    private void ESCSelect()
    {
        if (escIndex == 0)
        {
            ESC();
        }
        else if (escIndex == 1)
        {
            print("OPTION");
        }
        else if (escIndex == 2)
        {
            ESC();
            GameController.Restart();
        }
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
