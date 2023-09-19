using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript I = null;

    public DialogueData invenTutorial;

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
        if (listOverlap.Count > 0)
            return;

        foreach (EquipScript equip in GottenEquips)
        {
            //string strr = equip.number.ToString();

            //foreach (int idx in equip.posIndex)
            //    strr += "," + idx.ToString();

            //GameController.myEquips.Add(strr);

            StringBuilder strr = new StringBuilder(equip.number.ToString());

            foreach (int idx in equip.posIndex)
                strr.Append($",{idx}");

            GameController.myEquips.Add(strr.ToString());
        }
    }

    private void LOAD()
    {
        foreach (string stringEquip in GameController.myEquips)
        {
            string[] ints = stringEquip.Split(',');
            List<int> poses = new List<int>();

            for (int j = 1; j < ints.Length; j++)
            {
                poses.Add(int.Parse(ints[j]));
            }

            all[int.Parse(ints[0]) - 1].LoadThis(poses);
        }

        if (listOverlap.Count > 0)
            OpenInventory();
        else
            EquipEffect();
    }

    private void Update()
    {
        if (GameController.situation.Peek() != SituationType.INVENTORY)
            return;

        objectOverlapped.SetActive(listOverlap.Count > 0);
    }

    public void OpenInventory()
    {
        cursor.SetActive(true);
        UIScript.I.panelForInven.SetActive(true);

        UIScript.I.stackAssists.Push("[I] �κ��丮 ����/�ݱ�, [SPACE] ��� ����");
        GameController.situation.Push(SituationType.INVENTORY);

        if (!GameController.tutorial[1])
        {
            UIScript.I.StartDialogue(invenTutorial);
        }

        SoundManager.I.PlayEffect("EFFECT/InvenOpen");
    }

    public void CloseInventory()
    {
        if (CheckOverlap())
        {
            SoundManager.I.PlayEffect("EFFECT/Error");
        }
        else
        {
            cursor.SetActive(false);
            UIScript.I.panelForInven.SetActive(false);

            SoundManager.I.PlayEffect("EFFECT/InvenOpen");

            StartCoroutine(CloseCo());
        }        
    }

    IEnumerator CloseCo()
    {
        yield return GameController.delay_frame;
        UIScript.I.stackAssists.Pop();
        GameController.situation.Pop();
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

        switch (cv)
        {
            case "C":
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
                break;
            case "V":
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
                break;
            default:
                return;
        }

        SoundManager.I.PlayEffect("EFFECT/EquipRegist");
    }

    // �κ��丮 ��ħ üũ
    public bool CheckOverlap()
    {
        if (listOverlap.Count > 0)
            return true;
        else
        {
            EquipEffect();
            return false;
        }
    }

    // ��� ȹ�� �� �������� �ʴٸ� ��� ȿ�� �ߵ�
    private void EquipEffect()
    {
        foreach (EquipScript equip in GottenEquips)
        {
            if (!equip.isEffected)
                equip.EffectThis(true);
        }
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

    // EquipScript���� ����� ��޺� ����Ʈ ��ȯ �Լ�
    public List<EquipScript> ReturnGrade(EquipGrade grade)
    {
        switch (grade)
        {
            case EquipGrade.NORMAL:     return equipsNormal;
            case EquipGrade.RARE:       return equipsRare;
            case EquipGrade.UNIQUE:     return equipsUnique;
            default:                    return null;
        }
    }
}
