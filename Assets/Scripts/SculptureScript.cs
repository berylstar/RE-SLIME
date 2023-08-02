using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SculptureType
{
    WEB,        // 거미줄 = 잠시동안 플레이어 이동속도 감소
    LAVA,       // 용암 - 플레이어가 위에 올라와 있으면 데미지
    GRASS,      // 풀 - 위에 있는 오브젝트 가려지게
    WALL,       // 벽 - 펀치로 부술 수 있음
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

    // 충돌 감지로 조형물 효과 발동
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

    // 조형물밖으로 나갈 때 감지
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOn = false;
        }
    }

    // 거미줄 밟고 디버프 사라지기 전에 거미줄이 파괴될 때
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

    // 거미줄 효과 코루틴
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

    // 용암 효과 코루틴
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