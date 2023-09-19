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
    public Sprite imgStairFinalBoss;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();

        if (GameController.tutorial[1])
            Open();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameController.floor == 0)
                PlayerScript.I.StartTimeDamage();

            BoardManager.I.NextFloor();
            if (GameController.floor != 99)
                Close();
        }
    }

    public void Close()
    {
        bc2d.isTrigger = false;
        tag = "Wall";

        switch (GameController.floor)
        {
            case 18:
            case 38:
            case 58:
            case 78:
                sr.sprite = imgStairCloseLong;
                bc2d.size = new Vector2(2.4f, 0.5f);
                break;

            default:
                sr.sprite = imgStairClose;
                bc2d.size = new Vector2(0.5f, 0.5f);
                break;
        }

    }

    public void Open()
    {
        bc2d.isTrigger = true;
        tag = "Stair";

        switch (GameController.floor)
        {
            case 0:
            case 18:
            case 38:
            case 58:
            case 78:
                sr.sprite = imgStairOpenLong;
                bc2d.size = new Vector2(2.4f, 0.5f);
                break;

            case 99:
                sr.sprite = imgStairFinalBoss;
                bc2d.size = new Vector2(2.4f, 0.5f);
                break;

            default:
                sr.sprite = imgStairOpen;
                bc2d.size = new Vector2(0.5f, 0.5f);
                break;
        }
    }
}