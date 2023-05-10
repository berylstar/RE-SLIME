using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairScript : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D bc2d;

    public Sprite imgStairClose;
    public Sprite imgStairOpen;
    public Sprite imgStairCloseLong;
    public Sprite imgStairOpenLong;

    public BoardManager BM;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BM.NextFloor();
            StairClose();
        }
    }

    private void StairClose()
    {
        bc2d.isTrigger = false;

        if (GameController.floor % 10 == 0)
            sr.sprite = imgStairCloseLong;
        else
            sr.sprite = imgStairClose;
    }

    public void StairOpen()
    {
        bc2d.isTrigger = true;

        if (GameController.floor % 10 == 0)
        {
            bc2d.size = new Vector2 (2.4f, 0.5f);
            sr.sprite = imgStairOpenLong;
        }
        else
        {
            bc2d.size = new Vector2(0.5f, 0.5f);
            sr.sprite = imgStairOpen;
        }
    }
}