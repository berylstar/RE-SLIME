using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private UnitController unitController;
    private HPController hpController;

    private static readonly int IsDamaged = Animator.StringToHash("IsDamaged");
    private static readonly int Skill = Animator.StringToHash("Skill");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        unitController = GetComponent<UnitController>();
        hpController = GetComponent<HPController>();
    }

    private void Start()
    {
        unitController.OnSkillEvent += OnSkill;

        hpController.OnDamagedEvent += OnDamaged;
    }

    private void OnSkill()
    {
        animator.SetTrigger(Skill);
    }

    private void OnDamaged()
    {
        animator.SetTrigger(IsDamaged);
    }
}
