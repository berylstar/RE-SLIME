using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKingGhost : MonsterScript
{
    [Header("ALPHA")]
    public int transparentPercent;

    private Color32 colorO = new Color32(255, 255, 255, 255);
    private Color32 colorT = new Color32(255, 255, 255, 50);

    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());

        if (!GameController.effGlasses)
            StartCoroutine(Transparent());
    }

    IEnumerator Transparent()
    {
        while (isAlive)
        {
            sr.color = (Random.Range(0, 100) <= transparentPercent) ? colorT : colorO;

            yield return GameController.delay_1s;
        }
    }
}