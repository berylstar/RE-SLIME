using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlpha : MonsterScript
{
    [Header("ALPHA")]
    public int transparentPercent;

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
            if (Random.Range(0, 100) <= transparentPercent)
                sr.color = colorT;
            else
                sr.color = Color.white;

            yield return GameController.delay_1s;
        }
    }
}
