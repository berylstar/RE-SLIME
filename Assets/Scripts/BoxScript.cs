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
    }

    private void Update()
    {
        if (!GameController.inBox || GameController.Pause(3))
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPicked)
                StartCoroutine(CloseBox());
            else if (bi != 0)
                GetReward(bi);
        }

        // 박스 커서 이동
        if (!isPicked)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) SetPick(1);
            if (Input.GetKeyDown(KeyCode.RightArrow)) SetPick(2);
        }

        ShowRewardInfo(bi);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && BoardManager.I.NoMonster())
        {
            UIScript.I.panelBox.SetActive(true);
            GameController.inBox = true;
        }
    }

    IEnumerator CloseBox()
    {
        UIScript.I.panelBox.SetActive(false);
        
        yield return GameController.delay_01s;

        GameController.inBox = false;
        SetReward();
        this.gameObject.SetActive(false);
    }

    private void SetReward()
    {
        stands[0].sprite = coinMunch;
        stands[1].sprite = potion;
        isPicked = false;
        SetPick(0);
        picks[bi].SetActive(false);
    }

    private void SetPick(int idx)
    {
        for (int i = 0; i < 3; ++i)
        {
            picks[i].SetActive(false);
        }

        bi = idx;
        picks[bi].SetActive(true);
    }

    private void ShowRewardInfo(int idx)
    {
        if (idx == 1)
        {
            textName.text = "코인 더미";
            textAdject.text = "이제 나도 부자 ?";
            textEffect.text = "COIN + 5";
        }
        else if (idx == 2)
        {
            textName.text = "대용량 포션";
            textAdject.text = "갈증 완벽 해결";
            textEffect.text = "HP + 50";
        }
        else if (!isPicked)
        {
            textName.text = "하나를 고르세요.";
            textAdject.text = "";
            textEffect.text = "왼쪽 or 오른쪽";
        }
        else
        {
            textName.text = "";
            textAdject.text = "";
            textEffect.text = "";
        }
    }

    private void GetReward(int idx)
    {
        if (idx == 1)
            GameController.coin += 5;

        else if (idx == 2)
            GameController.ChangeHP(50);

        stands[0].sprite = nope;
        stands[1].sprite = nope;

        SetPick(0);
        isPicked = true;

        GameController.bossCut = false;
    }
}
