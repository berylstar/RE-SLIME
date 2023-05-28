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
        // 장비가 겹쳐있으면 인벤토리가 닫히지 않음
        if (Input.GetKeyDown(KeyCode.I) && CheckOverlap())
        {
            GameController.inInven = !GameController.inInven;

            cursor.SetActive(GameController.inInven);

            if (!GameController.inInven)
                cursor.GetComponent<CursorScript>().CursorReset();
        }
    }

    // 인벤토리 좌표 설정
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

    // 인벤토리 좌표를 인덱스로 접근. 좌상단이 0
    public Vector3 ReturnGrid(int index)
    {
        return equipGrid[index];
    }

    // 인벤토리에서 장비를 스킬로 등록
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

    // 인벤토리에 장비가 겹쳐있는지 확인
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
