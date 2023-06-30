using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZombie : MonsterScript
{
    public int count;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(MonsterMove());
    }

    private void Update()
    {
        //if(!isAlive)
        //{
        //    if (count > 0)
        //    {
        //        GameObject others = Instantiate(this.gameObject, BoardManager.I.RandomMonsterPosition(GetComponent<MovingObject>()), Quaternion.identity) as GameObject;
        //        others.GetComponent<BossTwinSkel>().count = count - 1;
        //        others.transform.SetParent(GameObject.Find("ObjectHolder").transform);
        //    }
        //}
    }

    private void OnDestroy()
    {
        if (BoardManager.I.NoMonster())
        {
            BoardManager.I.DropBox(transform.position);
        }
    }
}
