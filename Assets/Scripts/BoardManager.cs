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
        public GameObject[] walls;
        public GameObject[] monsters;
        public GameObject[] bosses;
    }

    public GameObject field;

    [Header("LEVELS")]
    public LevelStruct[] levels;

    [Header("ITEMS")]
    public GameObject[] items;

    [Header("NPC")]
    public GameObject kingslime;
    public GameObject sign;
    public GameObject coffin;
    public GameObject box;
    public GameObject recorder;
    public GameObject bossDemon;

    private readonly List<Vector3> gridPositions = new List<Vector3>();
    private List<MonsterScript> livingMonsters = new List<MonsterScript>();
    public Transform objectHolder;

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
    // MonsterSpawner ���� ���
    public Vector3 SpawnPosition()
    {
        return gridPositions[Random.Range(0, gridPositions.Count)];
    }

    // �׸��� ����Ʈ �� ���� ��ġ ��ȯ
    public Vector3 RandomMonsterPosition(MovingObject go)
    {
        int idx = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[idx];

        float new_x = pos.x + go.width < 9 ? pos.x + go.width : 9 - go.width;
        float new_y = pos.y + go.height < 9 ? pos.y + go.height : 9 - go.height;

        pos = new Vector3(new_x, new_y, pos.z);

        gridPositions.RemoveAt(idx);
        return pos;
    }

    // �׸��� ����Ʈ �� ���� ��ġ ��ȯ
    public Vector3 TeleportMonsterPosition(MovingObject go)
    {
        int idx = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[idx];

        float new_x = pos.x + go.width < 9 ? pos.x + go.width : 9 - go.width;
        float new_y = pos.y + go.height < 9 ? pos.y + go.height : 9 - go.height;

        pos = new Vector3(new_x, new_y, pos.z);

        return pos;
    }

    // ������Ʈ �׷쿡�� �������� ������ ������ ��ġ�� ��ġ
    private void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Instantiate(tileArray[Random.Range(0, tileArray.Length)], RandomPosition(), Quaternion.identity, objectHolder);
        }
    }
    
    private void LayoutStair(int floor)
    {
        if (floor == 100)
        {
            StairScript.I.transform.position = new Vector3(20f, 20f, 0f);
            sign.SetActive(false);
        }
        else if (floor == 99)
        {
            StairScript.I.transform.position = new Vector3(4.5f, 9f, 0f);
            StairScript.I.Open();
        }
        else if (floor == 18 || floor == 38 || floor == 58 || floor == 78)
        {
            for (int i = 3; i < 7; i++)
                RemovePositionAwayFrom(DesiredPosition(i, 9));

            StairScript.I.transform.position = new Vector3(4.5f, 9f, 0f);

            sign.SetActive(true);
        }
        else
        {
            StairScript.I.transform.position = RandomPosition();
            RemovePositionAwayFrom(StairScript.I.transform.position);

            sign.SetActive(false);
        }
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
        SoundManager.I.PlayEffect("EFFECT/NextFloor");

        if (floor % 20 == 19 && floor != 99)
        {
            UIScript.I.panelBoss.SetActive(true);
            GameController.situation.Push(SituationType.BOSS_APPEAR);

            yield return GameController.delay_3s;

            UIScript.I.panelBoss.SetActive(false);
            GameController.situation.Pop();
            SpawnBoss();
        }
        else
        {
            UIScript.I.panelNextFloor.SetActive(true);
            yield return GameController.delay_01s;
            UIScript.I.panelNextFloor.SetActive(false);
        }
    }

    private LevelStruct ObjectPerFloor(int floor)
    {
        if (floor < 80)
            return levels[floor / 20];
        else
            return levels[Random.Range(0, 4)];
    }

    // ���� �����ο� ���� field �̹��� ��ȯ
    private void SetField()
    {
        Sprite[] fieldSprites = ObjectPerFloor(GameController.floor).fieldImages;

        field.GetComponent<SpriteRenderer>().sprite = fieldSprites[Random.Range(0, 4)];
        field.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 4) * 90));
    }

    private void SetWalls(int min, int max)
    {
        for (int i = 0; i < Random.Range(min, max+1); i++)
        {
            Instantiate(ObjectPerFloor(GameController.floor).walls[Random.Range(0, 10)], RandomPosition(), Quaternion.identity, objectHolder);
        }
    }

    private void SetSculptures(int min, int max)
    {
        for (int i = 0; i < Random.Range(min, max + 1); i++)
        {
            Instantiate(ObjectPerFloor(GameController.floor).sculptures[Random.Range(0, 10)], RandomPosition(), Quaternion.identity, objectHolder);
        }
    }

    private void SetMonsters(int floor)
    {
        if (GameController.floor == 80 || GameController.floor == 100)
        {
            Instantiate(bossDemon, new Vector3(6, 4, 0), Quaternion.identity, objectHolder);
        }
        else if (GameController.floor % 20 == 19)
        {
            // NextFloorEffect ���� ���� ����
        }
        else
        {
            int min, max;
            if (floor % 20 <= 5) { min = 2; max = 4; }
            else if (floor % 20 <= 10) { min = 3; max = 6; }
            else if (floor % 20 <= 15) { min = 4; max = 7; }
            else if (floor % 20 <= 20) { min = 5; max = 7; }
            else { min = 2; max = 8; }

            for (int i = 0; i < Random.Range(min, max + 1); i++)
            {
                GameObject go = ObjectPerFloor(GameController.floor).monsters[Random.Range(0, 10)];
                Instantiate(go, RandomMonsterPosition(go.GetComponent<MovingObject>()), Quaternion.identity, objectHolder);
            }
        }
    }

    private void SpawnBoss()
    {
        GameObject go = ObjectPerFloor(GameController.floor).bosses[Random.Range(0, ObjectPerFloor(GameController.floor).bosses.Length)];
        Instantiate(go, RandomMonsterPosition(go.GetComponent<MovingObject>()), Quaternion.identity, objectHolder);
    }

    private void SetBGM(int floor)
    {
        if (floor == 80 || floor == 100)
        {
            SoundManager.I.PlayBGM("BGM/FinalBoss");
        }
        else if (floor % 20 == 19)
        {
            SoundManager.I.PlayBGM("BGM/Boss");
        }
        else
        {
            SoundManager.I.PlayBGM("BGM/Stage" + (int)(floor / 20 + 1));
        }
    }

    // �ش� ������ �°� ���� ����
    private void FloorSetup(int floor)
    {
        foreach (Transform child in objectHolder)
            Destroy(child.gameObject);                                  // objectHolder ����

        livingMonsters.Clear();

        SetBGM(floor);                                                  // 0. BGM ����

        InitialGrid();                                                  // 1. ��ǥ ����
        SetField();                                                     // 2. ��� ����
        RemovePositionAwayFrom(PlayerScript.I.transform.position);      // 3. �÷��̾� ��ǥ ��ó ��ġ ����

        LayoutStair(floor);                                             // 4. ��� ��ġ ���� + ��� ��ó ��ġ ����

        SetSculptures(6, 13);                                           // 5. �ٴ� ����

        SetMonsters(floor);                                             // 6. ���� ����

        SetWalls(2, 5);                                                 // 7. �� ����
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

        Instantiate(item, new Vector3((int)pos.x, (int)pos.y, 0), Quaternion.identity, objectHolder);
    }

    // ���� ������ �������� �ڽ� ���
    public void DropBox(Vector3 pos)
    {
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
