using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    NORMAL,     // �Ϲ� ����
    FOLLOW,     // �÷��̾������� �̵�
    QUICK,      // ������ �̵�
    SHOOTER,    // ����ü �߻�
    DASH,       // ����
    ALPHA,      // ����ȭ
    SPAWNER,    // ������Ʈ ��ȯ
    BOSS,       // ���� ���� - ���� ���
}

public class MonsterScript : MovingObject
{
    [Header("STATUS")]
    public int HP;
    public int AP;
    public float speed;
    protected MonsterType type;

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

    public void MonsterDamage(int d)
    {
        StartCoroutine(Damaged(d));
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
}
