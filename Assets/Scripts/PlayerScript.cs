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
        // �׾��ų� �����϶� �浹 ���� ��������
        if (!isAlive || invincivity)
            return;

        // ���Ϳ� �浹 ����
        if (collision.CompareTag("Monster"))
        {
            MonsterScript monster = collision.GetComponent<MonsterScript>();

            if (monster.CheckAlive())
            {
                StartCoroutine(Damaged(-monster.AP));
            }
        }

        // ����ü�� �浹 ����
        else if (collision.CompareTag("Bullet"))
        {
            BulletScript bullet = collision.GetComponent<BulletScript>();

            StartCoroutine(Damaged(-bullet.damage));
            Destroy(collision.gameObject);
        }
    }

    // Input �޾Ƽ� MovingObject�� Move �Լ� ����
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

    // �÷��̾� �̵��ӵ� ���� �Լ�
    public void ApplyMoveSpeed()
    {
        moveSpeed = GameController.playerSpeed;
    }

    // �÷��̾� ��ġ ���� �ڷ�ƾ
    IEnumerator PunchAttack()
    {
        // ��ġ ���߿� �÷��̾� ���� �ٲ� �� ����Ͽ� ���⺯�� ����
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

    // �÷��̾�� ���� �浹�������� �� ����Ǵ� �ڷ�ƾ
    IEnumerator Damaged(int dam)
    {
        GameController.ChangeHP(dam);

        sr.color = new Color(255, 0, 0);
        invincivity = true;

        yield return GameController.delay_05s;

        sr.color = new Color(255, 255, 255);
        invincivity = false;
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
