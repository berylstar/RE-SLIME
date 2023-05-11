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
    public MonsterType type;
    public int HP;
    public int AP;

    protected override void Start()
    {
        base.Start();

        BM.AddMonster(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch") && isAlive)
        {
            StartCoroutine(Damaged());
        }
    }

    public bool Alive()
    {
        return isAlive;
    }

    IEnumerator Damaged()
    {
        HP -= GameController.playerAP;

        sr.color = new Color(255, 0, 0);
        yield return GameController.delay_01s;
        sr.color = new Color(255, 255, 255);

        if (HP <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isAlive = false;
        ani.SetTrigger("MonsterDie");
        yield return GameController.delay_05s;

        BM.RemoveMonster(this);
        Destroy(this.gameObject);

        // ������ ���
    }
}
