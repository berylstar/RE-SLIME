using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript inst = null;

    [Header("EQUIPS")]
    public List<EquipScript> equipsNormal = new List<EquipScript>();
    public List<EquipScript> equipsRare = new List<EquipScript>();
    public List<EquipScript> equipsUnique = new List<EquipScript>();
    public List<EquipScript> GottenEquips = new List<EquipScript>();

    [Header("INVENTORY")]
    public List<int> invenChecker = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public GameObject objectOverlapped;
    public GameObject cursor;

    private List<Vector3> equipGrid = new List<Vector3>();

    private void Awake()
    {
        // �κ��丮�� ���� ����۵Ǿ ����
        if (inst == null) inst = this;
        else if (inst != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        InitialGrid();
    }

    private void Update()
    {
        // ��� ���������� �κ��丮�� ������ ����
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (GameController.inInven && CheckOverlap())
            {
                return;
            }

            OpenInventory();
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
    }

    // �κ��丮�� ��� �����ִ��� Ȯ��. ������=True
    public bool CheckOverlap()
    {
        for (int i = 0; i < invenChecker.Count; i++)
        {
            if (invenChecker[i] > 1)
            {
                objectOverlapped.SetActive(true);
                return true;
            }
        }

        objectOverlapped.SetActive(false);
        EquipEffect();
        return false;
    }

    // �κ��丮 ���� �ݴ� �Լ�
    public void OpenInventory()
    {
        GameController.inInven = !GameController.inInven;

        cursor.SetActive(GameController.inInven);

        if (!GameController.inInven)
            cursor.GetComponent<CursorScript>().CursorReset();
    }

    // ��� ȿ�� �ߵ�
    private void EquipEffect()
    {
        for (int i = 0; i < GottenEquips.Count; i++)
        {
            if (!GottenEquips[i].isEffected)
                GottenEquips[i].ApplyEffect();
        }
    }

    public void ShuffleEquipList()
    {
        ShuffleList<EquipScript>(equipsNormal);
        ShuffleList<EquipScript>(equipsRare);
        ShuffleList<EquipScript>(equipsUnique);
    }

    private List<T> ShuffleList<T> (List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }
}
