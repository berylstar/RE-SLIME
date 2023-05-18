using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBig : MonsterScript
{
    private Transform tf;

    protected override void Start()
    {
        base.Start();

        tf = GetComponent<Transform>();
        tf.position = new Vector2(tf.position.x + 0.5f, tf.position.y);

        StartCoroutine(MonsterMove());
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
}
