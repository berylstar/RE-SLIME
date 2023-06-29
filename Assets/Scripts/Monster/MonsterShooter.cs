using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShooter : MonsterScript
{
    [Header("SHOOTER")]
    public GameObject bullet;
    public int shootPercent;

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
            yield return GameController.delay_1s;

            if (Random.Range(0, 100) <= shootPercent)
                ani.SetTrigger("MonsterAction");
        }
    }

    // Shooting
    private void Action()
    {
        GameObject product = Instantiate(bullet, transform.position, Quaternion.identity);
        product.GetComponent<BulletScript>().direction = direction;
        product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
    }
}
