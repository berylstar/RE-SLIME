using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinshopScript : MonoBehaviour
{
    private UIScript US;

    private void Start()
    {
        US = GameObject.Find("CONTROLLER").GetComponent<UIScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch"))
        {
            US.panelShop.SetActive(true);
        }
    }

    IEnumerator LittleTime()
    {
        yield return GameController.delay_01s;
    }
}
