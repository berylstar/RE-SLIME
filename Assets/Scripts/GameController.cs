using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData
{
    // STATUS
    public int savedFloor = 0;

    // PLAYER
    public int playerLife = 3;

    // ITEM
    public int coin = 0;

    // EQUIP
    public List<string> EP = new List<string>();

    // GAME SYSTEM
    public bool endTutorial = false;
}

public class GameController : MonoBehaviour
{
    public static readonly WaitForSeconds delay_3s = new WaitForSeconds(3f);
    public static readonly WaitForSeconds delay_1s = new WaitForSeconds(1f);
    public static readonly WaitForSeconds delay_05s = new WaitForSeconds(0.5f);
    public static readonly WaitForSeconds delay_025s = new WaitForSeconds(0.25f);
    public static readonly WaitForSeconds delay_01s = new WaitForSeconds(0.1f);

    // STATUS
    public static int floor = 0;
    public static int savedFloor = 0;

    // PLAYER
    public static int playerLife = 3;
    public static int playerMaxHP = 100;
    public static int playerHP = playerMaxHP;
    public static int playerAP = 10;
    public static int playerDP = 0;
    public static float playerSpeed = 50f;
    public static List<float> speedStack = new List<float>() { };
    public static float playerTimeDamage = 1f;
    public static EquipScript skillC = null;
    public static EquipScript skillV = null;
    public static List<int> ShopGrade = new List<int>() { 20, 5 };

    // ITEM
    public static int coin = 0;
    public static int probCoin = 10;
    public static bool RedCoin = false;
    public static int potionEff = 5;
    public static int probPotion = 10;

    // EQUIPS
    public static List<string> myEquips = new List<string>();
    public static bool effBattery = false;
    public static bool effcrescent = false;
    public static bool effSkate = false;
    public static EquipScript effLastleaf = null;
    public static bool effGlasses = false;

    // GAME SYSYTEM
    public static bool esc = false;
    public static bool inDiaglogue = false;
    public static DialogueScript nowDialogue = null;
    public static bool inInven = false;
    public static bool inShop = false;
    public static bool inBox = false;
    public static bool endTutorial = false;

    public static bool Pause(int i)
    {
        // �켱 �������
        // 0      1              2          3         4       5
        // esc -> inDiaglogue -> inInven -> inShop -> inBox / all

        return esc || (inDiaglogue && (i >= 1)) || (inInven && (i >= 2)) || (inShop && (i >= 3)) || (inBox && (i >= 4));
    }

    public static void ChangeHP(int ch)
    {
        playerHP = Mathf.Min(playerMaxHP, Mathf.Max(0, playerHP + ch));
    }

    // �ӵ� ���� ���� �Լ�
    public static void ChangeSpeed(int ch)
    {
        playerSpeed += ch;
        PlayerScript.I.ApplyMoveSpeed();
    }

    public static void SpeedStackIn(float speed)
    {
        speedStack.Add(speed);
    }

    public static void SpeedStackOut()
    {
        int idx = speedStack.Count - 1;
        playerSpeed = speedStack[idx];
        speedStack.RemoveAt(idx);
    }

    // ���� �����
    public static void Restart()
    {
        Destroy(GameObject.Find("INVENTORY"));  // �κ��丮 �ı������ν� ����

        SceneManager.LoadScene("IntroScene");
    }

    // ���̺� ��ư
    public static void SAVE(int idx)
    {
        DataManager.I.SaveData(idx.ToString());
    }
}
