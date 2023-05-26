using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public InventoryScript IS;
    public int cursorIndex = 0;

    private Transform tf;
    private SpriteRenderer sr;
    private PolygonCollider2D pc;

    private GameObject pick = null;
    private GameObject on = null;

    private void Start()
    {
        tf = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!pick) pick = on;
            else pick = null;
        }

        sr.color = pick ? new Color32(255, 255, 0, 255) : new Color32(255, 255, 255, 255);

        MoveCursor();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Equip"))
        {
            on = collision.gameObject;
        }
        else
        {
            on = null;
        }
    }


    private void MoveCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && cursorIndex % 3 > 0) cursorIndex -= 1;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && cursorIndex % 3 < 2) cursorIndex += 1;
        else if (Input.GetKeyDown(KeyCode.UpArrow) && cursorIndex > 2) cursorIndex -= 3;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && cursorIndex < 15) cursorIndex += 3;
        else return;

        tf.position = IS.ReturnGrid(cursorIndex);

        if (pick)
        {
            pick.transform.position = IS.ReturnGrid(cursorIndex);
        }
    }
}
