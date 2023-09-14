using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffinshopScript : MonoBehaviour
{
    [Header ("UI")]
    public Sprite imgSoldout;
    public Text textName, textAdject, textEffect, textGrade, textPrice;
    public List<Image> stands = new List<Image>();
    public List<GameObject> picks = new List<GameObject>();
    
    [Header("SHOP")]
    public List<EquipScript> onStands = new List<EquipScript>() { null, null, null };

    private int si = 0;

    private void Start()
    {
        PutEquipsOnStand();
    }

    //private void OnEnable()
    //{
    //    GetComponent<SpriteRenderer>().sortingOrder = 10 - (int)transform.position.y;
    //}

    private void Update()
    {
        if (GameController.situation.Peek() != SituationType.SHOP)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (si == 0)
                StartCoroutine(CloseShopCo());
            else
                BuyEquip(si);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(CloseShopCo());

        if (Input.GetKeyDown(KeyCode.I))
            InventoryScript.I.OpenInventory();

        // ���� Ű���� �Է�
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SetPick(Mathf.Min(3, Mathf.Max(0, si - 1)));
        if (Input.GetKeyDown(KeyCode.RightArrow)) SetPick(Mathf.Min(3, Mathf.Max(0, si + 1)));

        if (Input.GetKeyDown(KeyCode.DownArrow)) SetPick(0);
        if (Input.GetKeyDown(KeyCode.UpArrow) && si == 0) SetPick(1);

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GameController.coin >= 2)
            {
                GameController.coin -= 2;
                SoundManager.I.PlayEffect("EFFECT/ShopReroll");
                PutEquipsOnStand();
            }
            else
            {
                SoundManager.I.PlayEffect("EFFECT/Error");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch") && GameController.tutorial[0])
        {
            UIScript.I.panelShop.SetActive(true);
            UIScript.I.stackAssists.Push("'ESC' : ���� �ݱ�, 'R' : ��� ���ΰ�ħ (2����)");
            GameController.situation.Push(SituationType.SHOP);
            SoundManager.I.PlayEffect("EFFECT/ShopOpen");
        }
    }

    // ���� �ݱ� �ڷ�ƾ
    IEnumerator CloseShopCo()
    {
        if (!InventoryScript.I.CheckOverlap())
        {
            UIScript.I.panelShop.SetActive(false);
            SoundManager.I.PlayEffect("EFFECT/ShopClose");

            yield return GameController.delay_frame;

            UIScript.I.stackAssists.Pop();
            GameController.situation.Pop();
        }
        else
        {
            InventoryScript.I.OpenInventory();
        }        
    }

    // UI ȭ��ǥ ����
    private void SetPick(int v)
    {
        si = v;

        foreach (GameObject pick in picks)
        {
            pick.SetActive(false);
        }

        picks[si].SetActive(true);
        SoundManager.I.PlayEffect("EFFECT/UIMove");

        ShowEquipInfo(si);
    }

    // ���Ǵ� �̹��� ������Ʈ
    private void DisplayEquip(int idx, Sprite image)
    {
        stands[idx].sprite = image;
        stands[idx].SetNativeSize();
    }

    // ���Ǵ�� �ȿ� ���� �ؽ�Ʈ ������Ʈ
    private void ShowEquipInfo(int idx)
    {
        if (idx == 0 || onStands[idx - 1] == null)
        {
            textName.text = "�ذ������ �������";
            textAdject.text = "������ ��� ��� ��󺸼���";
            textEffect.text = "�����̽� : ����\n'R' : ���ΰ�ħ (2 ���)";   // ����� : Ŀ�� �̵�
            textGrade.text = "";
            textPrice.text = "���";
        }
        else
        {
            textName.text = "- " + onStands[idx-1].EName + " -";
            textAdject.text = onStands[idx-1].adject;
            textEffect.text = onStands[idx-1].effect;
            textGrade.text = onStands[idx-1].grade.ToString();
            textPrice.text = "x " + onStands[idx-1].price;
        }
    }

    // ��� ��޿� ���� ���Ǵ뿡 ��� ����
    private void PutEquipsOnStand()
    {
        InventoryScript.I.ShuffleEquipList();

        for (int i = 0; i < 3; i++)
        {
            int iGrade = Random.Range(0, 101);

            if (iGrade <= GameController.ShopGrade[0] && i <= InventoryScript.I.equipsRare.Count)
            {
                onStands[i] = InventoryScript.I.equipsRare[i];
            }
            else if (iGrade >= 100 - GameController.ShopGrade[1] && i <= InventoryScript.I.equipsUnique.Count)
            {
                onStands[i] = InventoryScript.I.equipsUnique[i];
            }
            else
            {
                onStands[i] = InventoryScript.I.equipsNormal[i];
            }

            DisplayEquip(i, onStands[i].ReturnSprite());
        }

        ShowEquipInfo(si);
    }

    // ���Ǵ뿡�� ��� ����
    private void BuyEquip(int i)
    {
        if (onStands[i-1] == null || onStands[i - 1].price > GameController.coin)
        {
            SoundManager.I.PlayEffect("EFFECT/Error");
            return;
        }

        GameController.coin -= onStands[i-1].price;
        onStands[i-1].GetThis();
        onStands[i-1] = null;

        DisplayEquip(i-1, imgSoldout);

        SoundManager.I.PlayEffect("EFFECT/ShopBuy");
        ShowEquipInfo(si);
    }
}
