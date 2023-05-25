using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KingslimeScript : MonoBehaviour
{
    public List<Sprite> img = new List<Sprite>();
    public List<string> talker = new List<string>();
    public List<string> talk = new List<string>();

    private UIScript US;
    private int ii = 0;

    private void Start()
    {
        US = GameObject.Find("CONTROLLER").GetComponent<UIScript>();
    }

    private void Update()
    {
        if (!GameController.inDialogue)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ii += 1;

            if (ii >= img.Count)
            {
                US.EndDialogue();
                StartCoroutine(LittleTime());
                return;
            }

            US.Dialogue(img[ii], talker[ii], talk[ii]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch") && ii == 0)
        {
            US.Dialogue(img[0], talker[0], talk[0]);
        }
    }

    IEnumerator LittleTime()
    {
        yield return GameController.delay_01s;

        ii = 0;
    }
}
