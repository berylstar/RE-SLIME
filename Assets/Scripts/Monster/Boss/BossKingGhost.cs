using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKingGhost : MonsterScript
{
    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());

        StartCoroutine(TeleportCo());
    }

    private void OnDestroy()
    {
        if (!isAlive)
            BoardManager.I.DropBox(transform.position);
    }

    IEnumerator TeleportCo()
    {
        while (isAlive)
        {
            yield return GameController.delay_3s;
            sr.color = Color.clear;
            tag = "Untagged";
            Teleport();

            yield return GameController.delay_3s;
            sr.color = Color.white;
            tag = "Monster";
        }
            
    }

    private void Teleport()
    {
        transform.position = BoardManager.I.TeleportMonsterPosition(GetComponent<MovingObject>());
    }
}