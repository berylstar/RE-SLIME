using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    // STATUS
    public int slotNumber = 0;
    public int savedFloor = 0;
    public int inTime = 0;
    public int getCoin = 5;
    public int kills = 0;

    // PLAYER
    public int playerLife = 3;

    // ITEM
    public int coin = 0;

    // EQUIP
    public List<string> myEquips = new List<string>();

    // GAME SYSTEM
    public List<bool> tutorial = new List<bool>() { false, false };

    public GameData() { }
    public GameData(int slot) { slotNumber = slot; }
}

public enum SituationType
{
    ESC         = 0,
    DIE         = 1,
    BOSS_APPEAR = 2,
    DIALOGUE    = 3,
    INVENTORY   = 4,
    SHOP        = 5,
    BOX         = 6,
    NORMAL      = 100,
}

public class GameController : MonoBehaviour
{
    public static readonly WaitForSeconds delay_3s = new WaitForSeconds(3f);
    public static readonly WaitForSeconds delay_1s = new WaitForSeconds(1f);
    public static readonly WaitForSeconds delay_05s = new WaitForSeconds(0.5f);
    public static readonly WaitForSeconds delay_025s = new WaitForSeconds(0.25f);
    public static readonly WaitForSeconds delay_01s = new WaitForSeconds(0.1f);
    public static readonly WaitForEndOfFrame delay_frame = new WaitForEndOfFrame();

    // STATUS
    public static int floor = 0;
    public static int savedFloor = 0;
    public static int inTime = 0;
    public static int getCoin = 5;
    public static int kills = 0;

    // PLAYER
    public static int playerLife = 3;
    public static int playerMaxHP = 100;
    public static int playerHP = playerMaxHP;
    public static int playerAP = 10;
    public static int playerDP = 0;
    public static float playerSpeed = 20f;
    public static Stack<float> speedStack = new Stack<float>() { };
    public static float playerTimeDamage = 1f;
    public static EquipScript skillC = null;
    public static EquipScript skillV = null;
    public static List<int> ShopGrade = new List<int>() { 20, 5 };

    // ITEM
    public static int coin = 0;
    public static int probCoin = 15;
    public static bool RedCoin = false;
    public static int probPotion = 20;
    public static int potionEff = 10;    

    // EQUIPS
    public static List<string> myEquips = new List<string>();
    public static bool effBattery = false;
    public static bool effcrescent = false;
    public static bool effSkate = false;
    public static EquipScript effLastleaf = null;
    public static bool effGlasses = false;
    public static bool efftalisman = false;

    // GAME SYSYTEM
    public static Stack<SituationType> situation = new Stack<SituationType>() { };
    public static DialogueScript nowDialogue = null;
    public static List<bool> tutorial = new List<bool>() { false, false};

    // HP 변동 함수
    public static void ChangeHP(int val)
    {
        if (playerHP + val > playerMaxHP) playerHP = playerMaxHP;
        else if (playerHP + val < 0)      playerHP = 0;
        else                              playerHP += val;
    }

    // 속도 변경 관련 함수
    public static void ChangeSpeed(float val)
    {
        playerSpeed += val;
        PlayerScript.I.ChangeSpeed();
    }

    // 게임 재시작
    public static void Restart()
    {
        floor = 0;

        playerMaxHP = 100;
        playerHP = 100;
        playerAP = 10;
        playerDP = 0;
        playerSpeed = 20f;
        speedStack.Clear();
        playerTimeDamage = 1f;
        skillC = null;
        skillV = null;
        ShopGrade = new List<int>() { 20, 5 };

        probCoin = 15;
        RedCoin = false;
        probPotion = 20;
        potionEff = 10;

        effBattery = false;
        effcrescent = false;
        effSkate = false;
        effLastleaf = null;
        effGlasses = false;
        efftalisman = false;

        nowDialogue = null;

        Destroy(GameObject.Find("INVENTORY"));  // 인벤토리 파괴함으로써 리셋
    }
}
