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

        StartCoroutine(MonsterMove());
        StartCoroutine(ActionCo());
    }

    IEnumerator ActionCo()
    {
        while(isAlive)
        {
            yield return GameController.delay_3s;

            if (Random.Range(0, 100) <= spawnPercent)
                ani.SetTrigger("MonsterAction");
        }
    }

    private void Action()
    {
        GameObject product = Instantiate(spawn, BoardManager.I.SpawnPosition(), Quaternion.identity) as GameObject;
        product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
    }
}
