using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SculptureType
{
    WEB,        // 거미줄 = 잠시동안 플레이어 이동속도 감소
    LAVA,       // 용암 - 플레이어가 위에 올라와 있으면 데미지
    GRASS,      // 풀 숲 = 들어가 있으면 오브젝트 투명화
    ICY,        // 얼음 = 이동하면 그 방향으로 미끄러지게
    TREE,       // 나무
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

    // 충돌 감지로 조형물 효과 발동
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
                player.ApplyMoveSpeed();
            }
        }
    }

    // 거미줄 효과 코루틴
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

    // 풀숲 효과 코루틴
    private void GrassEffect(PlayerScript player)
    {

    }

    // 용암 효과 코루틴
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
