using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipScript : MonoBehaviour
{
    public List<int> posIndex = new List<int>();

    private InventoryScript IS;
    private Transform tf;

    private void Start()
    {
        tf = GetComponent<Transform>();
        IS = GameObject.Find("INVENTORY").GetComponent<InventoryScript>();

        tf.position = IS.ReturnGrid(posIndex[0]);
    }

    public void Move(int i)
    {
        posIndex[0] += i;

        tf.position = IS.ReturnGrid(posIndex[0]);
    }
}
