﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Data/Enemy Data/State Data/Move Data")]
public class D_E_Move : ScriptableObject
{
    [Header("移动速度")] public float moveSpeed = 3.0f;
}