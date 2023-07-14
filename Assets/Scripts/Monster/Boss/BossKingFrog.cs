using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKingFrog : MonsterScript
{
    [Header("SPAWNER")]
    public GameObject frog;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());
        StartCoroutine(SpawnCo());
    }

    private void OnDestroy()
    {
        if (!isAlive)
            BoardManager.I.DropBox(transform.position);
    }

    IEnumerator SpawnCo()
    {
        while (isAlive)
        {
            yield return GameController.delay_3s;
            Spawn();
            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject product = Instantiate(frog, BoardManager.I.SpawnPosition(), Quaternion.identity) as GameObject;
        product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
    }
}
