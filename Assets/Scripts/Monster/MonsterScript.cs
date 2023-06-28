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
    SPAWNER,    // 오브젝트 소환
    BOSS,       // 보스 몬스터 - 보상 드롭
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
        // 펀치와 충돌 감지로 몬스터 데미지
        if (collision.CompareTag("Punch") && isAlive)
        {
            StartCoroutine(Damaged(GameController.playerAP));
        }
    }

    public void MonsterDamage(int d)
    {
        StartCoroutine(Damaged(d));
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
}
