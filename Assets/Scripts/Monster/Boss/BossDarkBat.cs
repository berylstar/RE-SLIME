using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDarkBat : MonsterScript
{
    protected override void Start()
    {
        base.Start();

        PlayerScript.I.sight.SetActive(true);

        StartCoroutine(Dash());
        StartCoroutine(MonsterMove());
    }

    private void OnDestroy()
    {
        PlayerScript.I.sight.SetActive(false);

        Vector3 pos = transform.position;
        BoardManager.I.box.transform.position = new Vector3((int)pos.x, (int)pos.y, pos.z);
        BoardManager.I.box.SetActive(true);
        GameController.bossCut = true;
    }

    IEnumerator Dash()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;

            if (Random.Range(0, 4) <= 0)
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

        int a = (int)(sr.sprite.rect.width / 60) - 1;

        if (dir == 0) { sr.flipX = false; end = new Vector2(0 + 0.5f * a, start.y); direction = 0; }
        else if (dir == 1) { sr.flipX = true; end = new Vector2(9 - 0.5f * a, start.y); direction = 1; }
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
