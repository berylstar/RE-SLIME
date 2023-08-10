using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDemon : MonsterScript
{
    public GameObject[] lavas;
    public GameObject crown;

    private Color32 colorT = new Color32(255, 255, 255, 50);

    protected override void Start()
    {
        tag = "Monster";
        GetComponent<DialogueScript>().enabled = false;

        base.Start();

        SoundManager.I.PlayBGM("BGM/Floor100");

        StartCoroutine(DemonMove());
        StartCoroutine(DemonPattern());
    }

    private void OnDestroy()
    {
        if (!isAlive)
        {
            PlayerScript.I.sight.SetActive(false);

            if (GameController.floor == 80)
            {
                BoardManager.I.sign.transform.position = transform.position;
                BoardManager.I.sign.SetActive(true);
            }
            else
            {
                Instantiate(crown, transform.position, Quaternion.identity, GameObject.Find("ObjectHolder").transform);
            }
        }
    }

    private IEnumerator DemonMove()
    {
        int cnt = 0;

        while (isAlive)
        {
            yield return GameController.delay_1s;

            if (cnt < 7)
            {
                MoveRandom();
                cnt += 1;
            }
            else
            {
                Teleport();
                cnt = 0;
            }
        }
    }

    private void Action()
    {
        int iRand = Random.Range(1, 5);

        if (iRand == 0) Teleport();
        else if (iRand == 1) StartCoroutine(Blind());
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

    IEnumerator Blind()
    {
        PlayerScript.I.sight.SetActive(true);

        yield return new WaitForSeconds(5f);

        PlayerScript.I.sight.SetActive(false);
    }

    IEnumerator Transparent()
    {
        sr.color = colorT;

        yield return new WaitForSeconds(5f);

        sr.color = Color.white;
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
        for (int i = 0; i < 8; i++)
        {
            GameObject product = Instantiate(lavas[Random.Range(0, lavas.Length)], BoardManager.I.SpawnPosition(), Quaternion.identity) as GameObject;
            product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
        }
    }
}
