using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBox : MonsterScript
{
    [Header("BOX")]
    public GameObject[] spawn;
    public int spawnPercent;

    protected override void Start()
    {
        base.Start();

        //StartCoroutine(MonsterMove());
        //StartCoroutine(ActionCo());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Change());
            StartCoroutine(MonsterMove());
            StartCoroutine(ActionCo());
        }

        // 펀치와 충돌 감지로 몬스터 데미지
        if (collision.CompareTag("Punch") && isAlive)
        {
            MonsterDamage(GameController.playerAP);
        }
    }

    IEnumerator Change()
    {
        yield return GameController.delay_01s;

        this.tag = "Monster";
        GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);
        ani.enabled = true;
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
        // 1. 랜덤 위치에 스폰
        GameObject product = Instantiate(spawn[Random.Range(0, spawn.Length)], BoardManager.I.SpawnPosition(), Quaternion.identity) as GameObject;
        product.transform.SetParent(GameObject.Find("ObjectHolder").transform);

        // 2. 자기 자리에 스폰하고 이동
        //GameObject product = Instantiate(spawn[Random.Range(0, spawn.Length)], transform.position, Quaternion.identity) as GameObject;
        //product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
        //MoveRandom();
    }
}
