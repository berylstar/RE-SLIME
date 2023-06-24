using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DialogueType
{
    KINGSLIME,
    SIGN,
}

public class DialogueScript : MonoBehaviour
{
    public DialogueType type;

    [Serializable]
    public struct DialogueStruct
    {
        public Sprite img;
        public string talker;
        [Multiline (2)]
        public string talk;
    }
    
    public List<DialogueStruct> dialogues = new List<DialogueStruct>();

    private int start, end;
    private int index = 0;

    private void Update()
    {
        if (!GameController.inDiaglogue || GameController.Pause(0))
            return;

        // 지금 대화하는게 내가 아니라면 리턴
        if (GameController.nowDialogue != this)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            index += 1;

            if (index >= end)
            {
                StartCoroutine(CloseTutorial());

                // 최초 튜토리얼 진행 후
                if (type == DialogueType.KINGSLIME && !GameController.endTutorial)
                {
                    GameController.endTutorial = true;
                    GameController.coin += 5;
                }
            }
            else
                UIScript.I.ShowDialogue(dialogues[index].img, dialogues[index].talker, dialogues[index].talk);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch"))
        {
            SetDialogue();
            index = start;
            GameController.nowDialogue = this;
            UIScript.I.ShowDialogue(dialogues[start].img, dialogues[start].talker, dialogues[start].talk);
        }
    }

    // 대화 상대에 맞춰 미리 세팅해야 함
    private void SetDialogue()
    {
        if (type == DialogueType.SIGN) { start = 0; end = 1; }

        else if (type == DialogueType.KINGSLIME)
        {
            if (!GameController.endTutorial) { start = 0; end = 4; }
            else { start = 4; end = 6; }
        }
    }

    // 스페이스바 때문에 바로 NPC와 대화를 막기위한 잠깐의 딜레이
    IEnumerator CloseTutorial()
    {
        GameController.nowDialogue = null;

        yield return GameController.delay_01s;

        UIScript.I.EndDialogue();
    }
}
