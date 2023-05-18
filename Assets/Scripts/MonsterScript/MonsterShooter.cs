using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShooter : MonsterScript
{
    public GameObject bullet;

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

            int iRand = Random.Range(0, 6);
            if (iRand < 1)
            {
                GameObject bbb = Instantiate(bullet, tf.position, Quaternion.identity);
                bbb.transform.SetParent(GameObject.Find("ObjectHolder").transform);
            }
        }
    }
}
