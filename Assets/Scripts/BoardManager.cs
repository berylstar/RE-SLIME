using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameController GC;
    public UIScript US;

    [Header("FIELDS")]
    public Sprite[] imageFields;

    [Header("SCULPTURES")]
    public GameObject[] sculptures;
    public GameObject stair;

    [Header("MONSTERS")]
    public GameObject[] monsters;

    [Header("ITEMS")]
    public GameObject[] items;

    private readonly List<Vector3> gridPositions = new List<Vector3>();
    private List<MonsterScript> monsterGroup;

    private Transform objectHolder;

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

    // 레벨 디자인에 맞춰 field 이미지 변환
    private void SetField(int floor)
    {
        Sprite sp;

        if (floor <= 20) { sp = imageFields[Random.Range(0, 4)]; }
        else if (floor <= 40) { sp = imageFields[Random.Range(4, 7)]; }
        else if (floor <= 60) { sp = imageFields[Random.Range(7, 10)]; }
        else if (floor <= 80) { sp = imageFields[Random.Range(10, 12)]; }
        else { sp = imageFields[Random.Range(0, 12)]; }

        GC.field.GetComponent<SpriteRenderer>().sprite = sp;
        GC.field.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 4) * 90));
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
    private Vector3 ReturnPosition(int x, int y)
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

    // 계단 배치
    // 현재 상태 : 계단 주변 좌표 제외하지 않음 => 벽 같은 거로 막힐 가능성 있음. 추후에 벽 그룹 만들고 계단 주변 좌표 제거 후 계단보다 늦게 배치 예상
    private void LayoutStair(int floor)
    {
        if (floor % 20 != 19)
            stair.transform.position = RandomPosition();
        else
        {
            for (int i = 4; i < 8; i++)
            {
                ReturnPosition(i, 9);
            }

            stair.transform.position = new Vector3 (4.5f, 9f, 0f);
        }
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
        StartCoroutine(NextFloorEffect());
    }

    // 층 넘어갈 때 효과
    IEnumerator NextFloorEffect()
    {
        US.panelNextFloor.SetActive(true);
        yield return GameController.delay_01s;
        US.panelNextFloor.SetActive(false);
    }

    private void SetSculptures()
    {

    }

    private void SetMonsters(int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            GameObject go = monsters[Random.Range(0, monsters.Length)];
            GameObject instance = Instantiate(go, RandomMonsterPosition(go), Quaternion.identity) as GameObject;
            instance.transform.SetParent(objectHolder);
        }
    }

    // 해당 층수에 맞게 레벨 세팅
    private void FloorSetup(int floor)
    {
        if (GameObject.Find("ObjectHolder"))
            Destroy(GameObject.Find("ObjectHolder"));

        objectHolder = new GameObject("ObjectHolder").transform;        // ObjectHolder 자식으로 오브젝트를 넣어서 하이라키 창 정리
        monsterGroup = new List<MonsterScript>();

        InitialGrid();
        SetField(floor);
        RemovePositionAwayFrom(GC.player.GetComponent<Transform>().position);

        LayoutStair(floor);
        //RemovePositionAwayFrom(stair.GetComponent<Transform>().position);
        RemovePositionAwayFrom(stair.transform.position);

                                                    // 추후에 조형물과 몬스터는 레벨에 따라 배치해야함
        LayoutObjectAtRandom(sculptures, 2, 10);

        SetMonsters(2, 8);
    }

    // 몬스터가 생성될 때 몬스터 그룹에 추가
    public void AddMonster(MonsterScript mon)
    {
        monsterGroup.Add(mon);
    }

    // 몬스터 죽을 때 몬스터 그룹에서 제거
    public void RemoveMonster(MonsterScript mon)
    {
        monsterGroup.Remove(mon);

        if (monsterGroup.Count <= 0)
        {
            stair.GetComponent<StairScript>().StairOpen();
        }
    }

    // 몬스터가 죽을 때 랜덤으로 아이템 드롭
    public void ItemDrop(Vector3 pos)
    {
        int iRand = Random.Range(0, 101);
        GameObject item;

        if (iRand <= GameController.probCoin)
            item = items[0];
        else if (100 - GameController.probPotion <= iRand)
        {
            if (GameController.RedCoin && 95 <= iRand)
                item = items[2];
            else
                item = items[1];
        }
        else
            return;

        GameObject instance = Instantiate(item, new Vector3((int)pos.x, (int)pos.y, 0), Quaternion.identity) as GameObject;
        instance.transform.SetParent(objectHolder);
    }

    private void HideNPC()
    {
        GC.kingslime.SetActive(false);
        GC.coffinshop.SetActive(false);
        GC.treasurebox.SetActive(false);
    }

    // 소환한 포지션을 그리드 포지션에서 제거하지 않음 => 엄청 많이 생겼을 때 자리 없는 것을 방지하기 위해
    public Vector3 SpawnPosition()
    {
        int idx = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[idx];
        return pos;
    }

    // 그리드 리스트 내 랜덤 위치 반환
    private Vector3 RandomMonsterPosition(GameObject go)
    {
        int idx = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[idx];

        int a = (int)(go.GetComponent<SpriteRenderer>().sprite.rect.width / 120);

        pos = new Vector3(pos.x <= 8 ? pos.x + 0.5f * a : 9 - 0.5f * a, pos.y, pos.z);

        gridPositions.RemoveAt(idx);
        return pos;
    }
}
