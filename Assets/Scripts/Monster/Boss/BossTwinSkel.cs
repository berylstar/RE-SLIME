using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTwinSkel : MonsterScript
{
    public int count;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());

        if (count > 0)
        {
            GameObject others = Instantiate(this.gameObject, BoardManager.I.RandomMonsterPosition(GetComponent<MovingObject>()), Quaternion.identity, BoardManager.I.objectHolder);
            others.GetComponent<BossTwinSkel>().count = count - 1;
        }
    }

    private void OnDestroy()
    {
        //if (BoardManager.I.NoMonster())
        if (count == 0 && !isAlive)
        {
            BoardManager.I.DropBox(transform.position);
        }
    }
}
