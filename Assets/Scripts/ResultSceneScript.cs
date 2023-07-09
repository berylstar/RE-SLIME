using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneScript : MonoBehaviour
{
    public Image imageEnd;
    public Text textEnd;
    public Text textResult;
    public Text textRestart;

    private void OnEnable()
    {
        textEnd.text = "GAME OVER...";
        textResult.text = "YOU REACHED AT " + GameController.savedFloor + " FLOOR\n" +
                          "IN-DUNGEON TIME : " + Time(GameController.inTime) + "\n" +
                          "COINS : " + GameController.getCoin + "\n" +
                          "KILLS : " + GameController.kills + "\n" + 
                          "SLIME WILL BE RETURN";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            BackToIntro();
    }

    private void BackToIntro()
    {
        GameController.floor = 0;
        GameController.playerHP = GameController.playerMaxHP;
        DataManager.I.RemoveData();
        SceneManager.LoadScene("IntroScene");
    }

    private string Time(int time)
    {
        int min = (int)(time / 60);
        int sec = time - min * 60;

        return "[" + string.Format("{0:D2}", min) + ":" + sec + "]";
    }
}
