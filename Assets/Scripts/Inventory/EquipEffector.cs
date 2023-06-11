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
        if (i == 2)                  // ������
        {
            GameController.effBattery = true;
        }
        else if (i == 3)             // ������
        {
            GameController.probPotion += binocular;
            GameController.probCoin += binocular;
        }
        else if (i == 4)             // �ʽ´�
        {
            GameController.effcrescent = true;
        }
        else if (i == 7)             // ���� �۷���
        {
            GameController.playerAP += gloves;
        }
        else if (i == 9)             // �� ��
        {
            GameController.playerMaxHP += halfstone;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 10)            // ������ ��
        {
            GameController.playerMaxHP += heartstone;
            GameController.playerHP = Mathf.Max(GameController.playerHP, GameController.playerMaxHP);
        }
        else if (i == 11)            // ���̹�
        {
            GameController.playerDP += helmet;
        }
        else if (i == 12)            // ������
        {
            GameController.playerSpeed += ice;
            GameObject.Find("PLAYER").GetComponent<PlayerScript>().ApplyMoveSpeed();
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion += machine;
        }
        else if (i == 16)            // �ݼ� Ž����
        {
            GameController.probCoin += metaldetector;
        }
        else if (i == 17)            // û�� ����
        {
            GameController.playerAP += pepper;
        }
        else if (i == 18)            // ���� ������
        {
            GameController.RedCoin = true;
        }
        else if (i == 21)            // �ζ��� ������Ʈ
        {
            GameController.effSkate = true;
        }
        else if (i == 23)            // ����
        {
            GameController.potionEff += straw;
        }
        else if (i == 26)            // ��� Ƽ��
        {
            GameController.ShopGrade[0] += goldenticket;
            GameController.ShopGrade[1] += goldenticket;
        }
        else if (i == 31)            // ������ �ٻ�
        {
            GameController.effLastleaf = equip;
        }
        else if (i == 32)            // �ö�ũ
        {
            GameController.playerAP += plask;
            GameController.playerDP -= plask;
        }
        else
            return;
    }

    public void EquipUnEffect(int i, EquipScript equip)
    {
        if (i == 2)                  // ������
        {
            GameController.effBattery = false;
        }
        else if(i == 3)              // ������
        {
            GameController.probPotion -= binocular;
            GameController.probCoin -= binocular;
        }
        else if (i == 4)             // �ʽ´�
        {
            GameController.effcrescent = false;
        }
        else if (i == 7)             // ���� �۷���
        {
            GameController.playerAP -= gloves;
        }
        else if (i == 9)             // �� ��
        {
            GameController.playerMaxHP -= halfstone;
        }
        else if (i == 10)            // ������ ��
        {
            GameController.playerMaxHP -= heartstone;
        }
        else if (i == 11)            // ���̹�
        {
            GameController.playerDP -= helmet;
        }
        else if (i == 12)            // ������
        {
            GameController.playerSpeed -= ice;
            GameObject.Find("PLAYER").GetComponent<PlayerScript>().ApplyMoveSpeed();
        }
        else if (i == 13)            // ESP-8266
        {
            GameController.probPotion -= machine;
        }
        else if (i == 16)            // �ݼ� Ž����
        {
            GameController.probCoin -= metaldetector;
        }
        else if (i == 17)            // û�� ����
        {
            GameController.playerAP -= pepper;
        }
        else if (i == 18)            // ���� ������
        {
            GameController.RedCoin = false;
        }
        else if (i == 21)            // �ζ��� ������Ʈ
        {
            GameController.effSkate = false;
        }
        else if (i == 23)            // ����
        {
            GameController.potionEff -= straw;
        }
        else if (i == 26)            // ��� Ƽ��
        {
            GameController.ShopGrade[0] -= goldenticket;
            GameController.ShopGrade[1] -= goldenticket;
        }
        else if (i == 31)            // ������ �ٻ�
        {
            GameController.effLastleaf = null;
        }
        else if (i == 32)            // �ö�ũ
        {
            GameController.playerAP -= plask;
            GameController.playerDP += plask;
        }
        else
            return;
    }

    public void EquipSkill(int i)
    {
        if (i == 1)                 // �ٳ���
        {
            GameController.ChangeHP(banana);
        }
        else if (i == 5)            // ��� �ֻ���
        {
            GameController.ChangeHP(Random.Range(-5, 5));
        }
        else if (i == 8)            // �콼 ����
        {
            Key();
        }
        else if (i == 14)           // ���� ����
        {
            StartCoroutine(Cloak());
        }
        else if (i == 15)           // ����
        {
            GameController.ChangeHP(mandoo);
        }
        else if (i == 22)           // �̷� ��� 6ȣ
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
