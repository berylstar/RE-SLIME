using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public List<GameObject> equips = new List<GameObject>();

    public GameObject cursor;
    public GameObject skillC, skillV;

    private List<Vector3> equipGrid = new List<Vector3>();

    private void Awake()
    {
        InitialGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameController.inInven = !GameController.inInven;

            cursor.SetActive(GameController.inInven);
            cursor.GetComponent<CursorScript>().posIndex = 0;
            cursor.transform.position = equipGrid[0];
        }
    }

    private void InitialGrid()
    {
        equipGrid.Clear();

        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                equipGrid.Add(new Vector3(11.75f + 1.25f * y, 7 - 1.25f * x, 0f));
            }
        }
    }

    public Vector3 ReturnGrid(int index)
    {
        return equipGrid[index];
    }
}
