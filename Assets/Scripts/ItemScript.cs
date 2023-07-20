using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    COIN,       // ����
    REDCOIN,    // ���� ����
    POTION,     // ����
    POISON,     // �� ����
    MINIBOX,    // �̴� �ڽ�
    CROWN,      // ������ �հ�
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
                SoundManager.I.PlayEffect("EFFECT/ItemCoin");
            }
            else if (itemType == ItemType.REDCOIN)
            {
                GameController.coin += 3;
                GameController.getCoin += 3;
                SoundManager.I.PlayEffect("EFFECT/ItemCoin");
            }
            else if (itemType == ItemType.POTION)
            {
                GameController.ChangeHP(GameController.potionEff);
                SoundManager.I.PlayEffect("EFFECT/ItemPotion");
            }
            else if (itemType == ItemType.POISON)
            {
                GameController.ChangeHP(-5);
                SoundManager.I.PlayEffect("EFFECT/SlimeDamaged");
            }
            else if (itemType == ItemType.MINIBOX)
            {
                GameController.coin += 1;
                GameController.ChangeHP(5);
                SoundManager.I.PlayEffect("EFFECT/BoxClose");
            }
            else if (itemType == ItemType.CROWN)
            {

            }

            Destroy(this.gameObject);
        }
    }
}
