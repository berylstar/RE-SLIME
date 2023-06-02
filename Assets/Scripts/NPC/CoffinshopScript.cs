using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffinshopScript : MonoBehaviour
{
    public Text tName, tModifier, tEffect, tGrade, tPrice;
    public List<Image> stands = new List<Image>();

    public List<Sprite> equipImages = new List<Sprite>();

    private UIScript US;

    public int shopIndex = 0;

    private void Start()
    {
        US = GameObject.Find("CONTROLLER").GetComponent<UIScript>();
    }

    private void Update()
    {
        if (!GameController.inShop)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(CloseShop());
        }
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

    public void TEST()
    {
        for (int i = 0; i < 3; i++)
        {
            int iRand = Random.Range(0, equipImages.Count);
            stands[i].sprite = equipImages[iRand];
            stands[i].SetNativeSize();
        }
    }
}
