using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDemon : MonsterScript
{
    public GameObject[] lavas;
    public GameObject crown;

    private Color32 colorO = new Color32(255, 255, 255, 255);
    private Color32 colorT = new Color32(255, 255, 255, 50);

    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());
        StartCoroutine(DemonPattern());
    }

    private void OnDestroy()
    {
        if (!isAlive)
        {
            PlayerScript.I.sight.SetActive(false);

            Instantiate(crown, transform.position, Quaternion.identity, GameObject.Find("ObjectHolder").transform);
        }
    }

    private void Action()
    {
        int iRand = Random.Range(0, 5);

        if (iRand == 0) Teleport();
        else if (iRand == 1) Blind();
        else if (iRand == 2) StartCoroutine(Transparent());
        else if (iRand == 3) SpawnMonster();
        else if (iRand == 4) SpawnLava();
    }

    IEnumerator DemonPattern()
    {
        while (isAlive)
        {
            yield return GameController.delay_3s;
            ani.SetTrigger("MonsterAction");
        }
    }

    private void Teleport()
    {
        transform.position = BoardManager.I.TeleportMonsterPosition(GetComponent<MovingObject>());
    }

    private void Blind()
    {
        PlayerScript.I.sight.SetActive(true);
    }

    IEnumerator Transparent()
    {
        sr.color = colorT;

        yield return GameController.delay_3s;
        yield return GameController.delay_3s;

        sr.color = colorO;
    }

    private void SpawnMonster()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject[] mongroup = BoardManager.I.levels[Random.Range(0, 3)].monsters;
            GameObject monster = mongroup[Random.Range(0, mongroup.Length)];

            GameObject product = Instantiate(monster, BoardManager.I.TeleportMonsterPosition(monster.GetComponent<MovingObject>()), Quaternion.identity) as GameObject;
            product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
        }
    }

    private void SpawnLava()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject product = Instantiate(lavas[Random.Range(0, lavas.Length)], BoardManager.I.SpawnPosition(), Quaternion.identity) as GameObject;
            product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
        }
    }
}
