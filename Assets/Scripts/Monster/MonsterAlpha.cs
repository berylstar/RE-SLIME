using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlpha : MonsterScript
{
    [Header("ALPHA")]
    public int transparentPercent;

    private Color32 colorO = new Color32(255, 255, 255, 255);
    private Color32 colorT = new Color32(255, 255, 255, 50);

    protected override void Start()
    {
        base.Start();

        if (!GameController.effGlasses)
            StartCoroutine(Transparent());
        StartCoroutine(MonsterMove());
    }

    IEnumerator Transparent()
    {
        while (isAlive)
        {
            int iRand = Random.Range(0, 100);
            sr.color = (iRand <= transparentPercent) ? colorT : colorO;

            yield return GameController.delay_1s;
        }
    }
}
