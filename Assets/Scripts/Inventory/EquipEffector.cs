using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipEffector : MonoBehaviour
{
    private int banana = 15;
    private int binocular = 3;
    private int gloves = 6;
    private int halfstone = 10;
    private int heartstone = 25;
    private int helmet = 1;
    private float ice = 10f;
    private int machine = 3;
    private int mandoo = 5;
    private int metaldetector = 3;
    private int pepper = 2;
    private int straw = 3;
    private int goldenticket = 2;
    private int plask = 2;

    public void EquipEffect(int i, EquipScript equip)
    {
        if (i == 2)                  // 건전지
        {
            GameController.effBattery = true;
        }
        else if (i == 3)             // 망원경
        {
            GameController.probPotion += binocular;
            GameController.probCoin += binocular;
        }
        else if (i == 4)             // 초승달
        {
            GameController.effcrescent = true;
        }
        else if (i == 7)             // 복싱 글러브
        {
            GameController.playerAP += gloves;
        }
        else if (i == 9)             // 반 돌
        {
            GameController.playerMaxHP += halfstone;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 10)            // 마음의 돌
        {
            GameController.playerMaxHP += heartstone;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 11)            // 하이바
        {
            GameController.playerDP += helmet;
        }
        else if (i == 12)            // 각얼음
        {
            GameController.playerSpeed += ice;
            GameObject.Find("PLAYER").GetComponent<PlayerScript>().ApplyMoveSpeed();
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion += machine;
        }
        else if (i == 16)            // 금속 탐지기
        {
            GameController.probCoin += metaldetector;
        }
        else if (i == 17)            // 청양 고추
        {
            GameController.playerAP += pepper;
        }
        else if (i == 18)            // 돼지 저금통
        {
            GameController.RedCoin = true;
        }
        else if (i == 21)            // 인라인 스케이트
        {
            GameController.effSkate = true;
        }
        else if (i == 23)            // 빨대
        {
            GameController.potionEff += straw;
        }
        else if (i == 26)            // 골든 티켓
        {
            GameController.ShopGrade[0] += goldenticket;
            GameController.ShopGrade[1] += goldenticket;
        }
        else if (i == 31)            // 마지막 잎새
        {
            GameController.effLastleaf = equip;
        }
        else if (i == 32)            // 플라스크
        {
            GameController.playerAP += plask;
            GameController.playerDP -= plask;
        }
        else
            return;
    }

    public void EquipUnEffect(int i, EquipScript equip)
    {
        if (i == 2)                  // 건전지
        {
            GameController.effBattery = false;
        }
        else if(i == 3)              // 망원경
        {
            GameController.probPotion -= binocular;
            GameController.probCoin -= binocular;
        }
        else if (i == 4)             // 초승달
        {
            GameController.effcrescent = false;
        }
        else if (i == 7)             // 복싱 글러브
        {
            GameController.playerAP -= gloves;
        }
        else if (i == 9)             // 반 돌
        {
            GameController.playerMaxHP -= halfstone;
        }
        else if (i == 10)            // 마음의 돌
        {
            GameController.playerMaxHP -= heartstone;
        }
        else if (i == 11)            // 하이바
        {
            GameController.playerDP -= helmet;
        }
        else if (i == 12)            // 각얼음
        {
            GameController.playerSpeed -= ice;
            GameObject.Find("PLAYER").GetComponent<PlayerScript>().ApplyMoveSpeed();
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion -= machine;
        }
        else if (i == 16)            // 금속 탐지기
        {
            GameController.probCoin -= metaldetector;
        }
        else if (i == 17)            // 청양 고추
        {
            GameController.playerAP -= pepper;
        }
        else if (i == 18)            // 돼지 저금통
        {
            GameController.RedCoin = false;
        }
        else if (i == 21)            // 인라인 스케이트
        {
            GameController.effSkate = false;
        }
        else if (i == 23)            // 빨대
        {
            GameController.potionEff -= straw;
        }
        else if (i == 26)            // 골든 티켓
        {
            GameController.ShopGrade[0] -= goldenticket;
            GameController.ShopGrade[1] -= goldenticket;
        }
        else if (i == 31)            // 마지막 잎새
        {
            GameController.effLastleaf = null;
        }
        else if (i == 32)            // 플라스크
        {
            GameController.playerAP -= plask;
            GameController.playerDP += plask;
        }
        else
            return;
    }

    public void EquipSkill(int i)
    {
        if (i == 1)                 // 바나나
        {
            GameController.ChangeHP(banana);
        }
        else if (i == 5)            // 사기 주사위
        {
            GameController.ChangeHP(Random.Range(-5, 5));
        }
        else if (i == 8)            // 녹슨 열쇠
        {
            Key();
        }
        else if (i == 14)           // 투명 망토
        {
            StartCoroutine(Cloak());
        }
        else if (i == 15)           // 만두
        {
            GameController.ChangeHP(mandoo);
        }
        else if (i == 22)           // 미래 기술 6호
        {
            GameController.savedFloor = GameController.floor - 1;
            GameController.floor = 0;

            while (GameController.speedStack.Count > 0)
                GameController.SpeedStackOut();

            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        else
            return;
    }

    IEnumerator Cloak()
    {
        GameObject player = GameObject.Find("PLAYER");

        player.GetComponent<PlayerScript>().invincivity = true;
        player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);
        yield return GameController.delay_3s;
        player.GetComponent<PlayerScript>().invincivity = false;
        player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    private void Key()
    {
        StairScript stair = GameObject.Find("STAIR").GetComponent<StairScript>();

        stair.StairOpen();
    }
}
