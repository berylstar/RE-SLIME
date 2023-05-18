using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MovingObject
{
    // 쏘는 오브젝트의 방향으로 Move함수 계속 실행

    private int iRand;

    protected override void Start()
    {
        base.Start();

        movetime = 20;

        iRand = Random.Range(0, 4);
    }

    private void Update()
    {
        if (!isAlive)
            return;

        if (iRand == 0)
            Move(-1, 0);
        else if (iRand == 1)
            Move(1, 0);
        else if (iRand == 2)
            Move(0, -1);
        else
            Move(0, 1);
    }
}
