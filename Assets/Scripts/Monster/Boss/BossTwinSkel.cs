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
            //GameObject instance = Instantiate(this.gameObject, new Vector3(7.5f, 2.5f + count * 2, 0), Quaternion.identity) as GameObject;
            GameObject others = Instantiate(this.gameObject, BoardManager.I.RandomMonsterPosition(GetComponent<MovingObject>()), Quaternion.identity) as GameObject;
            others.GetComponent<BossTwinSkel>().count = count - 1;
            others.transform.SetParent(GameObject.Find("ObjectHolder").transform);
        }
    }

    private void OnDestroy()
    {
        //if (BoardManager.I.NoMonster())
        if (count == 0)
        {
            BoardManager.I.DropBox(transform.position);
        }
    }
}
