using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MovingObject
{
    public GameObject punchZip;
    public GameObject[] punches;

    public bool canPunch = true;       // ��ġ ��Ÿ��
    public bool invincivity = false;   // ����

    protected override void Start()
    {
        base.Start();

        moveSpeed = GameController.playerSpeed;

        StartCoroutine(GameController.TimeDamage(this));
    }

    private void Update()
    {
        if (!isAlive || GameController.Pause(10))
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
        if (Input.GetKeyDown(KeyCode.Space) && canPunch)
        {
            StartCoroutine(PunchAttack());
        }

        // Input : C = ��ų 1
        if (Input.GetKeyDown(KeyCode.C) && GameController.skillC)
        {
            GameController.skillC.Skill();
        }

        // Input : V = ��ų 2
        if (Input.GetKeyDown(KeyCode.V) && GameController.skillV)
        {
            GameController.skillV.Skill();
        }

        // Input : I = �κ��丮 -> EquipScript�� �����Ǿ� ����

        // Input : ESC = �޴� -> UIScript�� �����Ǿ� ����

        punchZip.transform.position = this.transform.position;

        if (GameController.playerHP <= 0 && !Cresent())
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
        if (collision.CompareTag("Monster") && !Battery())
        {
            MonsterScript monster = collision.GetComponent<MonsterScript>();

            if (monster.CheckAlive())
            {
                StartCoroutine(Damaged(-monster.AP + GameController.playerDP));
            }
        }

        // ����ü�� �浹 ����
        else if (collision.CompareTag("Bullet") && !Battery())
        {
            BulletScript bullet = collision.GetComponent<BulletScript>();

            StartCoroutine(Damaged(-bullet.damage + GameController.playerDP));
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
        StartCoroutine(PunchCooltime());

        // ��ġ ���߿� �÷��̾� ���� �ٲ� �� ����Ͽ� ���⺯�� ����
        int dir = direction;

        punches[dir].GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder - 1;
        punches[dir].SetActive(true);
        yield return GameController.delay_01s;
        punches[dir].SetActive(false);
    }

    // ��ġ ���� ��Ÿ�� �ڷ�ƾ
    IEnumerator PunchCooltime()
    {
        canPunch = false;
        yield return GameController.delay_025s;
        canPunch = true;
    }

    public void PlayerDamaged(int dam)
    {
        StartCoroutine(Damaged(dam));
    }

    // �÷��̾�� ���� �浹�������� �� ����Ǵ� �ڷ�ƾ
    IEnumerator Damaged(int dam)
    {
        if (dam >= 0)
            yield return null;

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

        if (GameController.playerLife > 0 || Lastleaf())
        {
            // REBORN
            GameController.playerLife -= 1;
            GameController.playerHP = GameController.playerMaxHP;
            GameController.savedFloor = GameController.floor - 1;
            GameController.floor = 0;

            while (GameController.speedStack.Count > 0)
                GameController.SpeedStackOut();

            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            // GAME OVER
        }
    }

    public void DirectionMove()
    {
        if (direction == 0)      Move(-1, 0);
        else if (direction == 1) Move(1, 0);
        else if (direction == 2) Move(0, 1);
        else if (direction == 3) Move(0, -1);
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    private bool Battery()
    {
        if (GameController.effBattery && Random.Range(0, 10) <= 0)
        {
            StartCoroutine(BatteryCo());
            return true;
        }
        return false;
    }

    
    IEnumerator BatteryCo()
    {
        sr.color = new Color(255, 255, 0);
        invincivity = true;

        yield return GameController.delay_1s;

        sr.color = new Color(255, 255, 255);
        invincivity = false;
    }
    
    private bool Cresent()
    {
        if (GameController.effcrescent && Random.Range(0, 19) <= 0)
        {
            GameController.ChangeHP(GameController.playerMaxHP / 2);
            return true;
        }
        return false;
    }

    private bool Lastleaf()
    {
        if (GameController.effLastleaf != null && GameController.playerLife == 0)
        {
            GameController.playerLife += 1;
            GameController.effLastleaf.RemoveThis();
            return true;
        }
        return false;
    }
}
