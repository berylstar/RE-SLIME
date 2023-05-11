using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MovingObject
{
    public GameObject punchZip;
    public GameObject[] punches;

    public UIScript US;

    protected override void Start()
    {
        base.Start();

        movetime *= GameController.playerSpeed;
    }

    private void Update()
    {
        if (!isAlive || GameController.pause)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PlayerMove(-1, 0);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            PlayerMove(1, 0);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            PlayerMove(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            PlayerMove(0, -1);


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
        if (collision.CompareTag("Monster") && isAlive)
        {
            StartCoroutine(PlayerDamaged(collision.GetComponent<MonsterScript>()));
        }
    }

    private void PlayerMove(int dx, int dy)
    {
        StartCoroutine(AniMove());
        Move(dx, dy);
    }

    IEnumerator AniMove()
    {
        ani.SetTrigger("MoveStart");
        yield return GameController.delay_01s;
        ani.SetTrigger("MoveEnd");
    }

    IEnumerator PunchAttack()
    {
        int dir = direction;

        punches[dir].GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder;
        punches[dir].SetActive(true);
        yield return GameController.delay_01s;
        punches[dir].SetActive(false);
    }

    IEnumerator PlayerDamaged(MonsterScript MS)
    {
        if (MS.Alive())
        {
            GameController.ChangeHP(-MS.AP);

            sr.color = new Color(255, 0, 0);
            yield return GameController.delay_025s;
            sr.color = new Color(255, 255, 255);
        }
    }

    IEnumerator PlayerDie()
    {
        isAlive = false;
        ani.SetTrigger("PlayerDie");
        yield return GameController.delay_3s;

        // US.PlayerDie();
        US.panelDie.SetActive(true);
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

        US.panelDie.SetActive(false);
    }

    public bool Alive()
    {
        return isAlive;
    }
}
