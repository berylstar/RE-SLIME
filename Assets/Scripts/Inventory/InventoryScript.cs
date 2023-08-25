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

    public List<int> listOverlap = new List<int>();
    private List<Vector3> equipGrid = new List<Vector3>();

    private bool isOpened = false;

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
        if (listOverlap.Count > 0)
            return;

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

        if (listOverlap.Count > 0)
            OpenInventory();
        else
            EquipEffect();
        
    }

    private void Update()
    {
        if (GameController.Pause(PauseType.INVEN))
            return;

        if (!isOpened)
        {
            isOpened = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (CheckOverlap())
                SoundManager.I.PlayEffect("EFFECT/Error");
            else
                CloseInventory();
        }
    }

    // 인벤토리 열고 닫는 함수
    public void OpenInventory()
    {
        cursor.SetActive(true);

        UIScript.I.stackAssists.Push("'I' : 인벤토리 열기/닫기, '스페이스' : 장비 선택");
        GameController.pause.Push(PauseType.INVEN);

        if (!GameController.tutorial[1])
        {
            BoardManager.I.kingslime.GetComponent<DialogueScript>().StartDialogue(DialogueType.InvenTutorial);
        }

        SoundManager.I.PlayEffect("EFFECT/InvenOpen");
    }

    private void CloseInventory()
    {
        cursor.SetActive(false);

        if (!CheckOverlap())
            EquipEffect();            

        SoundManager.I.PlayEffect("EFFECT/InvenOpen");

        StartCoroutine(CloseCo());
    }

    IEnumerator CloseCo()
    {
        yield return new WaitForEndOfFrame();
        isOpened = false;
        UIScript.I.stackAssists.Pop();
        GameController.pause.Pop();
    }

    // 인벤토리 좌표 초기화
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

        SoundManager.I.PlayEffect("EFFECT/EquipRegist");
    }

    // 인벤토리 겹침 체크
    public bool CheckOverlap()
    {
        objectOverlapped.SetActive(listOverlap.Count > 0);

        return listOverlap.Count > 0;
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
        ShuffleList(equipsNormal);
        ShuffleList(equipsRare);
        ShuffleList(equipsUnique);
    }

    private void ShuffleList (List<EquipScript> list)
    {
        int random1, random2;
        EquipScript temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }
    }
}
