using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAlpha : MonsterScript
{
    [Header("ALPHA")]
    public int transparentPercent;

    private bool isTransparent = false;

    private Color32 colorO = new Color32(255, 255, 255, 255);
    private Color32 colorT = new Color32(255, 255, 255, 50);

    protected override void Start()
    {
        base.Start();

        type = MonsterType.ALPHA;

        StartCoroutine(MonsterMove());

        if (!GameController.effGlasses)
            StartCoroutine(Transparent());
    }

    private void RandomMove()
    {
        int xDir = 0;
        int yDir = 0;

        int iRand = Random.Range(0, 9);

        if (iRand <= 1) { xDir = 1; }
        else if (iRand <= 3) { xDir = -1; }
        else if (iRand <= 5) { yDir = 1; }
        else if (iRand <= 7) { yDir = -1; }

        Move(xDir, yDir);
    }

    IEnumerator MonsterMove()
    {
        while (isAlive)
        {
            yield return GameController.delay_1s;
            RandomMove();
        }
    }

    IEnumerator Transparent()
    {
        while (isAlive)
        {
            int iRand = Random.Range(0, 100);
            isTransparent = (iRand <= transparentPercent) ? true : false;
            sr.color = isTransparent ? colorT : colorO;

            yield return GameController.delay_1s;
        }
    }
}
