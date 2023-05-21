using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDash : MonsterScript
{
    protected override void Start()
    {
        base.Start();

        type = MonsterType.DASH;

        StartCoroutine(MoveOrDash());
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

    IEnumerator MoveOrDash()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;

            int iRand = Random.Range(0, 6);

            if (iRand == 0)
                StartCoroutine(Dash());
            else
                RandomMove();
        }
    }

    IEnumerator Dash()
    {
        Vector2 start = transform.position;
        Vector2 end = transform.position;

        if (direction == 0) { sr.flipX = false; end = new Vector2(0, start.y); }
        else if (direction == 1) { sr.flipX = true; end = new Vector2(9, start.y); }
        else if (direction == 2) { end = new Vector2(start.x, 9); }
        else if (direction == 3) { end = new Vector2(start.x, 0); }

        if (!isMoving)
        {
            isMoving = true;

            while (((Vector2)transform.position - end).sqrMagnitude > float.Epsilon)
            {
                Vector3 newPos = Vector3.MoveTowards(rb2d.position, end, 80 * Time.deltaTime);
                rb2d.MovePosition(newPos);
                yield return null;
            }

            rb2d.MovePosition(end);
            isMoving = false;

            sr.sortingOrder = 10 - (int)end.y;          // Y 값에 따라 sorting layer 변경 => 아래쪽일 수록 앞에 보이게
        }
    }

}
