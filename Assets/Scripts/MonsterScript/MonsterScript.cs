using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    NORMAL,     // 일반 몬스터
    FOLLOW,     // 플레이어쪽으로 이동
    QUICK,      // 빠르게 이동
    SHOOTER,    // 투사체 발사
    DASH,       // 돌진
    ALPHA,      // 투명화
    SPAWN,      // 오브젝트 소환
    BOSS,       // 보스 몬스터 - 보상 드롭
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

        // 아이템 드롭
    }
}
