using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDasher : MonsterScript
{
    [Header("DASH")]
    public int dashPercent;

    protected override void Start()
    {
        base.Start();

        type = MonsterType.DASH;

        if (speed > 0)
            StartCoroutine(MonsterMove());
        StartCoroutine(Dash());
    }

    IEnumerator MonsterMove()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;
            RandomMove();
        }
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

    IEnumerator Dash()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;

            int iRand = Random.Range(0, 100);

            if (iRand <= dashPercent)
                ani.SetTrigger("Dash");
        }
    }

    private void DashAnimator()
    {
        StartCoroutine(DashCo(Random.Range(0, 3)));
    }

    IEnumerator DashCo(int dir)
    {
        Vector2 start = transform.position;
        Vector2 end;

        if (dir == 0) { sr.flipX = false; end = new Vector2(0, start.y); direction = 0; }
        else if (dir == 1) { sr.flipX = true; end = new Vector2(9, start.y); direction = 1; }
        else if (dir == 2) { end = new Vector2(start.x, 9); direction = 2; }
        else { end = new Vector2(start.x, 0); direction = 3; }

        if (!isMoving)
        {
            isMoving = true;

            while (((Vector2)transform.position - end).sqrMagnitude > float.Epsilon)
            {
                Vector3 newPos = Vector3.MoveTowards(rb2d.position, end, 3 * speed * Time.deltaTime);
                rb2d.MovePosition(newPos);
                yield return null;
            }

            rb2d.MovePosition(end);
            isMoving = false;

            sr.sortingOrder = 10 - (int)end.y;          // Y 값에 따라 sorting layer 변경 => 아래쪽일 수록 앞에 보이게
        }
    }

}
