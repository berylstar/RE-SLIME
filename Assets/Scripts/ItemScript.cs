using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    COIN,       // 코인
    REDCOIN,    // 레드 코인
    POTION,     // 포션
    POISON,     // 독 포션
}

public class ItemScript : MonoBehaviour
{
    public ItemType itemType;

    // 플레이어와 충돌 감지로 아이템 효과 발동
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (itemType == ItemType.COIN)
            {
                GameController.coin += 1;
                GameController.getCoin += 1;
            }
            else if (itemType == ItemType.REDCOIN)
            {
                GameController.coin += 3;
            }
            else if (itemType == ItemType.POTION)
            {
                GameController.ChangeHP(GameController.potionEff);
            }
            else if (itemType == ItemType.POISON)
            {
                GameController.ChangeHP(-5);
            }

            Destroy(this.gameObject);
        }
    }
}
