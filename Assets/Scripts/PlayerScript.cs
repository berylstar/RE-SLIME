using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
        punchZip.transform.position = transform.position;
    }

    #region InputSystem
    private void OnInventory()
    {
        if (!GameController.tutorial[0])
            return;

        switch (GameController.situation.Peek())
        {
            case SituationType.INVENTORY:
                InventoryScript.I.CloseInventory();
                break;

            case SituationType.NORMAL:
                InventoryScript.I.OpenInventory();
                break;
        }
    }

    private void OnSkill(InputValue value)
    {
        if (GameController.situation.Peek() != SituationType.NORMAL)
            return;

        switch (value.Get<float>())
        {
            case 1:
                if (GameController.skillC) GameController.skillC.Skill();
                break;

            case -1:
                if (GameController.skillV) GameController.skillV.Skill();
                break;
        }
    }

    private void OnPause()
    {
        if (GameController.situation.Peek() != SituationType.NORMAL)
            return;

        UIScript.I.EnterESC();
    }

    private void OnMove(InputValue value)
    {
        if (GameController.situation.Peek() != SituationType.NORMAL)
            return;

        Vector2 dir = value.Get<Vector2>();
        PlayerMove((int)dir.x, (int)dir.y);
    }

    private void OnPunch()
    {
        if (GameController.situation.Peek() != SituationType.NORMAL || !canPunch)
            return;

        StartCoroutine(PunchAttack());
    }
    #endregion

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
        if (collision.CompareTag("Bullet") && !EquipBattery())
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
    IEnumerator TimeDamage()
    {
        while (isAlive)
        {
            if (GameController.situation.Peek() == SituationType.NORMAL)
                GameController.ChangeHP(-1);

            GameController.inTime += 1;
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
        else
            return;

         if (Move(dx, dy))
            SoundManager.I.PlayEffect("EFFECT/SlimeMove");
    }

    // ����� �÷��̾� �̵��ӵ� ���� �Լ�
    public void ChangeSpeed()
    {
        moveSpeed = GameController.playerSpeed;
    }

    // �÷��̾� ��ġ ���� �ڷ�ƾ
    IEnumerator PunchAttack()
    {
        StartCoroutine(PunchCooltime());

        // ��ġ ���߿� �÷��̾� ���� �ٲ� �� ����Ͽ� ���⺯�� ����
        int dir = direction;

        punches[dir].GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder;
        punches[dir].SetActive(true);
        SoundManager.I.PlayEffect("EFFECT/SlimePunch");
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
            SoundManager.I.PlayEffect("EFFECT/SlimeDamaged");

            sr.color = Color.red;
            invincivity = true;

            yield return GameController.delay_05s;

            sr.color = Color.white;
            invincivity = false;
        }
    }

    public void Die()
    {
        if (GameController.playerHP <= 0 && !EquipCresent())
        {
            StartCoroutine(PlayerDieCo());
        }
    }

    // �÷��̾� HP�� 0���ϰ� �Ǹ� ����Ǵ� �ڷ�ƾ
    IEnumerator PlayerDieCo()
    {
        isAlive = false;
        GameController.situation.Push(SituationType.DIE);
        UIScript.I.stackAssists.Push("������...");
        ani.SetTrigger("PlayerDie");
        SoundManager.I.PlayEffect("EFFECT/SlimeDie");
        yield return GameController.delay_3s;       // �ִϸ����Ϳ��� �Լ��� �����Ű��

        UIScript.I.ShowDiePanel(GameController.playerLife);
        yield return GameController.delay_3s;

        UIScript.I.stackAssists.Pop();
        GameController.situation.Pop();
        if (GameController.playerLife > 1 || EquipLastleaf())
        {
            // REBORN
            GameController.playerLife -= 1;
            GameController.playerHP = GameController.playerMaxHP;
            GameController.savedFloor = GameController.floor - 1;
            GameController.floor = 0;

            while (GameController.speedStack.Count > 0)
                GameController.ChangeSpeed(GameController.speedStack.Pop());

            SceneManager.LoadScene("MainScene");
            DataManager.I.SaveData();
        }
        else
        {
            SceneManager.LoadScene("ResultScene");
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    private bool EquipBattery()
    {
        if (GameController.effBattery && Random.Range(0, 19) <= 0)
        {
            StartCoroutine(BatteryCo());
            return true;
        }
        return false;
    }
    
    IEnumerator BatteryCo()
    {
        sr.color = Color.yellow;
        invincivity = true;

        yield return GameController.delay_1s;

        sr.color = Color.white;
        invincivity = false;
    }
    
    private bool EquipCresent()
    {
        if (GameController.effcrescent && Random.Range(0, 20) <= 0)
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
        GameObject product = Instantiate(bullet, transform.position, Quaternion.identity, BoardManager.I.objectHolder) as GameObject;
        product.GetComponent<BulletScript>().direction = direction;
    }
}
