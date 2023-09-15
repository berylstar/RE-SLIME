using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueScript : MonoBehaviour
{
    public DialogueData[] dataSO;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 10 - (int)transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch") && CompareTag("NPC"))
        {
            int idx = 0;

            switch (gameObject.name)
            {
                case "N_KingSlime":
                    if (!GameController.tutorial[0]) { idx = 0; }
                    else if (!GameController.tutorial[1]) { idx = 1; }
                    else { idx = 2; }
                    break;

                case "N_Sign":
                    if (GameController.floor == 0) { idx = 0; }
                    else if (GameController.floor == 80) { idx = 2; }
                    else { idx = 1; }
                    break;

                case "N_CoffinShop":
                    if (GameController.tutorial[0]) return;
                    break;

                case "M_Demon":
                    if (GameController.floor == 80) { idx = 0; }
                    else if (GameController.floor == 100) { idx = 1; }
                    break;

                default:
                    break;
            }

            UIScript.I.StartDialogue(dataSO[idx]);
        }
    }
}
