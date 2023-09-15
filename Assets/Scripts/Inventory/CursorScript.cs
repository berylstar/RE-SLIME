using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorScript : MonoBehaviour
{
    private GameObject pick = null;
    private GameObject on = null;
    private GameObject check = null;

    private SpriteRenderer sr;
    private int posIndex = 0;
    
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        CursorReset();
    }

    private void OnDisable()
    {
        CursorReset();

        if (UIScript.I.panelInvenInfo.activeInHierarchy)
        {
            UIScript.I.panelInvenInfo.SetActive(false);
            UIScript.I.stackAssists.Pop();
        }
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

    private void OnMove(InputValue value)
    {
        if (GameController.situation.Peek() != SituationType.INVENTORY)
            return;

        Vector2 dir = value.Get<Vector2>();
        int change;

        if (dir == Vector2.left && posIndex % 3 > 0) { change = -1; }
        else if (dir == Vector2.right && posIndex % 3 < 2) { change = 1; }
        else if (dir == Vector2.up && posIndex > 2) { change = -3; }
        else if (dir == Vector2.down && posIndex < 15) { change = 3; }
        else { return; }

        if (pick && !pick.GetComponent<EquipScript>().EquipMove(change))
            return;

        posIndex += change;

        transform.position = InventoryScript.I.ReturnGrid(posIndex);
        SoundManager.I.PlayEffect("EFFECT/UIMove");
    }

    private void OnPick()
    {
        if (GameController.situation.Peek() != SituationType.INVENTORY)
            return;

        if (on == null)
            return;

        if (!pick)
        {
            pick = on;
            sr.color = Color.yellow;
            ShowInfo();
        }
        else
        {
            pick = null;
            check = null;
            sr.color = Color.white;
            UIScript.I.panelInvenInfo.SetActive(false);
            UIScript.I.stackAssists.Pop();
        }
        SoundManager.I.PlayEffect("EFFECT/InvenClick");
    }

    private void OnSetSkill(InputValue value)
    {
        if (GameController.situation.Peek() != SituationType.INVENTORY)
            return;

        if (pick == null)
            return;

        switch (value.Get<float>())
        {
            case 1:
                InventoryScript.I.SetSkill("C", pick.GetComponent<EquipScript>());
                break;

            case -1:
                InventoryScript.I.SetSkill("V", pick.GetComponent<EquipScript>());
                break;
        }

        pick = null;
        sr.color = Color.white;
    }

    private void OnRemove()
    {
        if (GameController.situation.Peek() != SituationType.INVENTORY)
            return;

        if (pick == null)
            return;

        if (check != pick)
        {
            sr.color = Color.red;
            check = pick;
            return;
        }

        pick.GetComponent<EquipScript>().RemoveThis();
        pick = null;
        check = null;
        sr.color = Color.white;
        UIScript.I.panelInvenInfo.SetActive(false);
        UIScript.I.stackAssists.Pop();
    }

    // 인벤토리 열었을 때 커서 초기화
    public void CursorReset()
    {
        posIndex = 0;
        transform.position = InventoryScript.I.ReturnGrid(0);
        pick = null;
        check = null;
        sr.color = Color.white;
    }

    // 선택한 장비 정보 보여주기
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

        UIScript.I.stackAssists.Push("[C][V] 스킬 등록, [R] 장비 제거");
    }
}
