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
    [Multiline (2)]
    public string adject;
    [Multiline(2)]
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

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Equip"))
        {
            InventoryScript.I.listOverlap.Add(number);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Equip"))
        {
            InventoryScript.I.listOverlap.Remove(number);
        }
    }

    private List<EquipScript> ReturnGrade()
    {
        if (grade == EquipGrade.NORMAL)
            return InventoryScript.I.equipsNormal;
        else if (grade == EquipGrade.RARE)
            return InventoryScript.I.equipsRare;
        else if (grade == EquipGrade.UNIQUE)
            return InventoryScript.I.equipsUnique;
        else
            return null;
    }

    public void GetThis()
    {
        transform.position = InventoryScript.I.ReturnGrid(posIndex[0]);

        InventoryScript.I.GottenEquips.Add(this);
        ReturnGrade().Remove(this);

        gameObject.SetActive(true);
    }

    public void RemoveThis()
    {
        InventoryScript.I.GottenEquips.Remove(this);
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
        SoundManager.I.PlayEffect("EFFECT/EquipRemove");
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

        for (int i = 0; i < posIndex.Count; i++)
        {
            posIndex[i] += m;
        }

        transform.position = InventoryScript.I.ReturnGrid(posIndex[0]);
        return true;
    }

    public void Skill()
    {
        if (inCoolTime)
            return;

        StartCoroutine(OnCoolTime());
        EquipEffector.I.EquipSkill(number);

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
        sr.color = Color.white;
    }

    public Sprite ReturnSprite()
    {
        return sr.sprite;
    }

    public void ApplyEffect()
    {
        if (isEffected)
            return;

        EquipEffector.I.ApplyEffect(number, this, true);

        isEffected = true;
    }

    public void UnEffect()
    {
        if (!isEffected)
            return;

        EquipEffector.I.ApplyEffect(number, this, false);

        isEffected = false;
    }

    public void LoadThis(List<int> poses)
    {
        SetPos(poses);
        transform.position = InventoryScript.I.ReturnGrid(posIndex[0]);

        InventoryScript.I.GottenEquips.Add(this);
        ReturnGrade().Remove(this);

        gameObject.SetActive(true);
    }

    private void SetPos(List<int> poses)
    {
        for (int i = 0; i < poses.Count; i++)
        {
            posIndex[i] = poses[i];
        }
    }
}
