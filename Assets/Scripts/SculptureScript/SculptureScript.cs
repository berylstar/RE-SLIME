using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SculptureType
{
    WEB,        // 거미줄 = 잠시동안 플레이어 이동속도 감소
    GRASS,      // 풀 숲 = 들어가 있으면 오브젝트 투명화
    LAVA,       // 용암 - 플레이어가 위에 올라와 있으면 데미지

}

public class SculptureScript : MonoBehaviour
{
    public SculptureType sculptureType;

    private bool isEffected = false;

    private float originSpeed = 0f;


    // 플레이어와 충돌 감지로 조형물 효과 발동
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
