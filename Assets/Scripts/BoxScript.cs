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

    private UIScript US;

    private bool isPicked = false;
    private int bi = 0;

    private void Start()
    {
        US = GameObject.Find("CONTROLLER").GetComponent<UIScript>();
    }

    private void Update()
    {
        if (!GameController.inBox || GameController.Pause(3))
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bi == 0)
                StartCoroutine(CloseBox());
            else
                GetEquip(bi);
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
        if (collision.CompareTag("Player"))
        {
            US.panelBox.SetActive(true);
            GameController.inBox = true;
        }
    }

    IEnumerator CloseBox()
    {
        US.panelBox.SetActive(false);
        
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
            textAdject.text = "";
            textEffect.text = "COIN + 5";
        }
        else if (idx == 2)
        {
            textName.text = "대용량 포션";
            textAdject.text = "";
            textEffect.text = "HP + 30";
        }
        else
        {
            textName.text = "CHOOSE 1";
            textAdject.text = "";
            textEffect.text = "";
        }
    }

    private void GetEquip(int idx)
    {
        if (idx == 1)
        {
            GameController.coin += 5;
        }
        else if (idx == 2)
        {
            GameController.ChangeHP(50);
        }

        stands[0].sprite = nope;
        stands[1].sprite = nope;

        SetPick(0);
        isPicked = true;
        
    }
}
