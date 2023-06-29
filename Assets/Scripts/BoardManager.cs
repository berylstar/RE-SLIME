using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public static BoardManager I = null;

    [Serializable]
    public struct LevelStruct
    {
        public Sprite[] fieldImages;
        public GameObject[] sculptures;
        public GameObject[] monsters;
        public GameObject[] bosses;
    }

    [Header("LEVELS")]
    public List<LevelStruct> levels = new List<LevelStruct>();

    [Header("ITEMS")]
    public GameObject[] items;

    [Header("NPC")]
    public GameObject kingslime;
    public GameObject sign;
    public GameObject coffin;
    public GameObject box;
    public GameObject recorder;

    private readonly List<Vector3> gridPositions = new List<Vector3>();
    private List<MonsterScript> livingMonsters = new List<MonsterScript>();
    private Transform objectHolder;

    private void Awake()
    {
        I = this;
    }

    // ������Ʈ���� ������ �׸��� ����Ʈ �ʱ�ȭ
    private void InitialGrid()
    {
        gridPositions.Clear();

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0));
            }
        }
    }

    private LevelStruct ObjectPerFloor(int floor)
    {
        return levels[floor / 20];
    }

    // �׸��� ����Ʈ �� ���� ��ġ ��ȯ
    private Vector3 RandomPosition()
    {
        int idx = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[idx];
        gridPositions.RemoveAt(idx);        // ���� ��ġ �ߺ� ���� ���� �׸��� ����Ʈ���� ��ǥ ����
        return pos;
    }

    // �׸��� ����Ʈ �� ���ϴ� ��ǥ ��ȯ
    private Vector3 DesiredPosition(int x, int y)
    {
        Vector3 pos = new Vector3(x, y, 0);
        gridPositions.Remove(pos);
        return pos;
    }

    // � ������Ʈ �ֺ� ��ġ�� �׸��� ����Ʈ���� ����
    private void RemovePositionAwayFrom(Vector3 pos)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3 removePos = new Vector3((int)pos.x + i, (int)pos.y + j, 0);
                gridPositions.Remove(removePos);
            }
        }
    }

    // ��ȯ�Ҷ� �������� �׸��� �����ǿ��� �������� ���� => ��û ���� ������ �� �ڸ� ���� ���� �����ϱ� ����
    public Vector3 SpawnPosition()
    {
        return gridPositions[Random.Range(0, gridPositions.Count)];
    }

    // �׸��� ����Ʈ �� ���� ��ġ ��ȯ
    private Vector3 RandomMonsterPosition(MovingObject go)
    {
        int idx = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[idx];

        //int a = (int)(go.GetComponent<SpriteRenderer>().sprite.rect.width / 60) - 1;

        //pos = new Vector3(pos.x <= 8 ? pos.x + 0.5f * a : 9 - 0.5f * a, pos.y, pos.z);

        float new_x = pos.x <= 8 ? pos.x + 0.5f * go.xx : 9 - 0.5f * go.xx;
        float new_y = pos.y <= 8 ? pos.y + 0.5f * go.yy : 9 - 0.5f * go.yy;

        pos = new Vector3(new_x, new_y, pos.z);

        gridPositions.RemoveAt(idx);
        return pos;
    }

    // ������Ʈ �׷쿡�� �������� ������ ������ ��ġ�� ��ġ
    private void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            GameObject instance = Instantiate(tileArray[Random.Range(0, tileArray.Length)], RandomPosition(), Quaternion.identity) as GameObject;
            instance.transform.SetParent(objectHolder);
        }
    }
    
    private void LayoutStair(int floor)
    {
        if (floor % 20 == 18)
        {
            for (int i = 3; i < 7; i++)
                DesiredPosition(i, 9);

            StairScript.I.transform.position = new Vector3(4.5f, 9f, 0f);
        }
        else
        {
            StairScript.I.transform.position = RandomPosition();
        }

        RemovePositionAwayFrom(StairScript.I.transform.position);
    }

    // ��ܰ� OnTrigger �۵� �Լ�
    public void NextFloor()
    {
        if (GameController.floor == 0)
        {
            GameController.floor = GameController.savedFloor;
            HideNPC();
        }
            
        GameController.floor += 1;
        FloorSetup(GameController.floor);
        StartCoroutine(NextFloorEffect(GameController.floor));
    }

    // �� �Ѿ �� ȿ��
    IEnumerator NextFloorEffect(int floor)
    {
        if (floor % 20 == 19)
        {
            UIScript.I.panelBoss.SetActive(true);
            yield return GameController.delay_3s;
            UIScript.I.panelBoss.SetActive(false);
            SpawnBoss();
        }
        else
        {
            UIScript.I.panelNextFloor.SetActive(true);
            yield return GameController.delay_01s;
            UIScript.I.panelNextFloor.SetActive(false);
        }
    }

    // ���� �����ο� ���� field �̹��� ��ȯ
    private void SetField(int floor)
    {
        GameObject field = GameObject.Find("FIELD");

        Sprite[] fieldSprites = ObjectPerFloor(floor).fieldImages;

        field.GetComponent<SpriteRenderer>().sprite = fieldSprites[Random.Range(0, fieldSprites.Length)];
        field.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 4) * 90));
    }

    private void SetMonsters(int floor, int min, int max)
    {
        if (floor % 20 == 19)
        {

        }
        else
        {
            for (int i = 0; i < Random.Range(min, max + 1); i++)
            {
                GameObject go = ObjectPerFloor(GameController.floor).monsters[Random.Range(0, ObjectPerFloor(GameController.floor).monsters.Length)];
                GameObject instance = Instantiate(go, RandomMonsterPosition(go.GetComponent<MovingObject>()), Quaternion.identity) as GameObject;
                instance.transform.SetParent(objectHolder);
            }
        }
    }

    private void SpawnBoss()
    {
        GameObject go = ObjectPerFloor(GameController.floor).bosses[Random.Range(0, ObjectPerFloor(GameController.floor).bosses.Length)];
        GameObject instance = Instantiate(go, RandomMonsterPosition(go.GetComponent<MovingObject>()), Quaternion.identity) as GameObject;
        instance.transform.SetParent(objectHolder);

        print("BOSS");
    }

    public void TEST()
    {
        SpawnBoss();
    }

    // �ش� ������ �°� ���� ����
    private void FloorSetup(int floor)
    {
        if (GameObject.Find("ObjectHolder"))
            Destroy(GameObject.Find("ObjectHolder"));

        objectHolder = new GameObject("ObjectHolder").transform;        // ObjectHolder �ڽ����� ������Ʈ�� �־ ���̶�Ű â ����
        livingMonsters.Clear();

        InitialGrid();
        SetField(floor);
        RemovePositionAwayFrom(PlayerScript.I.transform.position);

        LayoutStair(floor);

        LayoutObjectAtRandom(ObjectPerFloor(floor).sculptures, 2, 10);

        SetMonsters(floor, 2, 8);
    }

    // ���Ͱ� ������ �� ���� �׷쿡 �߰�
    public void AddMonster(MonsterScript mon)
    {
        livingMonsters.Add(mon);
    }

    // ���� ���� �� ���� �׷쿡�� ����
    public void RemoveMonster(MonsterScript mon)
    {
        livingMonsters.Remove(mon);

        if (livingMonsters.Count <= 0)
        {
            StairScript.I.Open();
        }
    }

    // ���Ͱ� ���� �� �������� ������ ���
    public void ItemDrop(Vector3 pos)
    {
        int iRand = Random.Range(0, 101);
        GameObject item;

        if (iRand <= GameController.probPotion)
            item = items[0];
        else if (100 - GameController.probCoin <= iRand)
        {
            if (GameController.RedCoin && 97 <= iRand)
                item = items[2];
            else
                item = items[1];
        }
        else
            return;

        GameObject instance = Instantiate(item, new Vector3((int)pos.x, (int)pos.y, 0), Quaternion.identity) as GameObject;
        instance.transform.SetParent(objectHolder);
    }

    // ���� ������ �������� �ڽ� ���
    public void DropBox(Vector3 pos)
    {
        GameController.bossCut = true;
        box.transform.position = new Vector3((int)pos.x, (int)pos.y, 0);
        box.SetActive(true);
    }

    private void HideNPC()
    {
        kingslime.SetActive(false);
        sign.SetActive(false);
        coffin.SetActive(false);
        box.SetActive(false);
        recorder.SetActive(false);
    }

    ////////////////////////////////////
    public void EquipThunder(int dam)
    {
        for (int i = 0; i < livingMonsters.Count; i++)
        {
            livingMonsters[i].MonsterDamage(dam);
        }
    }

    public void EquipTrafficlight()
    {
        for (int i = 0; i < livingMonsters.Count; i++)
        {
            livingMonsters[i].MoveStop();
        }
    }
}
