using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MovingObject
{
    public static PlayerScript I = null;

    public GameObject punchZip;
    public GameObject[] punches;

    [HideInInspector] public bool canPunch = true;       // ��ġ ��Ÿ��
    [HideInInspector] public bool invincivity = false;   // ����

    [Header ("EXTRA")]
    public GameObject bullet;
    public GameObject sight;

    private void Awake()
    {
        I = this;
    }

    protected override void Start()
    {
        base.Start();

        moveSpeed = GameController.playerSpeed;
    }

    private void Update()
    {
        if (!isAlive || GameController.Pause(5))
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

        punchZip.transform.position = transform.position;

        if (GameController.playerHP <= 0 && !EquipCresent())
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
        if (collision.CompareTag("Monster") && !EquipBattery())
        {
            MonsterScript monster = collision.GetComponent<MonsterScript>();

            if (monster.CheckAlive())
            {
                StartCoroutine(Damaged(-monster.AP + GameController.playerDP));
            }
        }

        // ����ü�� �浹 ����
        else if (collision.CompareTag("Bullet") && !EquipBattery())
        {
            BulletScript bullet = collision.GetComponent<BulletScript>();

            StartCoroutine(Damaged(-bullet.damage + GameController.playerDP));
            Destroy(collision.gameObject);
        }
    }

    // StairScript���� �Լ� ����
    public void StartTimeDamage()
    {
        StartCoroutine(TimeDamage());
    }

    // �ð� ������ �ڷ�ƾ
    private IEnumerator TimeDamage()
    {
        float time = 0f;

        while (isAlive)
        {
            if (GameController.floor > 0 && !GameController.Pause(10))
            {
                time += GameController.playerTimeDamage;

                if (time >= 1)
                {
                    GameController.ChangeHP(-1);
                    time = 0f;
                }
            }

            yield return GameController.delay_1s;
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

    // ����� �÷��̾� �̵��ӵ� ���� �Լ�
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

    // ��Ͽ��� ȣ���ϴ� �Լ�
    public void PlayerDamaged(int dam)
    {
        StartCoroutine(Damaged(dam));
    }

    // �÷��̾�� ���� �浹�������� �� ����Ǵ� �ڷ�ƾ
    IEnumerator Damaged(int dam)
    {
        if (dam < 0 && isAlive)
        {
            GameController.ChangeHP(dam);

            sr.color = new Color(255, 0, 0);
            invincivity = true;

            yield return GameController.delay_05s;

            sr.color = new Color(255, 255, 255);
            invincivity = false;
        }
    }

    // �÷��̾� HP�� 0���ϰ� �Ǹ� ����Ǵ� �ڷ�ƾ
    IEnumerator PlayerDie()
    {
        isAlive = false;
        ani.SetTrigger("PlayerDie");
        yield return GameController.delay_3s;       // �ִϸ����Ϳ��� �Լ��� �����Ű��

        UIScript.I.ShowDiePanel(GameController.playerLife);
        yield return GameController.delay_3s;

        if (GameController.playerLife > 0 || EquipLastleaf())
        {
            // REBORN
            GameController.playerLife -= 1;
            GameController.playerHP = GameController.playerMaxHP;
            GameController.savedFloor = GameController.floor - 1;
            GameController.floor = 0;

            while (GameController.speedStack.Count > 0)
                GameController.SpeedStackOut();

            SceneManager.LoadScene("MainScene");
            DataManager.I.SaveData();
        }
        else
        {
            GameController.floor = 0;
            GameController.playerHP = GameController.playerMaxHP;
            SceneManager.LoadScene("IntroScene");
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
    private bool EquipBattery()
    {
        if (GameController.effBattery && Random.Range(0, 20) == 5)
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
    
    private bool EquipCresent()
    {
        if (GameController.effcrescent && Random.Range(0, 19) <= 0)
        {
            GameController.ChangeHP(GameController.playerMaxHP / 2);
            return true;
        }
        return false;
    }

    private bool EquipLastleaf()
    {
        if (GameController.effLastleaf != null && GameController.playerLife == 0)
        {
            GameController.playerLife += 1;
            GameController.effLastleaf.RemoveThis();
            GameController.effLastleaf = null;
            return true;
        }
        return false;
    }

    public void EquipLasergun()
    {
        GameObject product = Instantiate(bullet, transform.position, Quaternion.identity);
        product.GetComponent<BulletScript>().direction = direction;
        product.transform.SetParent(GameObject.Find("ObjectHolder").transform);
    }
}
