using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (!isAlive || GameController.Pause(5))
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
        if (Input.GetKeyDown(KeyCode.Space) && canPunch)
        {
            StartCoroutine(PunchAttack());
        }

        // Input : C = 스킬 1
        if (Input.GetKeyDown(KeyCode.C) && GameController.skillC)
        {
            GameController.skillC.Skill();
        }

        // Input : V = 스킬 2
        if (Input.GetKeyDown(KeyCode.V) && GameController.skillV)
        {
            GameController.skillV.Skill();
        }

        // Input : I = 인벤토리 -> EquipScript에 구현되어 있음

        // Input : ESC = 메뉴 -> UIScript에 구현되어 있음

        punchZip.transform.position = transform.position;

        if (GameController.playerHP <= 0 && !EquipCresent())
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
        if (collision.CompareTag("Monster") && !EquipBattery())
        {
            MonsterScript monster = collision.GetComponent<MonsterScript>();

            if (monster.CheckAlive())
            {
                StartCoroutine(Damaged(-monster.AP + GameController.playerDP));
            }
        }

        // 투사체와 충돌 감지
        else if (collision.CompareTag("Bullet") && !EquipBattery())
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

    // 변경된 플레이어 이동속도 적용 함수
    public void ApplyMoveSpeed()
    {
        moveSpeed = GameController.playerSpeed;
    }

    // 플레이어 펀치 공격 코루틴
    IEnumerator PunchAttack()
    {
        StartCoroutine(PunchCooltime());

        // 펀치 도중에 플레이어 방향 바뀔 때 대비하여 방향변수 저장
        int dir = direction;

        punches[dir].GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder - 1;
        punches[dir].SetActive(true);
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

            sr.color = new Color(255, 0, 0);
            invincivity = true;

            yield return GameController.delay_05s;

            sr.color = new Color(255, 255, 255);
            invincivity = false;
        }
    }

    // 플레이어 HP가 0이하가 되면 실행되는 코루틴
    IEnumerator PlayerDie()
    {
        isAlive = false;
        ani.SetTrigger("PlayerDie");
        yield return GameController.delay_3s;       // 애니메이터에서 함수로 실행시키자

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
