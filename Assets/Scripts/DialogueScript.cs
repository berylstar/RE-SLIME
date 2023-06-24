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

        // ���� ��ȭ�ϴ°� ���� �ƴ϶�� ����
        if (GameController.nowDialogue != this)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            index += 1;

            if (index >= end)
            {
                StartCoroutine(CloseTutorial());

                // ���� Ʃ�丮�� ���� ��
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

    // ��ȭ ��뿡 ���� �̸� �����ؾ� ��
    private void SetDialogue()
    {
        if (type == DialogueType.SIGN) { start = 0; end = 1; }

        else if (type == DialogueType.KINGSLIME)
        {
            if (!GameController.endTutorial) { start = 0; end = 4; }
            else { start = 4; end = 6; }
        }
    }

    // �����̽��� ������ �ٷ� NPC�� ��ȭ�� �������� ����� ������
    IEnumerator CloseTutorial()
    {
        GameController.nowDialogue = null;

        yield return GameController.delay_01s;

        UIScript.I.EndDialogue();
    }
}
