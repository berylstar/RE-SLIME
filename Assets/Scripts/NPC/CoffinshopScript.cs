using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffinshopScript : MonoBehaviour
{
    public Text tName, tModifier, tEffect, tGrade, tPrice;
    public List<Image> stands = new List<Image>();
    public List<GameObject> picks = new List<GameObject>();

    private UIScript US;
    private InventoryScript INVEN;

    private int si = 0;

    private void Start()
    {
        US = GameObject.Find("CONTROLLER").GetComponent<UIScript>();
        INVEN = GameObject.Find("INVENTORY").GetComponent<InventoryScript>();
    }

    private void Update()
    {
        if (!GameController.inShop)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (si == 0)
                StartCoroutine(CloseShop());
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) MovePick(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MovePick(1);

        if (Input.GetKeyDown(KeyCode.DownArrow)) SetPick(0);
        if (Input.GetKeyDown(KeyCode.UpArrow) && si == 0) SetPick(1);
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

    public void TEST()
    {
        for (int i = 0; i < 3; i++)
        {
            int iRand = Random.Range(0, INVEN.equips.Count);
            stands[i].sprite = INVEN.equips[iRand].ReturnSprite();
            stands[i].SetNativeSize();
        }
    }

    public void TEST2()
    {
        int iRand = Random.Range(0, INVEN.equips.Count);

        if (INVEN.equips[iRand].gotten)
            return;

        INVEN.equips[iRand].GetThis();
    }
}
