using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("STATUS")]
    public int damage;
    public float moveSpeed;
    public int direction;

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private Transform tf;
    
    private bool isMoving = false;

    protected virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        tf = GetComponent<Transform>();

        Move(direction);
    }

    // MovingObject 이동 함수
    protected void Move(int direction)
    {
        int xDir = 0;
        int yDir = 0;

        if      (direction == 0) { xDir = -1; yDir = 0; }
        else if (direction == 1) { xDir = 1; yDir = 0; }
        else if (direction == 2) { xDir = 0; yDir = 1; }
        else if (direction == 3) { xDir = 0; yDir = -1; }
        else { return; }

        if      (xDir < 0) { sr.flipX = false; }
        else if (xDir > 0) { sr.flipX = true; }
        else if (yDir > 0) { tf.rotation = Quaternion.Euler(0, 0, -90); }
        else if (yDir < 0) { tf.rotation = Quaternion.Euler(0, 0, 90); }

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        RaycastHit2D hit = Physics2D.Linecast(start, end, LayerMask.GetMask("BlockingLayer"));   // end, end 에서 실험용으로 바꿔봄

        if (hit && (hit.transform.CompareTag("Wall")))       // 이동 불가 케이스
            Destroy(this.gameObject);

        if (end.x < 0 || end.x > 9 || end.y < 0 || end.y > 9)
            Destroy(this.gameObject);

        if (!isMoving)
        {
            StartCoroutine(SmoothMovement(end));
            sr.sortingOrder = 10 - (int)end.y;          // Y 값에 따라 sorting layer 변경 => 아래쪽일 수록 앞에 보이게
        }
    }

    // 매끄러운 이동을 위한 코루틴
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
