using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipEffector : MonoBehaviour
{
    public static EquipEffector I = null;

    private readonly int banana = 15;
    private readonly int binocular = 2;
    private readonly int energydrink = 15;
    private readonly int gloves = 6;
    private readonly int halfstone = 10;
    private readonly int heartstone = 25;
    private readonly int helmet = 1;
    private readonly int ice = 10;
    private readonly int machine = 3;
    private readonly int mandoo = 5;
    private readonly int metaldetector = 3;
    private readonly int pepper = 2;
    private readonly int pizza = 10;
    private readonly int quarterstone = 5;
    private readonly int straw = 3;
    private readonly int talisman = 1;
    private readonly int thunder = 5;
    private readonly int goldenticket = 2; 
    private readonly int yellowtail = 1;
    // guitar - 4, 1, 10
    // plask - 3, -1
    private readonly int magnet = 3;
    private readonly int herb = 3;
    private readonly int wax = 1;
    private readonly int scissor = 2;

    private void Awake()
    {
        I = this;
    }

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
        else if (i == 6)             // 에너지 드링크
        {
            GameController.ChangeSpeed(energydrink);
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
            GameController.ChangeSpeed(ice);
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
        else if (i == 20)            // 반의 반 돌
        {
            GameController.playerMaxHP += quarterstone;
        }
        else if (i == 21)            // 인라인 스케이트
        {
            GameController.effSkate = true;
        }
        else if (i == 23)            // 빨대
        {
            GameController.potionEff += straw;
        }
        else if (i == 24)            // 부적
        {
            GameController.playerDP += talisman;
            GameController.playerAP -= talisman;
        }
        else if (i == 26)            // 골든 티켓
        {
            GameController.ShopGrade[0] += goldenticket;
            GameController.ShopGrade[1] += goldenticket;
        }
        else if (i == 27)            // 대방어
        {
            GameController.playerDP += yellowtail;
        }
        else if (i == 28)            // 일렉 기타
        {
            GameController.playerAP += 4;
            GameController.playerDP += 1;
            GameController.ChangeSpeed(10);
        }
        else if (i == 29)            // 마지막 잎새
        {
            GameController.effLastleaf = equip;
        }
        else if (i == 30)            // 플라스크
        {
            GameController.playerAP += 3;
            GameController.playerDP -= 1;
        }
        else if (i == 32)            // 3D 안경
        {
            GameController.effGlasses = true;
        }
        else if (i == 33)            // 말굽 자석
        {
            GameController.probCoin += magnet;
        }
        else if (i == 34)            // 약초
        {
            GameController.probPotion += herb;
        }
        else if (i == 35)            // 다이아몬드
        {

        }
        else if (i == 37)           // 파란 장미
        {
            GameController.effPerfume = true;
        }
        else if (i == 39)           // 헤어 왁스
        {
            GameController.playerAP += wax;
        }
        else if (i == 41)           // 거북이 등딱지
        {
            GameController.playerMaxHP += 15;
            GameController.ChangeSpeed(-10);
        }
        else if (i == 42)           // 가위
        {
            GameController.playerAP += scissor;
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
        else if (i == 6)             // 에너지 드링크
        {
            GameController.ChangeSpeed(-energydrink);
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
            GameController.ChangeSpeed(-ice);
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
        else if (i == 20)            // 반의 반 돌
        {
            GameController.playerMaxHP -= quarterstone;
        }
        else if (i == 21)            // 인라인 스케이트
        {
            GameController.effSkate = false;
        }
        else if (i == 23)            // 빨대
        {
            GameController.potionEff -= straw;
        }
        else if (i == 24)            // 부적
        {
            GameController.playerDP -= talisman;
            GameController.playerAP += talisman;
        }
        else if (i == 26)            // 골든 티켓
        {
            GameController.ShopGrade[0] -= goldenticket;
            GameController.ShopGrade[1] -= goldenticket;
        }
        else if (i == 27)            // 대방어
        {
            GameController.playerDP -= yellowtail;
        }
        else if (i == 28)            // 일렉 기타
        {
            GameController.playerAP -= 4;
            GameController.playerDP -= 1;
            GameController.ChangeSpeed(-10);
        }
        else if (i == 29)            // 마지막 잎새
        {
            GameController.effLastleaf = null;
        }
        else if (i == 30)            // 플라스크
        {
            GameController.playerAP -= 3;
            GameController.playerDP += 1;
        }
        else if (i == 32)            // 3D 안경
        {
            GameController.effGlasses = false;
        }
        else if (i == 33)            // 말굽 자석
        {
            GameController.probCoin -= magnet;
        }
        else if (i == 34)            // 약초
        {
            GameController.probPotion -= herb;
        }
        else if (i == 35)            // 다이아몬드
        {

        }
        else if (i == 37)           // 파란 장미
        {
            GameController.effPerfume = false;
        }
        else if (i == 39)           // 헤어 왁스
        {
            GameController.playerAP -= wax;
        }
        else if (i == 41)           // 거북이 등딱지
        {
            GameController.playerMaxHP -= 15;
            GameController.ChangeSpeed(10);
        }
        else if (i == 42)           // 가위
        {
            GameController.playerAP -= scissor;
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
        else if (i == 5)            // 슬롯 머신
        {
            GameController.ChangeHP(Random.Range(-5, 5));
        }
        else if (i == 8)            // 녹슨 열쇠
        {
            StairScript.I.Open();
        }
        else if (i == 14)           // 투명 망토
        {
            StartCoroutine(Cloak());
        }
        else if (i == 15)           // 만두
        {
            GameController.ChangeHP(mandoo);
        }
        else if (i == 19)           // 치즈 피자
        {
            GameController.ChangeHP(pizza);
        }
        else if (i == 22 || i == 40)           // 마법서 or 종이배
        {
            GameController.savedFloor = GameController.floor - 1;
            GameController.floor = 0;

            while (GameController.speedStack.Count > 0)
                GameController.SpeedStackOut();

            SceneManager.LoadScene("MainScene");
        }
        else if (i == 25)           // 천둥 번개
        {
            GameObject.Find("CONTROLLER").GetComponent<BoardManager>().EquipThunder(thunder);
        }
        else if (i == 31)           // 레이저 건
        {
            PlayerScript.I.EquipLasergun();
        }
        if (i == 36)                 // 독사과
        {
            GameController.ChangeHP(GameController.playerMaxHP/2);
        }
        if (i == 38)                 // 신호등
        {
            BoardManager.I.EquipTrafficlight();
        }
        else
            return;

        SoundManager.I.PlayEffect("EFFECT/EquipSkill");
    }

    IEnumerator Cloak()
    {
        GameObject player = GameObject.Find("PLAYER");

        PlayerScript.I.invincivity = true;
        player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);
        yield return GameController.delay_3s;
        PlayerScript.I.invincivity = false;
        player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
}
