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

        StartCoroutine(MonsterMove());
        StartCoroutine(ActionCo());
    }

    IEnumerator ActionCo()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;

            if (Random.Range(0, 100) <= dashPercent)
                ani.SetTrigger("MonsterAction");
        }
    }

    private void Action()
    {
        StartCoroutine(DashCo(Random.Range(0, 3)));
    }

    IEnumerator DashCo(int dir)
    {
        Vector2 start = transform.position;
        Vector2 end;

        if      (dir == 0)  { sr.flipX = false;     end = new Vector3(width, start.y, 0);  direction = 0; }
        else if (dir == 1)  { sr.flipX = true;      end = new Vector3(9 - width, start.y, 0);  direction = 1; }
        else if (dir == 2)  {                       end = new Vector3(start.x, 9 - height, 0);  direction = 2; }
        else                {                       end = new Vector3(start.x, height, 0);  direction = 3; }

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
