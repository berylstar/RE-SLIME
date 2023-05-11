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

    // ���� �����ο� ���� field �̹��� ��ȯ
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

    // �׸��� ����Ʈ �� ���� ��ġ ��ȯ
    private Vector3 RandomPosition()
    {
        int idx = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[idx];
        gridPositions.RemoveAt(idx);        // ���� ��ġ �ߺ� ���� ���� �׸��� ����Ʈ���� ��ǥ ����
        return pos;
    }

    // �׸��� ����Ʈ �� ���ϴ� ��ǥ ��ȯ
    private Vector3 ReturnPosition(int x, int y)
    {
        int idx = x * 10 + y;
        Vector3 pos = gridPositions[idx];
        gridPositions.RemoveAt(idx);
        return pos;
    }

    // � ������Ʈ �ֺ� ��ġ�� �׸��� ����Ʈ���� ����
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

    // ��� ��ġ
    // ���� ���� : ��� �ֺ� ��ǥ �������� ���� => �� ���� �ŷ� ���� ���ɼ� ����. ���Ŀ� �� �׷� ����� ��� �ֺ� ��ǥ ���� �� ��ܺ��� �ʰ� ��ġ ����
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

    // �ش� ������ �°� ���� ����
    private void FloorSetup(int floor)
    {
        if (GameObject.Find("ObjectHolder"))
            Destroy(GameObject.Find("ObjectHolder"));

        objectHolder = new GameObject("ObjectHolder").transform;        // ObjectHolder �ڽ����� ������Ʈ�� �־ ���̶�Ű â ����
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
