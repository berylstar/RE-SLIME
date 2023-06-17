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
    public GameObject stair;
    public GameObject kingslime;
    public GameObject sign;
    public GameObject coffinshop;
    public GameObject treasurebox;

    // STATUS
    public static int floor = 0;
    public static int savedFloor = 0;

    // PLAYER
    public static int playerLife = 1;
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
    public static bool effBattery = false;
    public static bool effcrescent = false;
    public static bool effSkate = false;
    public static EquipScript effLastleaf = null;

    // GAME SYSYTEM
    public static bool esc = false;
    public static bool inDiaglogue = false;
    public static DialogueScript nowDialogue = null;
    public static bool inInven = false;
    public static bool inShop = false;
    public static bool inBox = false;

    public static bool Pause(int i)
    {
        // 우선 순위대로
        // 0      1              2          3         4       5
        // esc -> inDiaglogue -> inInven -> inShop -> inBox / all

        return esc || (inDiaglogue && (i >= 1)) || (inInven && (i >= 2)) || (inShop && (i >= 3)) || (inBox && (i >= 4));
    }

    public static void ChangeHP(int ch)
    {
        playerHP = Mathf.Min(playerMaxHP, Mathf.Max(0, playerHP + ch));
    }

    // 시간 데미지
    public static IEnumerator TimeDamage(PlayerScript player)
    {
        float time = 0f;

        while (true)
        {
            if (player.CheckAlive() && floor > 0 && !Pause(5))
            {
                time += playerTimeDamage;

                if (time >= 1)
                {
                    ChangeHP(-1);
                    time = 0f;
                }
            }

            yield return delay_1s;
        }
    }

    // 속도 변경 관련 함수
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

    public static void Restart()
    {
        // static 변수들 리셋 시켜줘야 함
        Destroy(GameObject.Find("INVENTORY"));

        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
