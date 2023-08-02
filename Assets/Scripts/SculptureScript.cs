using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SculptureType
{
    WEB,        // �Ź��� = ��õ��� �÷��̾� �̵��ӵ� ����
    LAVA,       // ��� - �÷��̾ ���� �ö�� ������ ������
    GRASS,      // Ǯ - ���� �ִ� ������Ʈ ��������
    WALL,       // �� - ��ġ�� �μ� �� ����
    MINIBOX
}

public class SculptureScript : MonoBehaviour
{
    public SculptureType type;

    private bool isEffected = false;
    private bool isOn = false;

    private int wallCrack = 5;

    private void Start()
    {        
        if (type == SculptureType.GRASS)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 10 - (int)transform.position.y;
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }

    // �浹 ������ ������ ȿ�� �ߵ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Punch") && (type == SculptureType.WALL || type == SculptureType.MINIBOX))
        {
            SoundManager.I.PlayEffect("EFFECT/MonsterDamaged");
            wallCrack -= 1;
            StartCoroutine(BreakingWall());

            if (wallCrack <= 0)
            {
                Destroy(this.gameObject);

                if (type == SculptureType.MINIBOX)
                {
                    GameObject instance = Instantiate(BoardManager.I.items[Random.Range(0, 2)], transform.position, Quaternion.identity) as GameObject;
                    instance.transform.SetParent(gameObject.transform.Find("objectHolder"));
                }
            }
        }

        if (GameController.effSkate)
            return;

        if (collision.CompareTag("Player") && !isEffected)
        {
            isOn = true;

            if (type == SculptureType.WEB)
                StartCoroutine(WebEffect());

            else if (type == SculptureType.LAVA)
                StartCoroutine(LavaEffect());

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
                PlayerScript.I.ApplyMoveSpeed();
            }
        }
    }

    // �Ź��� ȿ�� �ڷ�ƾ
    IEnumerator WebEffect()
    {
        GameController.SpeedStackIn(GameController.playerSpeed);
        GameController.playerSpeed /= 2;
        PlayerScript.I.ApplyMoveSpeed();

        yield return GameController.delay_3s;

        GameController.SpeedStackOut();
        PlayerScript.I.ApplyMoveSpeed();

        isEffected = false;
    }

    // ��� ȿ�� �ڷ�ƾ
    IEnumerator LavaEffect()
    {
        while (isOn)
        {
            PlayerScript.I.PlayerDamaged(-1);

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