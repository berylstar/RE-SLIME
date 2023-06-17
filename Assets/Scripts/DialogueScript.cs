using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum DialogueType
{
    KINGSLIME,
    SIGN,
}

public class DialogueScript : MonoBehaviour
{
    public DialogueType type;

    public int start, end;

    public List<Sprite> img = new List<Sprite>();
    public List<string> talker = new List<string>();
    public List<string> talk = new List<string>();

    private UIScript US;
    private int index = 0;

    private void Start()
    {
        US = GameObject.Find("CONTROLLER").GetComponent<UIScript>();
    }

    private void Update()
    {
        if (!GameController.inDiaglogue || GameController.Pause(0))
            return;

        if (GameController.nowDialogue != this)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            index += 1;

            if (index >= end)
            {
                StartCoroutine(CloseTutorial());

                if (type == DialogueType.KINGSLIME && index == 4)
                {
                    GameController.coin += 5;       // 테스트용
                    start = 4;
                    end = 6;
                }
                    
            }
            else
                US.Dialogue(img[index], talker[index], talk[index]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch"))
        {
            index = start;
            GameController.nowDialogue = this;
            US.Dialogue(img[start], talker[start], talk[start]);
        }
    }

    // 바로 다시 NPC와 대화를 막기위한 잠깐의 딜레이
    IEnumerator CloseTutorial()
    {
        GameController.nowDialogue = null;

        yield return GameController.delay_01s;

        US.EndDialogue();
    }
}
