using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterQuick : MonsterScript
{
    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterQuickMove());
    }

    IEnumerator MonsterQuickMove()
    {
        while (isAlive)
        {
            yield return GameController.delay_05s;
            MoveRandom();
        }
    }
}
