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
    public int coolTime;
    [HideInInspector] private bool inCoolTime = false;
    [HideInInspector] public bool isEffected = false;

    [Header("")]
    public List<int> posIndex = new List<int>();
    public GameObject iconC, iconV;

    private InventoryScript INVEN;
    private EquipEffector EFFECTOR;
    private Transform tf;
    private SpriteRenderer sr;

    private void Awake()
    {
        tf = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        INVEN = GameObject.Find("INVENTORY").GetComponent<InventoryScript>();
        EFFECTOR = GameObject.Find("INVENTORY").GetComponent<EquipEffector>();
        
        gameObject.SetActive(false);
    }

    private List<EquipScript> ReturnGrade()
    {
        if (grade == EquipGrade.NORMAL)
            return INVEN.equipsNormal;
        else if (grade == EquipGrade.RARE)
            return INVEN.equipsRare;
        else if (grade == EquipGrade.UNIQUE)
            return INVEN.equipsUnique;
        else
            return null;
    }

    public void GetThis()
    {
        tf.position = INVEN.ReturnGrid(posIndex[0]);
        FillIC(1);

        INVEN.GottenEquips.Add(this);
        ReturnGrade().Remove(this);
        GameController.gottenEquips.Add(this);

        gameObject.SetActive(true);
    }

    public void RemoveThis()
    {
        FillIC(-1);

        INVEN.GottenEquips.Remove(this);
        ReturnGrade().Add(this);

        UnEffect();

        if (iconC.activeInHierarchy)
        {
            GameController.skillC = null;
            iconC.SetActive(false);
        }
        else if (iconV.activeInHierarchy)
        {
            GameController.skillV = null;
            iconV.SetActive(false);
        }
            

        gameObject.SetActive(false);
    }

    // invenchecker의 값 변경함으로써 겹침 확인
    private void FillIC(int v)
    {
        for (int i = 0; i < posIndex.Count; i++)
        {
            INVEN.invenChecker[posIndex[i]] += v;
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

        tf.position = INVEN.ReturnGrid(posIndex[0]);
        return true;
    }

    public void Skill()
    {
        if (inCoolTime)
            return;

        StartCoroutine(OnCoolTime());
        EFFECTOR.EquipSkill(number);

        if (type == EquipType.ACTIVE)
            RemoveThis();
    }

    // 스킬 쿨타임 구현
    IEnumerator OnCoolTime()
    {
        int time = coolTime;
        inCoolTime = true;
        sr.color = new Color32(255, 255, 255, 150);

        while (time > 0)
        {
            yield return GameController.delay_1s;
            time -= 1;
        }

        inCoolTime = false;
        sr.color = new Color32(255, 255, 255, 255);
    }

    public Sprite ReturnSprite()
    {
        return sr.sprite;
    }

    public void ApplyEffect()
    {
        EFFECTOR.EquipEffect(number, this);

        isEffected = true;
    }

    public void UnEffect()
    {
        EFFECTOR.EquipUnEffect(number, this);

        isEffected = false;
    }
}
