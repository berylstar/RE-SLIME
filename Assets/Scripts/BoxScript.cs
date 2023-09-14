using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxScript : MonoBehaviour
{
    public Sprite coinMunch, potion, nope;
    public Text textName, textAdject, textEffect;
    public List<Image> stands = new List<Image>();
    public List<GameObject> picks = new List<GameObject>();

    private bool isPicked = false;
    private int bi = 0;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 10 - (int)transform.position.y;

        stands[0].sprite = coinMunch;
        stands[1].sprite = potion;
        isPicked = false;
        bi = 0;
        foreach (GameObject pick in picks)
        {
            pick.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameController.situation.Peek() != SituationType.BOX)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPicked)
                StartCoroutine(CloseBox());
            else if (bi != 0)
                GetReward();
        }

        // �ڽ� Ŀ�� �̵�
        if (!isPicked)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) SetPick(1);
            if (Input.GetKeyDown(KeyCode.RightArrow)) SetPick(2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIScript.I.panelBox.SetActive(true);
            GameController.situation.Push(SituationType.BOX);
            SoundManager.I.PlayEffect("EFFECT/BoxOpen");
        }
    }

    IEnumerator CloseBox()
    {
        UIScript.I.panelBox.SetActive(false);
        SoundManager.I.PlayEffect("EFFECT/BoxClose");

        yield return GameController.delay_frame;

        GameController.situation.Pop();
        this.gameObject.SetActive(false);
    }

    private void SetPick(int idx)
    {
        foreach (GameObject pick in picks)
        {
            pick.SetActive(false);
        }

        bi = idx;
        picks[bi].SetActive(true);
        ShowRewardInfo();
    }

    private void ShowRewardInfo()
    {
        if (bi == 1)
        {
            textName.text = "���� ����";
            textAdject.text = "���� ���� ���� ?";
            textEffect.text = "COIN + 10";
        }
        else if (bi == 2)
        {
            textName.text = "��뷮 ����";
            textAdject.text = "���� �Ϻ� �ذ�";
            textEffect.text = "HP + 50";
        }
        else if (!isPicked)
        {
            textName.text = "�ϳ��� ������.";
            textAdject.text = "";
            textEffect.text = "���� or ������";
        }
        else
        {
            textName.text = "";
            textAdject.text = "";
            textEffect.text = "";
        }
    }

    private void GetReward()
    {
        if (bi == 1)
            GameController.coin += 10;
        else if (bi == 2)
            GameController.ChangeHP(50);

        stands[0].sprite = nope;
        stands[1].sprite = nope;

        SetPick(0);
        isPicked = true;
    }
}
