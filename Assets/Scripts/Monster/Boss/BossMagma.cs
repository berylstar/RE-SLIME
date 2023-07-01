using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMagma : MonsterScript
{
    [Header("SHOOTER")]
    public GameObject fire;
    public int shootPercent;

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
            yield return GameController.delay_1s;

            if (Random.Range(0, 100) <= shootPercent)
                ani.SetTrigger("MonsterAction");
        }
    }

    // Shooting
    private void Action()
    {
        Vector3 pos = transform.position;

        for (int i = 0; i < 4; i++)
        {
            GameObject product = Instantiate(fire, new Vector3((int)pos.x, (int)pos.y, 0), Quaternion.identity);
            product.GetComponent<BulletScript>().direction = i;
            product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
        }
    }
}
