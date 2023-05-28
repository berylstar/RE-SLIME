using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public InventoryScript IS;

    private Transform tf;
    private SpriteRenderer sr;

    private int posIndex = 0;
    private GameObject pick = null;
    private GameObject on = null;

    private void Start()
    {
        tf = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();

        CursorReset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!pick) pick = on;
            else pick = null;
        }

        if (Input.GetKeyDown(KeyCode.C) && pick)
        {
            IS.SetSkill("C", pick.GetComponent<EquipScript>());
            pick = null;
        }

        if (Input.GetKeyDown(KeyCode.V) && pick)
        {
            IS.SetSkill("V", pick.GetComponent<EquipScript>());
            pick = null;
        }


        sr.color = pick ? new Color32(255, 255, 0, 255) : new Color32(255, 255, 255, 255);

        MoveCursor();
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    on = null;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Equip"))
            on = collision.gameObject;
    }

    private void MoveCursor()
    {
        int change = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && posIndex % 3 > 0) change = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && posIndex % 3 < 2) change = 1;
        else if (Input.GetKeyDown(KeyCode.UpArrow) && posIndex > 2) change = -3;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && posIndex < 15) change = 3;
        else return;

        posIndex += change;

        tf.position = IS.ReturnGrid(posIndex);

        if (pick)
        {
            pick.GetComponent<EquipScript>().Move(change);
        }
    }

    public void CursorReset()
    {
        posIndex = 0;
        tf.position = IS.ReturnGrid(0);
        pick = null;
    }
}
