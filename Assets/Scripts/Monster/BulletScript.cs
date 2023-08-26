using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("STATUS")]
    public int damage;
    public float moveSpeed;
    [HideInInspector] public int direction;

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    
    private bool isMoving = false;

    protected virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Move(direction);
    }

    // MovingObject �̵� �Լ�
    protected void Move(int direction)
    {
        int xDir;
        int yDir;

        if      (direction == 0) { xDir = -1; yDir = 0; sr.flipX = false; }
        else if (direction == 1) { xDir = 1; yDir = 0; sr.flipX = true; }
        else if (direction == 2) { xDir = 0; yDir = 1; transform.rotation = Quaternion.Euler(0, 0, -90); }
        else if (direction == 3) { xDir = 0; yDir = -1; transform.rotation = Quaternion.Euler(0, 0, 90); }
        else { return; }

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        RaycastHit2D hit = Physics2D.Linecast(start, end, LayerMask.GetMask("BlockingLayer"));

        if (hit && (hit.transform.CompareTag("Wall")))
            Destroy(this.gameObject);

        if (end.x < 0 || end.x > 9 || end.y < 0 || end.y > 9)
            Destroy(this.gameObject);

        if (!isMoving)
        {
            StartCoroutine(SmoothMovement(end));
            sr.sortingOrder = 10 - (int)end.y;          // Y ���� ���� sorting layer ���� => �Ʒ����� ���� �տ� ���̰�
        }
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

        Move(direction);
    }

}
