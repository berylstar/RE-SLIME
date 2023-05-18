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
    SPAWN,      // ������Ʈ ��ȯ
    BOSS,       // ���� ���� - ���� ���
}

public class MonsterScript : MovingObject
{
    [Header("STATUS")]
    public int HP;
    public int AP;
    public float speed;
    protected MonsterType type;

    private Animator ani;

    protected override void Start()
    {
        base.Start();

        ani = GetComponent<Animator>();

        movetime = speed;

        BM.AddMonster(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��ġ�� �浹 ������ ���� ������
        if (collision.CompareTag("Punch") && isAlive)
        {
            StartCoroutine(Damaged());
        }
    }

    // ���� �������� �� ������ �ڷ�ƾ
    IEnumerator Damaged()
    {
        HP -= GameController.playerAP;

        sr.color = new Color(255, 0, 0);
        yield return GameController.delay_01s;
        sr.color = new Color(255, 255, 255);

        if (HP <= 0)
        {
            // �̵��ϴ� ���߿� ���� �� �ٷ� �װ� �ϱ� ����
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    // ���� �׾��� �� ������ �ڷ�ƾ
    IEnumerator Die()
    {
        isAlive = false;
        ani.SetTrigger("MonsterDie");
        yield return GameController.delay_05s;

        BM.RemoveMonster(this);
        Destroy(this.gameObject);

        // ������ ���
        BM.ItemDrop(transform.position);
    }
}
