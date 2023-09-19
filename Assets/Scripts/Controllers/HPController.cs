using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController : MonoBehaviour
{
    private static readonly float damageDelay = 0.5f;   // 나중에 전역으로 관리

    public event Action OnDamagedEvent;
    public event Action OnDieEvent;
    public event Action OnEndInvincibilityEvent;

    private float _timeSinceLastDamaged = float.MaxValue;

    public bool ChangeHealth(float change)
    {
        if (change == 0)
            return false;

        else if (change > 0)
        {
            // 체력 회복
        }
        else
        {
            if (_timeSinceLastDamaged < damageDelay)
                return false;

            // 데미지
            _timeSinceLastDamaged = 0f;

            OnDamagedEvent?.Invoke();

            // 데미지 소리 효과 재생

            //if (hp <= 0)
            //    OnDieEvent?.Invoke();
        }

        return true;
    }
}
