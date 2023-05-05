using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public LayerMask blockingLayer;

    protected Rigidbody2D rb2d;
    protected SpriteRenderer sr;
    protected Animator ani;

    [HideInInspector] public int direction = 0;     // L, R, U, D
    private readonly float movetime = (1 / 0.05f);
    private bool isMoving = false;

    protected virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    protected bool Move(float xDir, float yDir)
    {
        if      (xDir < 0) { direction = 0; sr.flipX = false; }
        else if (xDir > 0) { direction = 1; sr.flipX = true; }
        else if (yDir > 0) { direction = 2; }
        else if (yDir < 0) { direction = 3; }

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        RaycastHit2D hit = Physics2D.Linecast(end, end, blockingLayer);

        sr.sortingOrder = 20 - (int)end.y;

        if (hit && (hit.transform.CompareTag("NPC") || hit.transform.CompareTag("Wall")))
            return false;

        if (Mathf.Abs(end.x) > 3 || Mathf.Abs(end.y) > 3)
            return false;

        if (!isMoving)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        else
            return false;
    }

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
}
