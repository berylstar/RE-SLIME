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

        type = MonsterType.SHOOTER;

        tf = GetComponent<Transform>();

        StartCoroutine(MonsterMove());
        StartCoroutine(Shoot());
    }

    private void RandomMove()
    {
        int xDir = 0;
        int yDir = 0;

        int iRand = Random.Range(0, 9);

        if (iRand <= 1) { xDir = 1; }
        else if (iRand <= 3) { xDir = -1; }
        else if (iRand <= 5) { yDir = 1; }
        else if (iRand <= 7) { yDir = -1; }

        Move(xDir, yDir);
    }

    IEnumerator MonsterMove()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;
            RandomMove();
        }
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
