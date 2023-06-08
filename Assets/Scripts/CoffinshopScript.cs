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

    private UIScript US;
    private InventoryScript INVEN;

    private int si = 0;

    private void Start()
    {
        US = GameObject.Find("CONTROLLER").GetComponent<UIScript>();
        INVEN = GameObject.Find("INVENTORY").GetComponent<InventoryScript>();

        PutEquipsOnStand();
    }

    private void Update()
    {
        if (!GameController.inShop || GameController.Pause(2))
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (si == 0)
            {
                StartCoroutine(CloseShop());

                if (INVEN.CheckOverlap())
                    INVEN.OpenInventory();
            }
            else
                BuyEquip(si);
        }
        
        // 상점 커서 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MovePick(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MovePick(1);

        if (Input.GetKeyDown(KeyCode.DownArrow)) SetPick(0);
        if (Input.GetKeyDown(KeyCode.UpArrow) && si == 0) SetPick(1);

        ShowEquipInfo(si);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch") && !GameController.inShop)
        {
            US.panelShop.SetActive(true);
            GameController.inShop = true;
        }
    }

    IEnumerator CloseShop()
    {
        US.panelShop.SetActive(false);
        yield return GameController.delay_01s;
        GameController.inShop = false;
    }

    private void MovePick(int v)
    {
        si += v;

        si = Mathf.Min(3, Mathf.Max(0, si));

        for (int i = 0; i < 4; i++)
        {
            picks[i].SetActive(false);
        }

        picks[si].SetActive(true);
    }

    private void SetPick(int v)
    {
        si = v;

        for (int i = 0; i < 4; i++)
        {
            picks[i].SetActive(false);
        }

        picks[si].SetActive(true);
    }

    // 가판대 이미지 업데이트
    private void DisplayEquip(int idx, Sprite image)
    {
        stands[idx].sprite = image;
        stands[idx].SetNativeSize();
    }

    // 가판대와 픽에 따라 텍스트 업데이트
    private void ShowEquipInfo(int idx)
    {
        if (idx == 0 || onStands[idx - 1] == null)
        {
            textName.text = "Coffin Shop";
            textAdject.text = "Welcome !";
            textEffect.text = "";
            textGrade.text = "-----";
            textPrice.text = "ONLY";
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

    // 가판대에 장비 세팅
    private void PutEquipsOnStand()
    {
        if (INVEN.countOfEquips[0] < 3)
        {
            print("ERROR");
            return;
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    int iRand = Random.Range(0, INVEN.countOfEquips[0]);
        //    onStands[i] = INVEN.equips[iRand];

        //    DisplayEquip(i, onStands[i].ReturnSprite());
        //}

        // 장비 등급에 따른 가판대 세팅
        for (int i = 0; i < 3; i++)
        {
            int iGrade = Random.Range(1, 101);

            if (iGrade <= GameController.ShopGrade[1])
            {
                onStands[i] = INVEN.equipsRare[Random.Range(0, INVEN.countOfEquips[2])];
                DisplayEquip(i, onStands[i].ReturnSprite());
            }
            else if (iGrade >= 100 - GameController.ShopGrade[2])
            {
                onStands[i] = INVEN.equipsUnique[Random.Range(0, INVEN.countOfEquips[3])];
                DisplayEquip(i, onStands[i].ReturnSprite());
            }
            else
            {
                onStands[i] = INVEN.equipsNormal[Random.Range(0, INVEN.countOfEquips[1])];
                DisplayEquip(i, onStands[i].ReturnSprite());
            }
        }
    }

    // 가판대에서 장비 구매
    private void BuyEquip(int i)
    {
        if (onStands[i-1] == null || onStands[i-1].gotten || onStands[i - 1].price > GameController.coin)
            return;

        GameController.coin -= onStands[i-1].price;
        onStands[i-1].GetThis();
        onStands[i-1] = null;

        DisplayEquip(i-1, imgSoldout);
    }

    public void TEST()
    {
        PutEquipsOnStand();
    }
}
