using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public List<GameObject> equips = new List<GameObject>();
    public List<int> equipOverlap = new List<int>();
    public GameObject textOverlapped;

    public GameObject cursor;

    private List<Vector3> equipGrid = new List<Vector3>();

    private void Awake()
    {
        InitialGrid();
    }

    private void Update()
    {
        // ��� ���������� �κ��丮�� ������ ����
        if (Input.GetKeyDown(KeyCode.I) && CheckOverlap())
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
            if (GameController.skillC && equip != GameController.skillC)
            {
                GameController.skillC.iconC.SetActive(false);
            }

            if (equip == GameController.skillV)
            {
                GameController.skillV = null;
                equip.iconV.SetActive(false);
            }

            GameController.skillC = equip;
            equip.iconC.SetActive(true);
        }
        else if (cv == "V")
        {
            if (GameController.skillV && equip != GameController.skillV)
            {
                GameController.skillV.iconV.SetActive(false);
            }

            if (equip == GameController.skillC)
            {
                GameController.skillC = null;
                equip.iconC.SetActive(false);
            }

            GameController.skillV = equip;
            equip.iconV.SetActive(true);
        }
        else
            return;

        print((GameController.skillC, GameController.skillV));
    }

    // �κ��丮�� ��� �����ִ��� Ȯ��
    private bool CheckOverlap()
    {
        for (int i = 0; i < equipOverlap.Count; i++)
        {
            if (equipOverlap[i] > 1)
            {
                textOverlapped.SetActive(true);
                return false;
            }
        }

        textOverlapped.SetActive(false);
        return true;
    }
}
