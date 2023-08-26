using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScarecrow : MonsterScript
{
    [Header("SPAWNER")]
    public GameObject bat;

    [Header("SHOOTER")]
    public GameObject crow;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());
        StartCoroutine(ActionCo());
    }

    private void OnDestroy()
    {
        BoardManager.I.DropBox(transform.position);
    }

    IEnumerator ActionCo()
    {
        while (isAlive)
        {
            yield return GameController.delay_3s;

            ani.SetTrigger("MonsterAction");
        }
    }

    private void Action()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(bat, BoardManager.I.SpawnPosition(), Quaternion.identity, BoardManager.I.objectHolder);
        }
        
        GameObject blet = Instantiate(crow, transform.position, Quaternion.identity, BoardManager.I.objectHolder);
        blet.transform.localScale = new Vector3(10 / 3f, 10 / 3f, 1);
        blet.GetComponent<BulletScript>().direction = direction;
    }
}