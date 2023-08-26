using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGreatWizard : MonsterScript
{
    [Header("SPAWNER")]
    public GameObject[] spawn;
    public int spawnPercent;

    protected override void Start()
    {
        base.Start();

        PlayerScript.I.sight.SetActive(true);

        StartCoroutine(MonsterMove());
        StartCoroutine(ActionCo());
    }

    private void OnDestroy()
    {
        PlayerScript.I.sight.SetActive(false);

        BoardManager.I.DropBox(transform.position);
    }

    IEnumerator ActionCo()
    {
        while (isAlive)
        {
            yield return GameController.delay_3s;

            if (Random.Range(0, 100) <= spawnPercent)
                ani.SetTrigger("MonsterAction");
        }
    }

    private void Action()
    {
        foreach (GameObject s in spawn)
        {
            Instantiate(s, BoardManager.I.SpawnPosition(), Quaternion.identity, BoardManager.I.objectHolder);
        }
    }
}
