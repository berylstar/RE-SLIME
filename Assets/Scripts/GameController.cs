using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static readonly WaitForSeconds delay_1s = new WaitForSeconds(1f);
    public static readonly WaitForSeconds delay_05s = new WaitForSeconds(0.5f);
    public static readonly WaitForSeconds delay_01s = new WaitForSeconds(0.1f);

    public GameObject player;
    public GameObject field;

    // STATUS
    public static int PlayerHP = 100;
    public static int playerAP = 10;
    public static int floor = 0;
}
