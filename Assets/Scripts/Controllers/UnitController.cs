using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action OnSkillEvent;
}
