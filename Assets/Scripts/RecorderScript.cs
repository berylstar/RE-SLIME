using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderScript : MonoBehaviour
{
    public List<GameObject> picks = new List<GameObject>();

    private int ri = 0;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 10 - (int)transform.position.y;
        SetPick(1);
    }

    private void Update()
    {
        if (!GameController.inRecord || GameController.Pause(4))
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            Select(ri);

        if (Input.GetKeyDown(KeyCode.LeftArrow)) SetPick(0);
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow)) SetPick(1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) SetPick(2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch"))
        {
            UIScript.I.PanelRecorder.SetActive(true);
            GameController.inRecord = true;
        }
    }

    private void Select(int idx)
    {
        if (idx == 1)
        {

        }
        else if (idx == 2)
        {

        }
        else
        {
            StartCoroutine(CloseRecorder());
        }
    }

    private void SetPick(int idx)
    {
        foreach (GameObject pick in picks)
        {
            pick.SetActive(false);
        }

        ri = idx;
        picks[ri].SetActive(true);
    }

    IEnumerator CloseRecorder()
    {
        UIScript.I.PanelRecorder.SetActive(false);

        yield return GameController.delay_01s;

        GameController.inRecord = false;
    }
}
