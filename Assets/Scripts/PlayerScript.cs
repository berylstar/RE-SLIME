using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MovingObject
{
    public GameObject punchZip;
    public GameObject[] punches;

    protected override void Start()
    {
        base.Start();

        movetime = GameController.playerSpeed;
    }

    private void Update()
    {
        if (!isAlive || GameController.pause)
            return;

        // Input : ����Ű = �÷��̾� �̵�
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PlayerMove(-1, 0);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            PlayerMove(1, 0);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            PlayerMove(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            PlayerMove(0, -1);

        // Input : �����̽��� = ��ġ ����
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
        // ���Ϳ� �浹 ����
        if (collision.CompareTag("Monster") && isAlive)
        {
            MonsterScript monster = collision.GetComponent<MonsterScript>();

            if (monster.CheckAlive())
            {
                GameController.ChangeHP(-monster.AP);
                StartCoroutine(DamagedAni());
            }
        }
        else if (collision.CompareTag("Bullet") && isAlive)
        {
            BulletScript bullet = collision.GetComponent<BulletScript>();

            GameController.ChangeHP(-bullet.damage);
            StartCoroutine(DamagedAni());
            Destroy(collision.gameObject);
        }
    }

    // Input �޾Ƽ� MovingObject�� Move �Լ� ����
    private void PlayerMove(int dx, int dy)
    {
        StartCoroutine(AniMove());
        Move(dx, dy);
    }

    // �÷��̾� �̵� �ִϸ��̼� �ڷ�ƾ
    IEnumerator AniMove()
    {
        ani.SetTrigger("MoveStart");
        yield return GameController.delay_01s;
        ani.SetTrigger("MoveEnd");
    }

    // �÷��̾� �̵��ӵ� ���� �Լ�
    public void ApplyMoveSpeed()
    {
        movetime = GameController.playerSpeed;
    }

    // �÷��̾� ��ġ ���� �ڷ�ƾ
    IEnumerator PunchAttack()
    {
        // ��ġ ���߿� �÷��̾� ���� �ٲ� �� ����Ͽ� ���⺯�� ����
        int dir = direction;

        punches[dir].GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder;
        punches[dir].SetActive(true);
        yield return GameController.delay_01s;
        punches[dir].SetActive(false);
    }

    // �÷��̾�� ���� �浹�������� �� ����Ǵ� �ڷ�ƾ
    IEnumerator DamagedAni()
    {
        sr.color = new Color(255, 0, 0);
        yield return GameController.delay_025s;
        sr.color = new Color(255, 255, 255);
    }

    // �÷��̾� HP�� 0���ϰ� �Ǹ� ����Ǵ� �ڷ�ƾ
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
