using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterQuick : MonsterScript
{
    protected override void Start()
    {
        base.Start();

        type = MonsterType.QUICK;

        StartCoroutine(MonsterMove());
    }

    private void RandomMove()
    {
        int xDir = 0;
        int yDir = 0;

        int iRand = Random.Range(0, 10);

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
            yield return GameController.delay_05s;
            RandomMove();
        }
    }
}
