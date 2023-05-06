using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MovingObject
{
    public GameObject[] punches;

    private bool isAlive = true;

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
}
