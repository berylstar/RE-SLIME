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

    [HideInInspector] public bool canPunch = true;       // 펀치 쿨타임
    [HideInInspector] public bool invincivity = false;   // 무적

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
        // 죽었거나 무적일때 충돌 감지 하지않음
        if (!isAlive || invincivity)
            return;

        // 몬스터와 충돌 감지
        if (collision.CompareTag("Monster") && !EquipBattery())
        {
            MonsterScript monster = collision.GetComponent<MonsterScript>();

            if (monster.CheckAlive())
            {
                StartCoroutine(Damaged(-monster.AP + GameController.playerDP));
            }
        }

        // 투사체와 충돌 감지
        if (collision.CompareTag("Bullet") && !EquipBattery())
        {
            BulletScript bullet = collision.GetComponent<BulletScript>();

            StartCoroutine(Damaged(-bullet.damage + GameController.playerDP));
            Destroy(collision.gameObject);
        }
    }

    // StairScript에서 함수 실행
    public void StartTimeDamage()
    {
        StartCoroutine(TimeDamage());
    }

    // 시간 데미지 코루틴
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

    // Input 받아서 MovingObject의 Move 함수 실행
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

    // 변경된 플레이어 이동속도 적용 함수
    public void ChangeSpeed()
    {
        moveSpeed = GameController.playerSpeed;
    }

    // 플레이어 펀치 공격 코루틴
    IEnumerator PunchAttack()
    {
        StartCoroutine(PunchCooltime());

        // 펀치 도중에 플레이어 방향 바뀔 때 대비하여 방향변수 저장
        int dir = direction;

        punches[dir].GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder;
        punches[dir].SetActive(true);
        SoundManager.I.PlayEffect("EFFECT/SlimePunch");
        yield return GameController.delay_01s;
        punches[dir].SetActive(false);
    }

    // 펀치 공격 쿨타임 코루틴
    IEnumerator PunchCooltime()
    {
        canPunch = false;
        yield return GameController.delay_025s;
        canPunch = true;
    }

    // 용암에서 호출하는 함수
    public void PlayerDamaged(int dam)
    {
        StartCoroutine(Damaged(dam));
    }

    // 플레이어와 몬스터 충돌감지했을 때 실행되는 코루틴
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

    // 플레이어 HP가 0이하가 되면 실행되는 코루틴
    IEnumerator PlayerDieCo()
    {
        isAlive = false;
        GameController.situation.Push(SituationType.DIE);
        UIScript.I.stackAssists.Push("저장중...");
        ani.SetTrigger("PlayerDie");
        SoundManager.I.PlayEffect("EFFECT/SlimeDie");
        yield return GameController.delay_3s;       // 애니메이터에서 함수로 실행시키자

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
