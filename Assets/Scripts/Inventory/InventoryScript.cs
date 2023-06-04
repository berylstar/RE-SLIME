using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript inst = null;

    public List<EquipScript> equips = new List<EquipScript>();
    public List<int> invenChecker = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public GameObject objectOverlapped;

    public GameObject cursor;

    private List<Vector3> equipGrid = new List<Vector3>();

    private void Awake()
    {
        // 인벤토리는 씬이 재시작되어도 유지
        if (inst == null) inst = this;
        else if (inst != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

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
    }

    // 인벤토리에 장비가 겹쳐있는지 확인
    private bool CheckOverlap()
    {
        // 겹쳐있어서 열어야 될때
        if (!GameController.inInven)
            return true;

        for (int i = 0; i < invenChecker.Count; i++)
        {
            if (invenChecker[i] > 1)
            {
                objectOverlapped.SetActive(true);
                return false;
            }
        }

        objectOverlapped.SetActive(false);
        return true;
    }
}
