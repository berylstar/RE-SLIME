using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairScript : MonoBehaviour
{
    public static StairScript I = null;

    private SpriteRenderer sr;
    private BoxCollider2D bc2d;

    public Sprite imgStairClose;
    public Sprite imgStairOpen;
    public Sprite imgStairCloseLong;
    public Sprite imgStairOpenLong;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameController.floor == 0)
                PlayerScript.I.StartTimeDamage();

            BoardManager.I.NextFloor();
            Close();
        }
    }

    public void Close()
    {
        bc2d.isTrigger = false;
        tag = "Wall";

        if (GameController.floor % 20 == 18)
            sr.sprite = imgStairCloseLong;
        else
            sr.sprite = imgStairClose;
    }

    public void Open()
    {
        bc2d.isTrigger = true;
        tag = "Stair";

        if (GameController.floor == 0 || GameController.floor % 20 == 18)
        {
            bc2d.size = new Vector2 (2.4f, 0.5f);
            sr.sprite = imgStairOpenLong;
        }
        else
        {
            bc2d.size = new Vector2 (0.5f, 0.5f);
            sr.sprite = imgStairOpen;
        }
    }
}