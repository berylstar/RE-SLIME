using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DialogueType
{
    KINGSLIME,
    SIGN,
    InvenTutorial,
    DEMON,
}

public class DialogueScript : MonoBehaviour
{
    public DialogueType type;

    [Serializable]
    public struct DialogueStruct
    {
        public Sprite img;
        public string talker;
        [Multiline (3)]
        public string talk;
    }
    
    public List<DialogueStruct> dialogues = new List<DialogueStruct>();

    private int start, end;
    private int index = 0;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 10 - (int)transform.position.y;
    }

    private void Update()
    {
        if (GameController.Pause(PauseType.DIALOGUE))
            return;

        // ���� ��ȭ�ϴ°� ������ Ȯ��
        if (GameController.nowDialogue != this)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && !UIScript.I.onTexting)
        {
            index += 1;
            NextDialogue();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            index = end + 1;
            NextDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch") && tag == "NPC")
        {
            StartDialogue(type);
        }
    }

    // ó�� �κ��丮 ������ ���� ���� �ǰ� �ϱ� ����
    public void StartDialogue(DialogueType tp)
    {
        UIScript.I.stackAssists.Push("'C':��ȭ ��ŵ");
        GameController.pause.Push(PauseType.DIALOGUE);
        SetDialogue(tp);
        index = start;
        GameController.nowDialogue = this;
        UIScript.I.ShowDialogue(dialogues[start].img, dialogues[start].talker, dialogues[start].talk);
    }

    // ��ȭ ��뿡 ���� �̸� �����ؾ� ��
    private void SetDialogue(DialogueType tp)
    {
        // start : ��ȭ ���� element
        // end : ��ȭ ������ element + 1

        if (tp == DialogueType.SIGN)
        {
            if (GameController.floor == 0) { start = 0; end = 1; }
            else if (GameController.floor == 80) { start = 2; end = 3; }
            else { start = 1; end = 2; }
        }

        else if (tp == DialogueType.KINGSLIME)
        {
            if (!GameController.tutorial[0]) { start = 0; end = 23; }
            else if (!GameController.tutorial[1]) { start = 32; end = 33; }
            else { start = 23; end = 25; }
        }

        else if (tp == DialogueType.InvenTutorial) { start = 25; end = 32; }

        else if (tp == DialogueType.DEMON)
        {
            if (GameController.floor == 80) { start = 0; end = 6; }
            else if (GameController.floor == 100) { start = 6; end = 10; }
        }
    }

    IEnumerator CloseDialogue()
    {
        GameController.nowDialogue = null;
        UIScript.I.panelDialogue.SetActive(false);

        yield return GameController.delay_frame;
        
        UIScript.I.stackAssists.Pop();
        GameController.pause.Pop();
    }

    private void NextDialogue()
    {
        if (index >= end)
        {
            StartCoroutine(CloseDialogue());

            if (type == DialogueType.KINGSLIME)
            {
                // ���� Ʃ�丮�� ���� ��
                if (start == 0 && !GameController.tutorial[0])
                {
                    GameController.tutorial[0] = true;
                    GameController.coin += 5;
                }

                // �κ��丮 Ʃ�丮�� ���� ��
                else if (start == 25 && !GameController.tutorial[1])
                {
                    GameController.tutorial[1] = true;
                    StairScript.I.Open();
                }
            }
            else if (type == DialogueType.DEMON)
            {
                GetComponent<BossDemon>().enabled = true;
            }
        }
        else
            UIScript.I.ShowDialogue(dialogues[index].img, dialogues[index].talker, dialogues[index].talk);
    }
}
