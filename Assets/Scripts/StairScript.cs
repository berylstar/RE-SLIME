using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Close();

            if (GameController.floor > 100)
                SceneManager.LoadScene("ResultScene");
        }
    }

    public void Close()
    {
        bc2d.isTrigger = false;
        tag = "Wall";

        if (GameController.floor % 20 == 18)
        {
            sr.sprite = imgStairCloseLong;
            bc2d.size = new Vector2(2.4f, 0.5f);
        }
        else
        {
            sr.sprite = imgStairClose;
            bc2d.size = new Vector2(0.5f, 0.5f);
        }
            
    }

    public void Open()
    {
        bc2d.isTrigger = true;
        tag = "Stair";

        if (GameController.floor == 0 || GameController.floor % 20 == 18)
        {
            sr.sprite = imgStairOpenLong;
            bc2d.size = new Vector2(2.4f, 0.5f);
        }
        else
        {
            sr.sprite = imgStairOpen;
            bc2d.size = new Vector2(0.5f, 0.5f);
        }
    }
}