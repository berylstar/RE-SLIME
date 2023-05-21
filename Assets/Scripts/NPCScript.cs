using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType
{
    KINGSLIME,
    COFFINSHOP,
    TREASUREBOX,
}

public class NPCScript : MonoBehaviour
{
    public NPCType type;

    private UIScript US;
    private bool isActived = false;

    private void Start()
    {
        US = GameObject.Find("CONTROLLER").GetComponent<UIScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch"))
        {
            isActived = !isActived;
            InteractionNPC();
        }
    }

    private void InteractionNPC()
    {
        if (type == NPCType.KINGSLIME)
        {
            US.panelDialogue.SetActive(isActived);
        }
        else if (type == NPCType.COFFINSHOP)
        {
            US.panelShop.SetActive(isActived);
        }
    }
}
