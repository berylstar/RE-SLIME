using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SculptureType
{
    WEB,        // �Ź��� = ��õ��� �÷��̾� �̵��ӵ� ����
    LAVA,       // ��� - �÷��̾ ���� �ö�� ������ ������
    GRASS,      // Ǯ �� = �� ������ ������Ʈ ����ȭ
    ICY,        // ���� = �̵��ϸ� �� �������� �̲�������
    TREE,       // ����
}

public class SculptureScript : MonoBehaviour
{
    public SculptureType type;

    private PlayerScript player;

    private bool isEffected = false;
    private bool isOn = false;

    private void Start()
    {
        player = PlayerScript.I;
        
        if (type == SculptureType.TREE)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 10 - (int)transform.position.y;
        }
    }

    // �浹 ������ ������ ȿ�� �ߵ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameController.effSkate)
            return;

        if (collision.CompareTag("Player") && !isEffected)
        {
            isOn = true;

            if (type == SculptureType.WEB)
                StartCoroutine(WebEffect(player));

            else if (type == SculptureType.LAVA)
                StartCoroutine(LavaEffect(player));

            else if (type == SculptureType.GRASS)
                GrassEffect(player);

            isEffected = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && type == SculptureType.ICY)
        {
            IcyEffect(player);
        }
    }

    // ������������ ���� �� ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOn = false;
        }
    }

    // �Ź��� ��� ����� ������� ���� �Ź����� �ı��� ��
    private void OnDestroy()
    {
        if (isEffected)
        {
            if (type == SculptureType.WEB)
            {
                GameController.SpeedStackOut();
                player.ApplyMoveSpeed();
            }
        }
    }

    // �Ź��� ȿ�� �ڷ�ƾ
    IEnumerator WebEffect(PlayerScript player)
    {
        GameController.SpeedStackIn(GameController.playerSpeed);
        GameController.playerSpeed /= 2;
        player.ApplyMoveSpeed();

        yield return GameController.delay_3s;

        GameController.SpeedStackOut();
        player.ApplyMoveSpeed();

        isEffected = false;
    }

    // Ǯ�� ȿ�� �ڷ�ƾ
    private void GrassEffect(PlayerScript player)
    {

    }

    // ��� ȿ�� �ڷ�ƾ
    IEnumerator LavaEffect(PlayerScript player)
    {
        while (isOn)
        {
            player.PlayerDamaged(-1);

            yield return GameController.delay_1s;
        }

        isEffected = false;
    }

    private void IcyEffect(PlayerScript player)
    {
        player.DirectionMove();

        isEffected = false;
    }
}
