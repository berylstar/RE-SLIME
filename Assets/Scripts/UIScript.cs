using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text textLife, textFloor, textCoin;
    public Text textPlayerHP, textPlayerAP, textPlayerDP, textPlayerSpeed, textPlayerTimeDamage;

    private void Update()
    {
        textLife.text = "SLIME\nx " + GameController.playerLife;
        textFloor.text = "FLOOR\n" + GameController.floor + " F";
        textCoin.text = "COIN\nx " + GameController.coin;

        textPlayerHP.text = "HP : " + GameController.PlayerHP + " / " + GameController.playerMaxHP;
    }
}
