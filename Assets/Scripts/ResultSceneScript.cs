using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneScript : MonoBehaviour
{
    public Image imageEnd;
    public Sprite[] spritesEnd;
    public Text textEnd;
    public Text textResult;
    public Text textRestart;
    public Image[] imageEquips;

    private bool ready = false;

    private void OnEnable()
    {
        if (GameController.floor == 100)
        {
            imageEnd.sprite = spritesEnd[0];
            textEnd.text = "GAME CLEAR !";
        }
        else
        {
            imageEnd.sprite = spritesEnd[1];
            textEnd.text = "GAME OVER...";
        }

        textResult.text = "     이번 슬라임은 " + GameController.floor + " 층까지...\n" +
                          "던전에 머문 시간 : " + Time(GameController.inTime) + "\n" +
                          "     획득한 코인 : " + GameController.getCoin + "\n" +
                          "         처치 수 : " + GameController.kills + "\n"; 
                          //+ "SLIME WILL BE RETURN";

        for (int i = 0; i < imageEquips.Length; i++)
        {
            imageEquips[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < InventoryScript.I.GottenEquips.Count; i++)
        {
            imageEquips[i].gameObject.SetActive(true);
            imageEquips[i].sprite = InventoryScript.I.GottenEquips[i].ReturnSprite();
            imageEquips[i].SetNativeSize();
        }

        StartCoroutine(Delay());
    }

    private void Update()
    {
        if (!ready)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            BackToIntro();
    }

    IEnumerator Delay()
    {
        yield return GameController.delay_3s;
        ready = true;
        textRestart.gameObject.SetActive(true);
    }

    private void BackToIntro()
    {
        GameController.Restart();
        DataManager.I.RemoveData();
        SceneManager.LoadScene("TitleScene");
        SoundManager.I.PlayBGM("BGM/Title");
    }

    private string Time(int time)
    {
        int min = (int)(time / 60);
        int sec = time - min * 60;

        return "[" + string.Format("{0:D2}", min) + ":" + sec + "]";
    }
}
