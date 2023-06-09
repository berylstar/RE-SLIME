using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipEffector : MonoBehaviour
{
    private GameController GC;

    private void Start()
    {
        GC = GameObject.Find("CONTROLLER").GetComponent<GameController>();
    }

    public void EquipEffect(int i)
    {
        if (i == 3)                  // 망원경
        {
            GameController.probPotion += 3;
            GameController.probCoin += 3;
        }
        else if (i == 9)             // 반 돌
        {
            GameController.playerMaxHP += 10;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 10)            // 마음의 돌
        {
            GameController.playerMaxHP += 20;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 11)            // 하이바
        {
            GameController.playerDP += 1;
        }
        else if (i == 12)            // 각얼음
        {
            GameController.playerSpeed += 10;
            GC.player.GetComponent<PlayerScript>().ApplyMoveSpeed();
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion += 3;
        }
        else if (i == 16)            // 금속 탐지기
        {
            GameController.probCoin += 3;
        }
        else if (i == 17)            // 청양 고추
        {
            GameController.playerAP += 3;
        }
        else if (i == 18)            // 돼지 저금통
        {
            GameController.RedCoin = true;
        }
        else if (i == 23)            // 빨대
        {
            GameController.potionEff += 3;
        }
        else if (i == 26)            // 골든 티켓
        {
            GameController.ShopGrade[0] += 2;
            GameController.ShopGrade[1] += 2;
        }
        else
            return;
    }

    public void EquipUnEffect(int i)
    {
        if (i == 3)                 // 망원경
        {
            GameController.probPotion -= 3;
            GameController.probCoin -= 3;
        }
        else if (i == 9)             // 반 돌
        {
            GameController.playerMaxHP -= 10;
        }
        else if (i == 10)            // 마음의 돌
        {
            GameController.playerMaxHP -= 20;
        }
        else if (i == 11)            // 하이바
        {
            GameController.playerDP -= 1;
        }
        else if (i == 12)            // 각얼음
        {
            GameController.playerSpeed -= 10;
            GC.player.GetComponent<PlayerScript>().ApplyMoveSpeed();
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion -= 3;
        }
        else if (i == 16)            // 금속 탐지기
        {
            GameController.probCoin -= 3;
        }
        else if (i == 17)            // 청양 고추
        {
            GameController.playerAP -= 3;
        }
        else if (i == 18)            // 돼지 저금통
        {
            GameController.RedCoin = false;
        }
        else if (i == 23)            // 빨대
        {
            GameController.potionEff -= 3;
        }
        else if (i == 26)            // 골든 티켓
        {
            GameController.ShopGrade[0] -= 2;
            GameController.ShopGrade[1] -= 2;
        }
        else
            return;
    }

    public void EquipSkill(int i)
    {
        if (i == 1)                 // 바나나
        {
            GameController.ChangeHP(15);
        }
        else if (i == 5)            // 사기 주사위
        {
            GameController.ChangeHP(Random.Range(-5, 5));
        }
        else if (i == 14)           // 투명 망토
        {
            StartCoroutine(Cloak());
        }
        else if (i == 15)           // 만두
        {
            GameController.ChangeHP(5);
        }
        else
            return;
    }

    IEnumerator Cloak()
    {
        GC.player.GetComponent<PlayerScript>().invincivity = true;
        GC.player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);
        yield return GameController.delay_3s;
        GC.player.GetComponent<PlayerScript>().invincivity = false;
        GC.player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
}
