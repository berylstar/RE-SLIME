using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MovingObject
{
    public GameObject[] punches;

    protected override void Start()
    {
        base.Start();

        movetime *= GameController.playerSpeed;
    }

    private void Update()
    {
        if (!isAlive)
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

        punches[dir].SetActive(true);
        yield return GameController.delay_01s;
        punches[dir].SetActive(false);
    }

    IEnumerator PlayerDamaged(MonsterScript MS)
    {
        GameController.PlayerHP -= MS.AP;

        sr.color = new Color(255, 0, 0);
        yield return GameController.delay_01s;
        sr.color = new Color(255, 255, 255);

        if (GameController.PlayerHP <= 0)
        {
            StartCoroutine(PlayerDie());
        }
    }

    IEnumerator PlayerDie()
    {
        isAlive = false;
        ani.SetTrigger("PlayerDie");
        yield return GameController.delay_3s;
        Time.timeScale = 0;
    }
}
