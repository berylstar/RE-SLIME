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
    public SculptureType type;

    private PlayerScript player;

    private bool isEffected = false;
    private bool isOn = false;

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

    // Sculpture ȿ��
    private void SculptureEffect(PlayerScript player)
    {
        isOn = true;

        if (type == SculptureType.WEB)
            StartCoroutine(WebEffect(player));

        else if (type == SculptureType.GRASS)
            StartCoroutine(GrassEffect(player));

        else if (type == SculptureType.LAVA)
            StartCoroutine(LavaEffect(player));

        isEffected = true;
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
    IEnumerator GrassEffect(PlayerScript player)
    {
        yield return null;
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
}
