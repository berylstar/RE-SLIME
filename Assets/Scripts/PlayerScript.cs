using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MovingObject
{
    public GameObject punchZip;
    public GameObject[] punches;

    private bool invincivity = false;

    protected override void Start()
    {
        base.Start();

        moveSpeed = GameController.playerSpeed;
    }

    private void Update()
    {
        if (!isAlive || GameController.pause)
            return;

        // Input : 방향키 = 플레이어 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PlayerMove(-1, 0);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            PlayerMove(1, 0);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            PlayerMove(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            PlayerMove(0, -1);

        // Input : 스페이스바 = 펀치 공격
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(PunchAttack());

        punchZip.transform.position = this.transform.position;

        if (GameController.playerHP <= 0)
        {
            StartCoroutine(PlayerDie());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 죽었거나 무적일때 충돌 감지 하지않음
        if (!isAlive || invincivity)
            return;

        // 몬스터와 충돌 감지
        if (collision.CompareTag("Monster"))
        {
            MonsterScript monster = collision.GetComponent<MonsterScript>();

            if (monster.CheckAlive())
            {
                StartCoroutine(Damaged(-monster.AP));
            }
        }

        // 투사체와 충돌 감지
        else if (collision.CompareTag("Bullet"))
        {
            BulletScript bullet = collision.GetComponent<BulletScript>();

            StartCoroutine(Damaged(-bullet.damage));
            Destroy(collision.gameObject);
        }
    }

    // Input 받아서 MovingObject의 Move 함수 실행
    private void PlayerMove(int dx, int dy)
    {
        if (dx != 0)
            ani.SetTrigger("MoveLR");
        else if (dy == 1)
            ani.SetTrigger("MoveUp");
        else if (dy == -1)
            ani.SetTrigger("MoveDown");

        Move(dx, dy);
    }

    // 플레이어 이동속도 변경 함수
    public void ApplyMoveSpeed()
    {
        moveSpeed = GameController.playerSpeed;
    }

    // 플레이어 펀치 공격 코루틴
    IEnumerator PunchAttack()
    {
        // 펀치 도중에 플레이어 방향 바뀔 때 대비하여 방향변수 저장
        int dir = direction;

        punches[dir].GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder - 1;
        punches[dir].SetActive(true);
        yield return GameController.delay_01s;
        punches[dir].SetActive(false);
    }

    public void PlayerDamaged(int dam)
    {
        StartCoroutine(Damaged(dam));
    }

    // 플레이어와 몬스터 충돌감지했을 때 실행되는 코루틴
    IEnumerator Damaged(int dam)
    {
        GameController.ChangeHP(dam);

        sr.color = new Color(255, 0, 0);
        invincivity = true;

        yield return GameController.delay_05s;

        sr.color = new Color(255, 255, 255);
        invincivity = false;
    }

    // 플레이어 HP가 0이하가 되면 실행되는 코루틴
    IEnumerator PlayerDie()
    {
        isAlive = false;
        ani.SetTrigger("PlayerDie");
        yield return GameController.delay_3s;

        BM.US.panelDie.SetActive(true);
        yield return GameController.delay_3s;

        if (GameController.playerLife > 0)
        {
            // REBORN
            GameController.playerLife -= 1;
            GameController.playerHP = GameController.playerMaxHP;
            GameController.savedFloor = GameController.floor - 1;
            GameController.floor = 0;

            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            // GAME OVER
        }
    }

}
