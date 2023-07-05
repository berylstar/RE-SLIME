using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    private SpriteRenderer sr;

    private int posIndex = 0;
    public GameObject pick = null;
    public GameObject on = null;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        CursorReset();
    }

    private void OnDisable()
    {
        CursorReset();
    }

    private void Update()
    {
        if (GameController.Pause(1))
            return;

        // Input : �����̽� �� = ��� ����/����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!pick)
            {
                pick = on;
                ShowInfo();
            }
            else
            {
                pick = null;
                UIScript.I.panelInvenInfo.SetActive(false);
                UIScript.I.textKey.gameObject.SetActive(false);
            }
        }

        // Input : C = ��� C ��ų ���
        if (Input.GetKeyDown(KeyCode.C) && pick)
        {
            InventoryScript.I.SetSkill("C", pick.GetComponent<EquipScript>());
            pick = null;
        }

        // Input : V = ��� V ��ų ���
        if (Input.GetKeyDown(KeyCode.V) && pick)
        {
            InventoryScript.I.SetSkill("V", pick.GetComponent<EquipScript>());
            pick = null;
        }

        // Input : R = ��� ����
        if (Input.GetKeyDown(KeyCode.R) && pick)
        {
            pick.GetComponent<EquipScript>().RemoveThis();
            pick = null;
        }

        sr.color = pick ? new Color32(255, 255, 0, 255) : new Color32(255, 255, 255, 255);

        // Input : ����Ű = �κ��丮 �� Ŀ�� ����
        MoveCursor();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        on = null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Equip"))
            on = collision.gameObject;
    }

    // Ŀ�� �̵� �Լ�
    private void MoveCursor()
    {
        int change;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && posIndex % 3 > 0) change = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && posIndex % 3 < 2) change = 1;
        else if (Input.GetKeyDown(KeyCode.UpArrow) && posIndex > 2) change = -3;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && posIndex < 15) change = 3;
        else return;
        
        if (pick && !pick.GetComponent<EquipScript>().EquipMove(change))
            return;

        posIndex += change;

        transform.position = InventoryScript.I.ReturnGrid(posIndex);
    }

    // �κ��丮 ������ �� Ŀ�� �ʱ�ȭ
    public void CursorReset()
    {
        posIndex = 0;
        transform.position = InventoryScript.I.ReturnGrid(0);
        pick = null;
    }

    private void ShowInfo()
    {
        if (pick == null)
            return;

        UIScript.I.panelInvenInfo.SetActive(true);

        EquipScript pickEquip = pick.GetComponent<EquipScript>();
        UIScript.I.textName.text = pickEquip.EName;
        UIScript.I.textGrade.text = pickEquip.grade.ToString();
        UIScript.I.textPrice.text = "x " + pickEquip.price;
        UIScript.I.textEffect.text = pickEquip.effect;

        UIScript.I.textKey.text = "'C'/'V' : ��ų ���, 'R' : ��� ����";
        UIScript.I.textKey.gameObject.SetActive(true);
    }
}
