using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScarecrow : MonsterScript
{
    [Header("SPAWNER")]
    public GameObject[] spawn;

    [Header("SHOOTER")]
    public GameObject bullet;

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
        GameObject product = Instantiate(spawn[Random.Range(0, spawn.Length)], BoardManager.I.SpawnPosition(), Quaternion.identity) as GameObject;
        product.transform.SetParent(GameObject.Find("ObjectHolder").transform);

        GameObject blet = Instantiate(bullet, transform.position, Quaternion.identity);
        blet.transform.localScale = new Vector3(10 / 3f, 10 / 3f, 1);
        blet.GetComponent<BulletScript>().direction = direction;
        blet.transform.SetParent(GameObject.Find("ObjectHolder").transform);
    }
}