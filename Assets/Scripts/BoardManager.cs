using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameController GC;

    [Header("FIELDS")]
    public Sprite[] imageFields;

    [Header("SCULPTURES")]
    public GameObject[] sculptures;
    public GameObject stair;

    [Header("MONSTERS")]
    public GameObject[] monsters;

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
        Sprite sp = null;

        if      (floor <= 20) { sp = imageFields[Random.Range(0, 4)]; }
        else if (floor <= 40) { sp = imageFields[Random.Range(4, 7)]; }
        else if (floor <= 60) { sp = imageFields[Random.Range(7, 10)]; }
        else if (floor <= 80) { sp = imageFields[Random.Range(10, 12)]; }
        else                  { sp = imageFields[Random.Range(0, 12)]; }

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
        int idx = x * 10 + y;
        Vector3 pos = gridPositions[idx];
        gridPositions.RemoveAt(idx);
        return pos;
    }

    // 어떤 오브젝트 주변 위치를 그리드 리스트에서 제거
    private void RemovePositionAwayFrom(Vector3 pos)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3 removePos = new Vector3(pos.x + i, pos.y + j, 0);
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
        if (floor % 10 != 0)
            stair.transform.position = RandomPosition();
        else
        {
            ReturnPosition(4, 9);
            ReturnPosition(5, 9);
            ReturnPosition(6, 9);
            ReturnPosition(7, 9);
            stair.transform.position = new Vector3 (4.5f, 9f, 0f);
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

        LayoutObjectAtRandom(sculptures, 2, 5);

        LayoutObjectAtRandom(monsters, 1, 4);
    }

    public void AddMonster(MonsterScript mon)
    {
        monsterGroup.Add(mon);
    }

    public void RemoveMonster(MonsterScript mon)
    {
        monsterGroup.Remove(mon);

        if (monsterGroup.Count <= 0)
        {
            stair.GetComponent<StairScript>().StairOpen();
        }
    }

    public void NextFloor()
    {
        if (GameController.floor == 0)
            GameController.floor = GameController.savedFloor;

        GameController.floor += 1;
        FloorSetup(GameController.floor);
    }
}
