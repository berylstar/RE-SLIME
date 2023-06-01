using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipScript : MonoBehaviour
{
    [Header("Information")]
    public string EquipName;
    public string sub;
    public string effect;
    public float coolTime;

    [Header("")]
    public List<int> posIndex = new List<int>();
    public GameObject iconC, iconV;

    private InventoryScript IS;
    private Transform tf;
    private SpriteRenderer sr;

    private void Start()
    {
        tf = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        IS = GameObject.Find("INVENTORY").GetComponent<InventoryScript>();

        tf.position = IS.ReturnGrid(posIndex[0]);
        sr.sortingOrder = 18 - posIndex[0];

        FillIC(1);
    }

    // invenchecker�� �� ���������ν� ��ħ Ȯ��
    private void FillIC(int v)
    {
        for (int i = 0; i < posIndex.Count; i++)
        {
            IS.invenChecker[posIndex[i]] += v;
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

        tf.position = IS.ReturnGrid(posIndex[0]);
        sr.sortingOrder = 18 - posIndex[0];
        return true;
    }

    // PlayerScript���� Input �޾Ƽ� ��ų ��� -> ���߿� ����
    public void Skill()
    {
        if (coolTime > 0)
            return;

        print(tf.name);
    }
}
