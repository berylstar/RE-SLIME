using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFollower : MonsterScript
{
    private Transform target;

    protected override void Start()
    {
        base.Start();

        type = MonsterType.FOLLOW;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(MonsterMove());
    }

    private void MoveToPlayer()
    {
        int xDir = 0;
        int yDir = 0;

        if (Random.Range(0, 2) == 0)
        {
            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
                yDir = target.position.y > transform.position.y ? 1 : -1;
            else
                xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        else
        {
            if (Mathf.Abs(target.position.y - transform.position.y) < float.Epsilon)
                xDir = target.position.x > transform.position.x ? 1 : -1;
            else
                yDir = target.position.y > transform.position.y ? 1 : -1;
        }

        Move(xDir, yDir);
    }

    IEnumerator MonsterMove()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;
            MoveToPlayer();
        }
    }
}
