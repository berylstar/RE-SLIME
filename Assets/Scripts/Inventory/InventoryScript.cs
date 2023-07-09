using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript I = null;

    [Header("EQUIPS")]
    public List<EquipScript> all = new List<EquipScript>();
    public List<EquipScript> equipsNormal = new List<EquipScript>();
    public List<EquipScript> equipsRare = new List<EquipScript>();
    public List<EquipScript> equipsUnique = new List<EquipScript>();
    public List<EquipScript> GottenEquips = new List<EquipScript>();

    [Header("INVENTORY")]
    public GameObject objectOverlapped;
    public GameObject cursor;

    [SerializeField] private List<int> invenChecker = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private List<Vector3> equipGrid = new List<Vector3>();
    [HideInInspector] public bool isLoaded = false;

    private void Awake()
    {
        // 싱글톤
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        InitialGrid();
    }

    private void Start()
    {
        LOAD();
    }

    public void SAVE()
    {
        for (int i = 0; i < GottenEquips.Count; i++)
        {
            string strr = GottenEquips[i].number.ToString();

            for (int j = 0; j < GottenEquips[i].posIndex.Count; j++)
            {
                strr += "," + GottenEquips[i].posIndex[j].ToString();
            }

            GameController.myEquips.Add(strr);
        }
    }

    private void LOAD()
    {
        for (int i = 0; i < GameController.myEquips.Count; i++)
        {
            string[] ints = GameController.myEquips[i].Split(',');

            List<int> poses = new List<int>();

            for (int j = 1; j < ints.Length; j++)
            {
                poses.Add(int.Parse(ints[j]));
            }

            all[int.Parse(ints[0])-1].LoadThis(poses);
        }

        if (CheckOverlap())
            OpenInventory();

        isLoaded = true;
    }

    private void Update()
    {
        // 장비가 겹쳐있으면 인벤토리가 닫히지 않음
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!GameController.tutorial[0] || GameController.Pause(1))
                return;

            if (GameController.inInven && CheckOverlap())
            {
                return;
            }

            OpenInventory();
        }
    }

    // 인벤토리 열고 닫는 함수
    public void OpenInventory()
    {
        GameController.inInven = !GameController.inInven;

        cursor.SetActive(GameController.inInven);

        if (GameController.inInven)
            UIScript.I.texttext.text = "'I' : 인벤토리 열기/닫기, '스페이스' : 장비 선택";
        else
            UIScript.I.texttext.text = "";

        if (!GameController.tutorial[1])
        {
            BoardManager.I.kingslime.GetComponent<DialogueScript>().StartDialogue(DialogueType.InvenTutorial);
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
        if (equip.type == EquipType.PASSIVE)
            return;

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

    // 장비 겹침을 확인하는 InvenChecker 업데이트 함수 => EquipScript에서 실행
    public void UpdateInvenChecker(int idx, int val)
    {
        invenChecker[idx] += val;
    }

    // 인벤토리에 장비가 겹쳐있는지 확인. 오버랩=True
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

    // 장비 획득 후 겹쳐있지 않다면 장비 효과 발동
    private void EquipEffect()
    {
        for (int i = 0; i < GottenEquips.Count; i++)
        {
            if (!GottenEquips[i].isEffected)
                GottenEquips[i].ApplyEffect();
        }
    }

    // 상점 판매를 위해 장비 리스트 셔플 => CoffinShopScript에서 사용
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
