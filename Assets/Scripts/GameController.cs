using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static readonly WaitForSeconds delay_3s = new WaitForSeconds(3f);
    public static readonly WaitForSeconds delay_1s = new WaitForSeconds(1f);
    public static readonly WaitForSeconds delay_05s = new WaitForSeconds(0.5f);
    public static readonly WaitForSeconds delay_025s = new WaitForSeconds(0.25f);
    public static readonly WaitForSeconds delay_01s = new WaitForSeconds(0.1f);

    public GameObject player;
    public GameObject field;

    // STATUS
    public static int floor = 0;
    public static int savedFloor = 0;

    // PLAYER
    public static int playerLife = 3;
    public static int playerMaxHP = 100;
    public static int playerHP = playerMaxHP;
    public static int playerAP = 10;
    public static int playerDP = 0;
    public static float playerSpeed = 60f;
    public static List<float> speedStack = new List<float>();
    public static float playerTimeDamage = 1f;

    // ITEM
    public static int coin = 0;
    public static int probCoin = 10;
    public static bool canRedCoin = false;
    public static int potionEff = 5;
    public static int probPotion = 10;

    // GAME SYSYTEM
    public static bool pause = false;
    public static bool tutorialFirst = false;
    public static bool tutorialShop = false;

    public static void ChangeHP(int ch)
    {
        playerHP = Mathf.Min(playerMaxHP, Mathf.Max(0, playerHP + ch));
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
}
