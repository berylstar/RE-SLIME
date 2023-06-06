using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KingslimeScript : MonoBehaviour
{
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            index += 1;

            if (index >= img.Count)
            {
                StartCoroutine(CloseTutorial());
                GameController.coin += 5;       // 테스트용
            }
            else
                US.Dialogue(img[index], talker[index], talk[index]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch") && index == 0)
        {
            US.Dialogue(img[0], talker[0], talk[0]);
        }
    }

    // 바로 다시 NPC와 대화를 막기위한 잠깐의 딜레이
    IEnumerator CloseTutorial()
    {
        US.EndDialogue();

        yield return GameController.delay_01s;

        index = 0;
    }
}
