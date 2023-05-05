using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MovingObject
{
    private static readonly WaitForSeconds delay_01s = new WaitForSeconds(0.1f);

    private void Update()
    {
        int dx = 0;
        int dy = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            dx = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            dx = 1;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            dy = 1;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            dy = -1;

        if (dx != 0)
            dy = 0;

        if (dx != 0 || dy != 0)
        {
            StartCoroutine(AniMove());
            Move(dx, dy);
        }
            
    }

    IEnumerator AniMove()
    {
        ani.SetTrigger("MoveStart");
        yield return delay_01s;
        ani.SetTrigger("MoveEnd");
    }
}
