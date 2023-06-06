using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipType
{
    PASSIVE = 0,
    ACTIVE = 1,
    SKILL = 2,
}

public enum EquipGrade
{
    NORMAL = 1,
    RARE = 2,
    UNIQUE = 3,
}

public class EquipScript : MonoBehaviour
{
    [Header("Information")]
    public int number;
    public string EName;
    public string adject;
    public string effect;
    public int price;
    public EquipType type;
    public EquipGrade grade;
    public float coolTime;
    public bool gotten = false;

    [Header("")]
    public List<int> posIndex = new List<int>();
    public GameObject iconC, iconV;

    private InventoryScript INVEN;
    private Transform tf;
    private SpriteRenderer sr;

    private void Awake()
    {
        tf = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        INVEN = GameObject.Find("INVENTORY").GetComponent<InventoryScript>();

        sr.sortingOrder = 10 - posIndex.Count;
        gameObject.SetActive(false);
    }

    public void GetThis()
    {
        tf.position = INVEN.ReturnGrid(posIndex[0]);
        gotten = true;
        INVEN.countOfEquips[0] -= 1;
        INVEN.countOfEquips[(int)grade] -= 1;
        FillIC(1);

        gameObject.SetActive(true);
    }

    public void RemoveThis()
    {
        gotten = false;
        INVEN.countOfEquips[0] += 1;
        FillIC(-1);

        gameObject.SetActive(false);
    }

    // invenchecker�� �� ���������ν� ��ħ Ȯ��
    private void FillIC(int v)
    {
        for (int i = 0; i < posIndex.Count; i++)
        {
            INVEN.invenChecker[posIndex[i]] += v;
        }
    }

    // �κ��丮 �ε����� ����� �ʰ� ����
    public bool EquipMove(int m)
    {
        for (int i = 0; i < posIndex.Count; i++)
        {
            if ((m == 1 && posIndex[i] % 3 < 2) || (m == -1 && posIndex[i] % 3 > 0) || (m == 3 && posIndex[i] < 15) || (m == -3 && posIndex[i] > 2)) { }
            else
                return false;
        }

        // �̵��ϱ� �� �ε����� invenchecker �� ����
        FillIC(-1);

        for (int i = 0; i < posIndex.Count; i++)
        {
            posIndex[i] += m;
        }

        // �̵� �� �ε����� invenchecker �� ����
        FillIC(1);

        tf.position = INVEN.ReturnGrid(posIndex[0]);
        return true;
    }

    // PlayerScript���� Input �޾Ƽ� ��ų ��� -> ���߿� ����
    public void Skill()
    {
        if (coolTime > 0)
            return;
    }

    public Sprite ReturnSprite()
    {
        return sr.sprite;
    }
}
