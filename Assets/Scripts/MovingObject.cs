using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("Sprite")]
    public int xx = 0;
    public int yy = 0;      // ������ �ش�

    protected Rigidbody2D rb2d;
    protected SpriteRenderer sr;
    protected Animator ani;

    protected int direction = 0;     // L, R, U, D
    protected float moveSpeed;
    protected bool isMoving = false;
    protected bool isAlive = true;

    protected virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    // MovingObject �̵� �Լ�
    protected bool Move(int xDir, int yDir)
    {
        if      (xDir < 0) { direction = 0; sr.flipX = false; }
        else if (xDir > 0) { direction = 1; sr.flipX = true; }
        else if (yDir > 0) { direction = 2; }
        else if (yDir < 0) { direction = 3; }

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        RaycastHit2D hit = Physics2D.Linecast(end, end, LayerMask.GetMask("BlockingLayer"));

        if (hit && (hit.transform.CompareTag("NPC") || hit.transform.CompareTag("Wall")))       // �̵� �Ұ� ���̽�
            return false;

        if (end.x - xx < 0 || end.x + xx > 9 || end.y - yy < 0 || end.y + yy > 9)
            return false;

        if (!isMoving)
        {
            StartCoroutine(SmoothMovement(end));
            sr.sortingOrder = 10 - (int)end.y;          // Y ���� ���� sorting layer ���� => �Ʒ����� ���� �տ� ���̰�
            return true;
        }
        else
            return false;
    }

    // �Ų����� �̵��� ���� �ڷ�ƾ
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        float sqrRemainDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(rb2d.position, end, moveSpeed * Time.deltaTime);
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
