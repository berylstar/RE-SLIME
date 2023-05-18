using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    COIN,       // ����
    REDCOIN,    // ���� ����
    POTION,     // ����
    BOX,        // ����
}

public class ItemScript : MonoBehaviour
{
    public ItemType itemType;

    // �÷��̾�� �浹 ������ ������ ȿ�� �ߵ�
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
        else if (itemType == ItemType.REDCOIN)
        {
            GameController.coin += 3;
        }
        else if (itemType == ItemType.POTION)
        {
            GameController.ChangeHP(GameController.potionEff);
        }

        Destroy(this.gameObject);
    }
}
