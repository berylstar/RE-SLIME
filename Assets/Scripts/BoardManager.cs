using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameController GC;

    public Sprite[] imageFields;

    public GameObject[] sculptures;

    private readonly List<Vector3> gridPositions = new List<Vector3>();
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

    public void TEST()
    {
        FloorSetup(Random.Range(0, 101));
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

    private void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            GameObject instance = Instantiate(tileArray[Random.Range(0, tileArray.Length)], RandomPosition(), Quaternion.identity) as GameObject;
            instance.transform.SetParent(objectHolder);
        }
    }

    private void FloorSetup(int floor)
    {
        if (GameObject.Find("ObjectHolder"))
            Destroy(GameObject.Find("ObjectHolder"));

        objectHolder = new GameObject("ObjectHolder").transform;        // ObjectHolder �ڽ����� ������Ʈ�� �־ ���̶�Ű â ����

        InitialGrid();
        SetField(floor);
        RemovePositionAwayFrom(GC.player.GetComponent<Transform>().position);

        LayoutObjectAtRandom(sculptures, 2, 5);
    }
}
