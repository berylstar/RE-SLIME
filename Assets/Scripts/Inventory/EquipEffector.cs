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
        if (i == 3)                  // ������
        {
            GameController.probPotion += 3;
            GameController.probCoin += 3;
        }
        else if (i == 9)             // �� ��
        {
            GameController.playerMaxHP += 10;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 10)            // ������ ��
        {
            GameController.playerMaxHP += 20;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 11)            // ���̹�
        {
            GameController.playerDP += 1;
        }
        else if (i == 12)            // ������
        {
            GameController.playerSpeed += 10;
            GC.player.GetComponent<PlayerScript>().ApplyMoveSpeed();
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion += 3;
        }
        else if (i == 16)            // �ݼ� Ž����
        {
            GameController.probCoin += 3;
        }
        else if (i == 17)            // û�� ����
        {
            GameController.playerAP += 3;
        }
        else if (i == 18)            // ���� ������
        {
            GameController.RedCoin = true;
        }
        else if (i == 23)            // ����
        {
            GameController.potionEff += 3;
        }
        else if (i == 26)            // ��� Ƽ��
        {
            GameController.ShopGrade[0] += 2;
            GameController.ShopGrade[1] += 2;
        }
        else
            return;
    }

    public void EquipUnEffect(int i)
    {
        if (i == 3)                 // ������
        {
            GameController.probPotion -= 3;
            GameController.probCoin -= 3;
        }
        else if (i == 9)             // �� ��
        {
            GameController.playerMaxHP -= 10;
        }
        else if (i == 10)            // ������ ��
        {
            GameController.playerMaxHP -= 20;
        }
        else if (i == 11)            // ���̹�
        {
            GameController.playerDP -= 1;
        }
        else if (i == 12)            // ������
        {
            GameController.playerSpeed -= 10;
            GC.player.GetComponent<PlayerScript>().ApplyMoveSpeed();
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion -= 3;
        }
        else if (i == 16)            // �ݼ� Ž����
        {
            GameController.probCoin -= 3;
        }
        else if (i == 17)            // û�� ����
        {
            GameController.playerAP -= 3;
        }
        else if (i == 18)            // ���� ������
        {
            GameController.RedCoin = false;
        }
        else if (i == 23)            // ����
        {
            GameController.potionEff -= 3;
        }
        else if (i == 26)            // ��� Ƽ��
        {
            GameController.ShopGrade[0] -= 2;
            GameController.ShopGrade[1] -= 2;
        }
        else
            return;
    }

    public void EquipSkill(int i)
    {
        if (i == 1)                 // �ٳ���
        {
            GameController.ChangeHP(15);
        }
        else if (i == 5)            // ��� �ֻ���
        {
            GameController.ChangeHP(Random.Range(-5, 5));
        }
        else if (i == 14)           // ���� ����
        {
            StartCoroutine(Cloak());
        }
        else if (i == 15)           // ����
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
