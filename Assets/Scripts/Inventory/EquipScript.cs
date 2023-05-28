using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipScript : MonoBehaviour
{
    public List<int> posIndex = new List<int>();

    private InventoryScript IS;
    private Transform tf;
    private SpriteRenderer sr;

    private float coolTime = 0f;

    private void Start()
    {
        tf = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        IS = GameObject.Find("INVENTORY").GetComponent<InventoryScript>();

        tf.position = IS.ReturnGrid(posIndex[0]);
        sr.sortingOrder = 18 - posIndex[0];
    }

    // 인벤토리 인덱스를 벗어나지 않게 설정
    public void Move(int m)
    {
        for (int i = 0; i < posIndex.Count; i++)
        {
            if ((m == 1 && posIndex[i] % 3 < 2) || (m == -1 && posIndex[i] % 3 > 0) || (m == 3 && posIndex[i] < 15) || (m == -3 && posIndex[i] > 2)) { }
            else
                return;
        }

        for (int i = 0; i < posIndex.Count; i++)
        {
            posIndex[i] += m;
        }

        tf.position = IS.ReturnGrid(posIndex[0]);
        sr.sortingOrder = 18 - posIndex[0];
    }

    public void Skill()
    {
        if (coolTime > 0)
            return;

        print(tf.name);
    }
}
