using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShooter : MonsterScript
{
    [Header("SHOOTER")]
    public GameObject bullet;
    public int shootPercent;    

    private Transform tf;

    protected override void Start()
    {
        base.Start();

        tf = GetComponent<Transform>();

        StartCoroutine(Shoot());
        StartCoroutine(MonsterMove());
    }

    IEnumerator Shoot()
    {
        while(isAlive)
        {
            yield return GameController.delay_1s;

            if (Random.Range(0, 100) <= shootPercent)
                ani.SetTrigger("Shoot");
        }
    }

    private void ShootAnimator()
    {
        GameObject product = Instantiate(bullet, tf.position, Quaternion.identity);
        product.GetComponent<BulletScript>().direction = direction;
        product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
    }
}
