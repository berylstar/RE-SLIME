using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWerewolf : MonsterScript
{
    [Header("SPAWNER")]
    public GameObject wolf;

    protected override void Start()
    {
        base.Start();

        PlayerScript.I.sight.SetActive(true);

        StartCoroutine(MonsterMove());
        StartCoroutine(SpawnCo());
    }

    private void OnDestroy()
    {
        PlayerScript.I.sight.SetActive(false);

        BoardManager.I.DropBox(transform.position);
    }

    IEnumerator SpawnCo()
    {
        while (isAlive)
        {
            yield return GameController.delay_3s;

            Spawn();

            yield return GameController.delay_3s;
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject product = Instantiate(wolf, BoardManager.I.SpawnPosition(), Quaternion.identity) as GameObject;
            product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
        }
    }
}
