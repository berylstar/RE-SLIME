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

    // 오브젝트들이 생성될 그리드 리스트 초기화
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

    // 그리드 리스트 내 랜덤 위치 반환
    private Vector3 RandomPosition()
    {
        int idx = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[idx];
        gridPositions.RemoveAt(idx);        // 같은 위치 중복 방지 위해 그리드 리스트에서 좌표 제거
        return pos;
    }

    // 그리드 리스트 내 원하는 좌표 반환
    private Vector3 DesiredPosition(int x, int y)
    {
        Vector3 pos = new Vector3(x, y, 0);
        gridPositions.Remove(pos);
        return pos;
    }

    // 어떤 오브젝트 주변 위치를 그리드 리스트에서 제거
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

    // 소환할때 포지션을 그리드 포지션에서 제거하지 않음 => 엄청 많이 생겼을 때 자리 없는 것을 방지하기 위해
    public Vector3 SpawnPosition()
    {
        return gridPositions[Random.Range(0, gridPositions.Count)];
    }

    // 그리드 리스트 내 랜덤 위치 반환
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

    // 오브젝트 그룹에서 무작위로 선택해 무작위 위치에 배치
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

    // 계단과 OnTrigger 작동 함수
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

    // 층 넘어갈 때 효과
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

    // 레벨 디자인에 맞춰 field 이미지 변환
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

    // 해당 층수에 맞게 레벨 세팅
    private void FloorSetup(int floor)
    {
        if (GameObject.Find("ObjectHolder"))
            Destroy(GameObject.Find("ObjectHolder"));

        objectHolder = new GameObject("ObjectHolder").transform;        // ObjectHolder 자식으로 오브젝트를 넣어서 하이라키 창 정리
        livingMonsters.Clear();

        InitialGrid();
        SetField(floor);
        RemovePositionAwayFrom(PlayerScript.I.transform.position);

        LayoutStair(floor);

        LayoutObjectAtRandom(ObjectPerFloor(floor).sculptures, 2, 10);

        SetMonsters(floor, 2, 8);
    }

    // 몬스터가 생성될 때 몬스터 그룹에 추가
    public void AddMonster(MonsterScript mon)
    {
        livingMonsters.Add(mon);
    }

    // 몬스터 죽을 때 몬스터 그룹에서 제거
    public void RemoveMonster(MonsterScript mon)
    {
        livingMonsters.Remove(mon);

        if (livingMonsters.Count <= 0)
        {
            StairScript.I.Open();
        }
    }

    // 몬스터가 죽을 때 랜덤으로 아이템 드롭
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

    // 보스 잡으면 보상으로 박스 드롭
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
