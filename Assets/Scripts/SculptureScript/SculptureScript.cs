using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SculptureType
{
    WEB,        // �Ź��� = ��õ��� �÷��̾� �̵��ӵ� ����
    GRASS,      // Ǯ �� = �� ������ ������Ʈ ����ȭ
    LAVA,       // ��� - �÷��̾ ���� �ö�� ������ ������

}

public class SculptureScript : MonoBehaviour
{
    public SculptureType sculptureType;

    private bool isEffected = false;

    private float originSpeed = 0f;


    // �÷��̾�� �浹 ������ ������ ȿ�� �ߵ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isEffected)
        {
            SculptureEffect(collision.GetComponent<PlayerScript>());
        }
    }

    private void OnDestroy()
    {
        if (isEffected)
        {
            if (sculptureType == SculptureType.WEB)
            {
                GameController.playerSpeed = originSpeed;
                //player.ApplyMoveSpeed();
            }
        }
    }

    public void SculptureEffect(PlayerScript player)
    {
        if (sculptureType == SculptureType.WEB)
        {
            StartCoroutine(WebEffect(player));
        }
        else if (sculptureType == SculptureType.GRASS)
        {

        }

        isEffected = true;
    }

    IEnumerator WebEffect(PlayerScript player)
    {
        originSpeed = GameController.playerSpeed;

        GameController.playerSpeed /= 2;
        player.ApplyMoveSpeed();

        yield return GameController.delay_3s;

        GameController.playerSpeed = originSpeed;
        player.ApplyMoveSpeed();

        isEffected = false;
    }
}
