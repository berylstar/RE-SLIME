using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header ("Panel")]
    public GameObject panelESC;

    [Header("Text")]
    public Text textFloor;
    public Text textLife, textCoin;
    public Text textPlayerHP, textPlayerAP, textPlayerDP, textPlayerSpeed, textPlayerTimeDamage;

    private void Update()
    {
        textLife.text = "SLIME\nx " + GameController.playerLife;
        textFloor.text = "FLOOR\n" + GameController.floor + " F";
        textCoin.text = "COIN\nx " + GameController.coin;

        textPlayerHP.text = "HP : " + GameController.PlayerHP + " / " + GameController.playerMaxHP;

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        if (!GameController.pause)
        {
            panelESC.SetActive(true);
            Time.timeScale = 0;
            GameController.pause = true;
        }
        else
        {
            panelESC.SetActive(false);
            Time.timeScale = 1;
            GameController.pause = false;
        }
            
    }
}
