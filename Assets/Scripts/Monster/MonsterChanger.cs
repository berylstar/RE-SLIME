using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChanger : MonoBehaviour
{
    public enum changerType
    {
        book,
        box
    }

    public changerType type;

    private void Start()
    {
        BoardManager.I.AddMonster(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Change());
        }
    }

    IEnumerator Change()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);

        yield return GameController.delay_01s;

        this.tag = "Monster";
        GetComponent<Animator>().enabled = true;

        if (type == changerType.book) GetComponent<MonsterQuick>().enabled = true;
        else if (type == changerType.box) GetComponent<MonsterSpawner>().enabled = true;

        BoardManager.I.RemoveMonster(null);
    }
}
