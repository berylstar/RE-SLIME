using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRedSkel : MonsterScript
{
    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());
    }

    private void OnDestroy()
    {

    }
}
