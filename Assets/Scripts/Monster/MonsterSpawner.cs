using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonsterScript
{
    [Header("SPAWNER")]
    public GameObject spawn;
    public int spawnPercent;

    protected override void Start()
    {
        base.Start();

        type = MonsterType.SPAWNER;

        if (speed > 0)
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

            if (Random.Range(0, 100) <= spawnPercent)
                ani.SetTrigger("Spawn");
        }
    }

    private void SpawnAnimator()
    {
        GameObject product = Instantiate(spawn, BoardManager.I.SpawnPosition(), Quaternion.identity) as GameObject;
        product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
    }
}
