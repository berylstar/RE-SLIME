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

    // invenchecker의 값 변경함으로써 겹침 확인
    private void FillIC(int v)
    {
        for (int i = 0; i < posIndex.Count; i++)
        {
            IS.invenChecker[posIndex[i]] += v;
        }
    }

    // 인벤토리 인덱스를 벗어나지 않게 설정
    public bool EquipMove(int m)
    {
        for (int i = 0; i < posIndex.Count; i++)
        {
            if ((m == 1 && posIndex[i] % 3 < 2) || (m == -1 && posIndex[i] % 3 > 0) || (m == 3 && posIndex[i] < 15) || (m == -3 && posIndex[i] > 2)) { }
            else
                return false;
        }

        // 이동하기 전 인덱스의 invenchecker 값 감소
        FillIC(-1);

        for (int i = 0; i < posIndex.Count; i++)
        {
            posIndex[i] += m;
        }

        // 이동 후 인덱스의 invenchecker 값 증가
        FillIC(1);

        tf.position = IS.ReturnGrid(posIndex[0]);
        sr.sortingOrder = 18 - posIndex[0];
        return true;
    }

    // PlayerScript에서 Input 받아서 스킬 사용 -> 나중에 구현
    public void Skill()
    {
        if (coolTime > 0)
            return;

        print(tf.name);
    }
}
