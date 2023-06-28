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
        // ��ġ�� �浹 ������ ���� ������
        if (collision.CompareTag("Punch") && isAlive)
        {
            StartCoroutine(Damaged(GameController.playerAP));
        }
    }

    // ������ �������� �̵�
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

    // �÷��̾ ���� �̵�
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

    // ���� �������� �� ������ �ڷ�ƾ
    IEnumerator Damaged(int damage)
    {
        HP -= damage;

        Color temp = sr.color;
        sr.color = new Color(255, 0, 0);
        yield return GameController.delay_01s;
        sr.color = temp;

        if (HP <= 0)
        {
            // �̵��ϴ� ���߿� ���� �� �ٷ� �װ� �ϱ� ����
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

    // Disappear �ִϸ��̼Ǳ��� ������ Animator���� �����ϴ� �Լ�
    private void Disappear()
    {
        BoardManager.I.RemoveMonster(this);
        BoardManager.I.ItemDrop(transform.position);
        Destroy(this.gameObject);
    }

    ////////////////////////////////////
    

    // BoardManager���� EquipThunder�� �Լ�
    public void MonsterDamage(int d)
    {
        StartCoroutine(Damaged(d));
    }

    // BoardManager���� EquipTrafficlight�� �Լ�
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
