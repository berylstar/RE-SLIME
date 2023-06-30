using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKingGhost : MonsterScript
{
    private Color32 colorO = new Color32(255, 122, 122, 255);
    private Color32 colorZ = new Color32(255, 122, 122, 0);

    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());

        StartCoroutine(TeleportCo());
    }

    private void OnDestroy()
    {
        BoardManager.I.DropBox(transform.position);
    }

    IEnumerator TeleportCo()
    {
        while (isAlive)
        {
            yield return GameController.delay_3s;
            sr.color = colorZ;
            Teleport();

            yield return GameController.delay_3s;
            sr.color = colorO;
        }
            
    }

    private void Teleport()
    {
        transform.position = BoardManager.I.RandomMonsterPosition(GetComponent<MovingObject>());
    }
}