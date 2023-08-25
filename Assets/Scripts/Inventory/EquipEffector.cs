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

    // ȿ�� ���� / ����
    public void ApplyEffect(int i, EquipScript equip, bool onoff)
    {
        int pm = onoff ? 1 : -1;

        if (i == 2)                  // ������
        {
            GameController.effBattery = onoff;
        }
        else if (i == 3)             // ������
        {
            GameController.probPotion += 2 * pm;
            GameController.probCoin += 2 * pm;
        }
        else if (i == 4)             // �ʽ´�
        {
            GameController.effcrescent = true;
        }
        else if (i == 6)             // ������ �帵ũ
        {
            GameController.ChangeSpeed(15 * pm);
        }
        else if (i == 7)             // ���� �۷���
        {
            GameController.playerAP += 6 * pm;
        }
        else if (i == 9)             // �� ��
        {
            GameController.playerMaxHP += 10 * pm;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 10)            // ������ ��
        {
            GameController.playerMaxHP += 25 * pm;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 11)            // ���̹�
        {
            GameController.playerDP += 1 * pm;
        }
        else if (i == 12)            // ������
        {
            GameController.ChangeSpeed(10 * pm);
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion += 3 * pm;
        }
        else if (i == 16)            // �ݼ� Ž����
        {
            GameController.probCoin += 3 * pm;
        }
        else if (i == 17)            // û�� ����
        {
            GameController.playerAP += 2 * pm;
        }
        else if (i == 18)            // ���� ������
        {
            GameController.RedCoin = onoff;
        }
        else if (i == 20)            // ���� �� ��
        {
            GameController.playerMaxHP += 5 * pm;
        }
        else if (i == 21)            // �ζ��� ������Ʈ
        {
            GameController.effSkate = onoff;
        }
        else if (i == 23)            // ����
        {
            GameController.potionEff += 3 * pm;
        }
        else if (i == 24)            // �Ķ� ���
        {
            GameController.playerDP += 1 * pm;
            GameController.playerAP -= 2 * pm;
        }
        else if (i == 26)            // ��� Ƽ��
        {
            GameController.ShopGrade[0] += 3 * pm;
            GameController.ShopGrade[1] += 3 * pm;
        }
        else if (i == 27)            // ����
        {
            GameController.playerDP += 1 * pm;
        }
        else if (i == 28)            // �Ϸ� ��Ÿ
        {
            GameController.playerAP += 4 * pm;
            GameController.playerDP += 1 * pm;
            GameController.ChangeSpeed(10 * pm);
        }
        else if (i == 29)            // ������ �ٻ�
        {
            GameController.effLastleaf = equip;
        }
        else if (i == 30)            // �ö�ũ
        {
            GameController.playerAP += 3 * pm;
            GameController.ChangeSpeed(10 * pm);
            GameController.playerDP -= 1 * pm;
        }
        else if (i == 32)            // 3D �Ȱ�
        {
            GameController.effGlasses = onoff;
        }
        else if (i == 33)            // ���� �ڼ�
        {
            GameController.probCoin += 3 * pm;
        }
        else if (i == 34)            // ����
        {
            GameController.probPotion += 3 * pm;
        }
        else if (i == 35)            // ���̾Ƹ��
        {

        }
        else if (i == 37)           // ����
        {
            GameController.efftalisman = onoff;
        }
        else if (i == 39)           // ��� �ν�
        {
            GameController.playerAP += 1 * pm;
        }
        else if (i == 41)           // �ź��� �����
        {
            GameController.playerMaxHP += 15 * pm;
            GameController.ChangeSpeed(-10 * pm);
        }
        else if (i == 42)           // ����
        {
            GameController.playerAP += 2 * pm;
        }
        else
            return;
    }

    public void EquipSkill(int i)
    {
        if (i == 1)                 // �ٳ���
        {
            GameController.ChangeHP(25);
        }
        else if (i == 5)            // ���� �ӽ�
        {
            GameController.ChangeHP(Random.Range(-5, 5));
        }
        else if (i == 8)            // �콼 ����
        {
            StairScript.I.Open();
        }
        else if (i == 14)           // ���� ����
        {
            StartCoroutine(Cloak());
        }
        else if (i == 15)           // ����
        {
            GameController.ChangeHP(10);
        }
        else if (i == 19)           // ġ�� ����
        {
            GameController.ChangeHP(15);
        }
        else if (i == 22 || i == 40)           // ������ or ���̹�
        {
            GameController.savedFloor = GameController.floor - 1;
            GameController.floor = 0;

            while (GameController.speedStack.Count > 0)
                GameController.SpeedStackOut();

            SceneManager.LoadScene("MainScene");
        }
        else if (i == 25)           // õ�� ����
        {
            GameObject.Find("CONTROLLER").GetComponent<BoardManager>().EquipThunder(5);
        }
        else if (i == 31)           // ������ ��
        {
            PlayerScript.I.EquipLasergun();
        }
        if (i == 36)                 // �����
        {
            GameController.ChangeHP(GameController.playerMaxHP/2);
        }
        if (i == 38)                 // ��ȣ��
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
