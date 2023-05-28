using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public List<GameObject> equips = new List<GameObject>();

    public GameObject cursor;
    public GameObject iconC, iconV;

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

            if (!GameController.inInven)
                cursor.GetComponent<CursorScript>().CursorReset();
        }
    }

    // �κ��丮 ��ǥ ����
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

    // �κ��丮 ��ǥ�� �ε����� ����. �»���� 0
    public Vector3 ReturnGrid(int index)
    {
        return equipGrid[index];
    }

    // �κ��丮���� ��� ��ų�� ���
    public void SetSkill(string cv, EquipScript equip)
    {
        if (cv == "C")
        {
            if (equip == GameController.skillV)
            {
                GameController.skillV = null;
                iconV.SetActive(false);
            }

            GameController.skillC = equip;
            iconC.SetActive(true);
            iconC.transform.position = ReturnGrid(equip.posIndex[0]);
        }
        else if (cv == "V")
        {
            if (equip == GameController.skillC)
            {
                GameController.skillC = null;
                iconC.SetActive(false);
            }

            GameController.skillV = equip;
            iconV.SetActive(true);
            iconV.transform.position = ReturnGrid(equip.posIndex[0]);
        }
        else
            return;

        print((GameController.skillC, GameController.skillV));
    }
}
