using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    COIN,       // ����
    REDCOIN,    // ���� ����
    POTION,     // ����
    POISON,     // �� ����
}

public class ItemScript : MonoBehaviour
{
    public ItemType itemType;

    // �÷��̾�� �浹 ������ ������ ȿ�� �ߵ�
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
