using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNormal : MonsterScript
{
    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());
    }
}
