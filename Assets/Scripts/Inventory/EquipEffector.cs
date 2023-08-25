using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipEffector : MonoBehaviour
{
    public static EquipEffector I = null;

    private void Awake()
    {
        I = this;
    }

    // 효과 적용 / 해제
    public void ApplyEffect(int i, EquipScript equip, bool onoff)
    {
        int pm = onoff ? 1 : -1;

        if (i == 2)                  // 건전지
        {
            GameController.effBattery = onoff;
        }
        else if (i == 3)             // 망원경
        {
            GameController.probPotion += 2 * pm;
            GameController.probCoin += 2 * pm;
        }
        else if (i == 4)             // 초승달
        {
            GameController.effcrescent = true;
        }
        else if (i == 6)             // 에너지 드링크
        {
            GameController.ChangeSpeed(15 * pm);
        }
        else if (i == 7)             // 복싱 글러브
        {
            GameController.playerAP += 6 * pm;
        }
        else if (i == 9)             // 반 돌
        {
            GameController.playerMaxHP += 10 * pm;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 10)            // 마음의 돌
        {
            GameController.playerMaxHP += 25 * pm;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 11)            // 하이바
        {
            GameController.playerDP += 1 * pm;
        }
        else if (i == 12)            // 각얼음
        {
            GameController.ChangeSpeed(10 * pm);
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion += 3 * pm;
        }
        else if (i == 16)            // 금속 탐지기
        {
            GameController.probCoin += 3 * pm;
        }
        else if (i == 17)            // 청양 고추
        {
            GameController.playerAP += 2 * pm;
        }
        else if (i == 18)            // 돼지 저금통
        {
            GameController.RedCoin = onoff;
        }
        else if (i == 20)            // 반의 반 돌
        {
            GameController.playerMaxHP += 5 * pm;
        }
        else if (i == 21)            // 인라인 스케이트
        {
            GameController.effSkate = onoff;
        }
        else if (i == 23)            // 빨대
        {
            GameController.potionEff += 3 * pm;
        }
        else if (i == 24)            // 파란 장미
        {
            GameController.playerDP += 1 * pm;
            GameController.playerAP -= 2 * pm;
        }
        else if (i == 26)            // 골든 티켓
        {
            GameController.ShopGrade[0] += 3 * pm;
            GameController.ShopGrade[1] += 3 * pm;
        }
        else if (i == 27)            // 대방어
        {
            GameController.playerDP += 1 * pm;
        }
        else if (i == 28)            // 일렉 기타
        {
            GameController.playerAP += 4 * pm;
            GameController.playerDP += 1 * pm;
            GameController.ChangeSpeed(10 * pm);
        }
        else if (i == 29)            // 마지막 잎새
        {
            GameController.effLastleaf = equip;
        }
        else if (i == 30)            // 플라스크
        {
            GameController.playerAP += 3 * pm;
            GameController.ChangeSpeed(10 * pm);
            GameController.playerDP -= 1 * pm;
        }
        else if (i == 32)            // 3D 안경
        {
            GameController.effGlasses = onoff;
        }
        else if (i == 33)            // 말굽 자석
        {
            GameController.probCoin += 3 * pm;
        }
        else if (i == 34)            // 약초
        {
            GameController.probPotion += 3 * pm;
        }
        else if (i == 35)            // 다이아몬드
        {

        }
        else if (i == 37)           // 부적
        {
            GameController.efftalisman = onoff;
        }
        else if (i == 39)           // 헤어 왁스
        {
            GameController.playerAP += 1 * pm;
        }
        else if (i == 41)           // 거북이 등딱지
        {
            GameController.playerMaxHP += 15 * pm;
            GameController.ChangeSpeed(-10 * pm);
        }
        else if (i == 42)           // 가위
        {
            GameController.playerAP += 2 * pm;
        }
        else
            return;
    }

    public void EquipSkill(int i)
    {
        if (i == 1)                 // 바나나
        {
            GameController.ChangeHP(25);
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
            GameController.ChangeHP(10);
        }
        else if (i == 19)           // 치즈 피자
        {
            GameController.ChangeHP(15);
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
            GameObject.Find("CONTROLLER").GetComponent<BoardManager>().EquipThunder(5);
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
        PlayerScript.I.invincivity = true;
        PlayerScript.I.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);
        yield return GameController.delay_3s;
        PlayerScript.I.invincivity = false;
        PlayerScript.I.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
