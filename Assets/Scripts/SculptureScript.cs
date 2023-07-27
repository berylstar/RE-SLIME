using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SculptureType
{
    WEB,        // �Ź��� = ��õ��� �÷��̾� �̵��ӵ� ����
    LAVA,       // ��� - �÷��̾ ���� �ö�� ������ ������
    GRASS,      // Ǯ - ���� �ִ� ������Ʈ ��������
    WALL,
}

public class SculptureScript : MonoBehaviour
{
    public SculptureType type;

    private PlayerScript player;

    private bool isEffected = false;
    private bool isOn = false;

    private int crack = 5;

    private void Start()
    {
        player = PlayerScript.I;
        
        if (type == SculptureType.GRASS)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 10 - (int)transform.position.y;
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }

    // �浹 ������ ������ ȿ�� �ߵ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch") && type == SculptureType.WALL)
        {
            SoundManager.I.PlayEffect("EFFECT/MonsterDamaged");
            crack -= 1;
            StartCoroutine(BreakingWall());

            if (crack <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        if (GameController.effSkate)
            return;

        if (collision.CompareTag("Player") && !isEffected)
        {
            isOn = true;

            if (type == SculptureType.WEB)
                StartCoroutine(WebEffect(player));

            else if (type == SculptureType.LAVA)
                StartCoroutine(LavaEffect(player));

            isEffected = true;
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

    IEnumerator BreakingWall()
    {
        transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, 0);
        yield return GameController.delay_01s;
        transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, 0);
        yield return GameController.delay_01s;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}