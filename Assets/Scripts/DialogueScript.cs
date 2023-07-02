using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DialogueType
{
    KINGSLIME,
    SIGN,
    InvenTutorial,
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
        if (!GameController.inDiaglogue || GameController.Pause(0))
            return;

        // ���� ��ȭ�ϴ°� ���� �ƴ϶�� ����
        if (GameController.nowDialogue != this)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && !UIScript.I.onTexting)
        {
            index += 1;

            if (index >= end)
            {
                StartCoroutine(CloseTutorial());

                // ���� Ʃ�丮�� ���� ��
                if (start == 0 && !GameController.tutorial[0])
                {
                    GameController.tutorial[0] = true;
                    GameController.coin += 5;
                }

                // �κ��丮 Ʃ�丮�� ���� ��
                if (start == 25 && !GameController.tutorial[1])
                {
                    GameController.tutorial[1] = true;
                    StairScript.I.Open();
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
            StartDialogue(type);
        }
    }

    // ó�� �κ��丮 ������ ���� ���� �ǰ� �ϱ� ����
    public void StartDialogue(DialogueType tp)
    {
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

        if (tp == DialogueType.SIGN) { start = 0; end = 1; }

        else if (tp == DialogueType.KINGSLIME)
        {
            if (!GameController.tutorial[0]) { start = 0; end = 23; }
            else { start = 23; end = 25; }
        }

        else if (tp == DialogueType.InvenTutorial) { start = 25; end = 30; }
    }

    // �����̽��� ������ �ٷ� NPC�� ��ȭ�� �������� ����� ������
    IEnumerator CloseTutorial()
    {
        GameController.nowDialogue = null;

        yield return GameController.delay_01s;

        UIScript.I.EndDialogue();
    }
}
