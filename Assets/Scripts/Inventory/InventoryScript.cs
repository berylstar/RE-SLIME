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

    private void Awake()
    {
        // �̱���
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

        CheckAndEffect();
    }

    private void Update()
    {
        // ��� ���������� �κ��丮�� ������ ����
        if (GameController.inInven && CheckOverlap())
            return;

        if (Input.GetKeyDown(KeyCode.I) || (GameController.inInven && Input.GetKeyDown(KeyCode.Escape)))
        {
            if (!GameController.tutorial[0] || GameController.Pause(1))
                return;

            EquipEffect();
            OpenInventory();
        }
    }

    // �κ��丮 ���� �ݴ� �Լ�
    public void OpenInventory()
    {
        GameController.inInven = !GameController.inInven;

        cursor.SetActive(GameController.inInven);

        if (GameController.inInven)
            UIScript.I.listAssists.Add("'I' : �κ��丮 ����/�ݱ�, '�����̽�' : ��� ����");
        else
            UIScript.I.listAssists.RemoveAt(UIScript.I.listAssists.Count - 1);

        if (!GameController.tutorial[1])
        {
            BoardManager.I.kingslime.GetComponent<DialogueScript>().StartDialogue(DialogueType.InvenTutorial);
        }

        SoundManager.I.PlayEffect("EFFECT/InvenOpen");
    }

    // �κ��丮 ��ǥ �ʱ�ȭ
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

    private bool CheckOverlap()
    {
        objectOverlapped.SetActive((listOverlap.Count > 0));

        if (Input.GetKeyDown(KeyCode.I))
            SoundManager.I.PlayEffect("EFFECT/Error");

        return (listOverlap.Count > 0);
    }

    // ��� ȹ�� �� �������� �ʴٸ� ��� ȿ�� �ߵ�
    private void EquipEffect()
    {
        for (int i = 0; i < GottenEquips.Count; i++)
        {
            if (!GottenEquips[i].isEffected)
                GottenEquips[i].ApplyEffect();
        }
    }

    public void CheckAndEffect()
    {
        if (!CheckOverlap())
            EquipEffect();
        else
            OpenInventory();
    }

    // ���� �ǸŸ� ���� ��� ����Ʈ ���� => CoffinShopScript���� ���
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
