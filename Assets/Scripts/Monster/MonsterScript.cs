using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    NORMAL,
    FOLLOWER,
}

public class MonsterScript : MovingObject
{
    [Header("STATUS")]
    public int HP;
    public int AP;
    public MoveType movetype;
    public float speed;

    protected override void Start()
    {
        base.Start();

        sr.sortingOrder = 10 - (int)transform.position.y;
        moveSpeed = speed;

        BoardManager.I.AddMonster(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 펀치와 충돌 감지로 몬스터 데미지
        if (collision.CompareTag("Punch") && isAlive)
        {
            StartCoroutine(Damaged(GameController.playerAP));
        }
    }

    // 무작위 방향으로 이동
    protected void MoveRandom()
    {
        int xDir = 0;
        int yDir = 0;

        int iRand = Random.Range(0, 9);

        if      (iRand <= 1) { xDir = 1; }
        else if (iRand <= 3) { xDir = -1; }
        else if (iRand <= 5) { yDir = 1; }
        else if (iRand <= 7) { yDir = -1; }

        Move(xDir, yDir);
    }

    // 플레이어를 향해 이동
    protected void MoveToPlayer()
    {
        int xDir = 0;
        int yDir = 0;

        Transform target = PlayerScript.I.transform;

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

    protected IEnumerator MonsterMove()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;

            if (movetype == MoveType.NORMAL || GameController.effPerfume)
                MoveRandom();
            else if (movetype == MoveType.FOLLOWER)
                MoveToPlayer();
        }
    }

    // 몬스터 피해입을 때 실행할 코루틴
    IEnumerator Damaged(int damage)
    {
        HP -= damage;

        Color temp = sr.color;
        sr.color = new Color(255, 0, 0);
        yield return GameController.delay_01s;
        sr.color = temp;

        if (HP <= 0)
        {
            // 이동하는 도중에 죽을 때 바로 죽게 하기 위해
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isAlive = false;
        ani.SetTrigger("MonsterDie");

        yield return null;
    }

    // Disappear 애니매이션까지 끝나면 Animator에서 실행하는 함수
    private void Disappear()
    {
        BoardManager.I.RemoveMonster(this);
        BoardManager.I.ItemDrop(transform.position);
        Destroy(this.gameObject);
    }

    ////////////////////////////////////
    

    // BoardManager에서 EquipThunder용 함수
    public void MonsterDamage(int d)
    {
        StartCoroutine(Damaged(d));
    }

    // BoardManager에서 EquipTrafficlight용 함수
    public void MoveStop()
    {
        StartCoroutine(MoveStopCo());
    }

    IEnumerator MoveStopCo()
    {
        moveSpeed = 0f;
        yield return GameController.delay_3s;
        moveSpeed = speed;
    }
}
