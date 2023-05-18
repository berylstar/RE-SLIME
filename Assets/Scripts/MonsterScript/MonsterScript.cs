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
        // 펀치와 충돌 감지로 몬스터 데미지
        if (collision.CompareTag("Punch") && isAlive)
        {
            StartCoroutine(Damaged());
        }
    }

    // 몬스터 피해입을 때 실행할 코루틴
    IEnumerator Damaged()
    {
        HP -= GameController.playerAP;

        sr.color = new Color(255, 0, 0);
        yield return GameController.delay_01s;
        sr.color = new Color(255, 255, 255);

        if (HP <= 0)
        {
            // 이동하는 도중에 죽을 때 바로 죽게 하기 위해
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    // 몬스터 죽었을 때 실행할 코루틴
    IEnumerator Die()
    {
        isAlive = false;
        ani.SetTrigger("MonsterDie");
        yield return GameController.delay_05s;

        BM.RemoveMonster(this);
        Destroy(this.gameObject);

        // 아이템 드롭
        BM.ItemDrop(transform.position);
    }
}
