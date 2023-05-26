using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipScript : MonoBehaviour
{
    private InventoryScript IS;
    private Transform tf;

    private void Start()
    {
        tf = GetComponent<Transform>();
        IS = GameObject.Find("INVENTORY").GetComponent<InventoryScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Cursor")
            return;
    }
}
