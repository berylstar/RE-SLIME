using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController : MonoBehaviour
{
    private static readonly float damageDelay = 0.5f;   // ���߿� �������� ����

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
            // ü�� ȸ��
        }
        else
        {
            if (_timeSinceLastDamaged < damageDelay)
                return false;

            // ������
            _timeSinceLastDamaged = 0f;

            OnDamagedEvent?.Invoke();

            // ������ �Ҹ� ȿ�� ���

            //if (hp <= 0)
            //    OnDieEvent?.Invoke();
        }

        return true;
    }
}
