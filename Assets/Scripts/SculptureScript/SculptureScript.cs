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

    private PlayerScript player;
    private bool isEffected = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // �浹 ������ ������ ȿ�� �ߵ�
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
                GameController.playerSpeed = GameController.tempSpeed;
                player.ApplyMoveSpeed();
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
        GameController.tempSpeed = GameController.playerSpeed;

        GameController.playerSpeed /= 2;
        player.ApplyMoveSpeed();

        yield return GameController.delay_3s;

        GameController.playerSpeed = GameController.tempSpeed;
        player.ApplyMoveSpeed();

        isEffected = false;
    }
}
