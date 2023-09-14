using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch"))
        {
            DataManager.I.SaveData();
            UIScript.I.TextBlink("저장되었습니다.");
        }
    }
}
