using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonsterScript
{
    public GameObject spawn;

    private Transform tf;

    protected override void Start()
    {
        base.Start();

        type = MonsterType.SPAWNER;
        tf = GetComponent<Transform>();

        StartCoroutine(MonsterMove());
        StartCoroutine(Spawn());
    }

    private void RandomMove()
    {
        int xDir = 0;
        int yDir = 0;

        int iRand = Random.Range(0, 9);

        if (iRand <= 1) { xDir = 1; }
        else if (iRand <= 3) { xDir = -1; }
        else if (iRand <= 5) { yDir = 1; }
        else if (iRand <= 7) { yDir = -1; }

        Move(xDir, yDir);
    }

    IEnumerator MonsterMove()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;
            RandomMove();
        }
    }

    IEnumerator Spawn()
    {
        while(isAlive)
        {
            yield return GameController.delay_3s;

            int iRand = Random.Range(0, 6);

            if (iRand < 1)
            {
                GameObject sss = Instantiate(spawn, tf.position, Quaternion.identity);
                sss.transform.SetParent(GameObject.Find("ObjectHolder").transform);
            }
        }
    }
}
