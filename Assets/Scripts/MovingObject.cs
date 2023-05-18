using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public LayerMask blockingLayer;

    protected BoardManager BM;
    protected Rigidbody2D rb2d;
    protected SpriteRenderer sr;
    protected Animator ani;

    protected int direction = 0;     // L, R, U, D
    protected float movetime;
    protected bool isMoving = false;
    protected bool isAlive = true;

    protected virtual void Start()
    {
        BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoardManager>();
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    // MovingObject 이동 함수
    protected bool Move(int xDir, int yDir)
    {
        if      (xDir < 0) { direction = 0; sr.flipX = false; }
        else if (xDir > 0) { direction = 1; sr.flipX = true; }
        else if (yDir > 0) { direction = 2; }
        else if (yDir < 0) { direction = 3; }

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        RaycastHit2D hit = Physics2D.Linecast(end, end, blockingLayer);

        if (hit && (hit.transform.CompareTag("NPC") || hit.transform.CompareTag("Wall")))       // 이동 불가 케이스
            return false;

        if (end.x < 0 || end.x > 9 || end.y < 0 || end.y > 9)
            return false;

        if (!isMoving)
        {
            StartCoroutine(SmoothMovement(end));
            sr.sortingOrder = 10 - (int)end.y;          // Y 값에 따라 sorting layer 변경 => 아래쪽일 수록 앞에 보이게
            return true;
        }
        else
            return false;
    }

    // 매끄러운 이동을 위한 코루틴
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        float sqrRemainDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(rb2d.position, end, movetime * Time.deltaTime);
            rb2d.MovePosition(newPos);
            sqrRemainDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

        rb2d.MovePosition(end);
        isMoving = false;
    }

    public bool CheckAlive()
    {
        return isAlive;
    }
}
