using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    COIN,       // 코인
    REDCOIN,    // 레드 코인
    POTION,     // 포션
    BOX,        // 상자
}

public class ItemScript : MonoBehaviour
{
    public ItemType itemType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ItemEffect();
        }
    }

    public void ItemEffect()
    {
        if (itemType == ItemType.COIN)
        {
            GameController.coin += 1;
        }
        else if (itemType == ItemType.POTION)
        {
            GameController.ChangeHP(GameController.potionEff);
        }

        Destroy(this.gameObject);
    }
}
