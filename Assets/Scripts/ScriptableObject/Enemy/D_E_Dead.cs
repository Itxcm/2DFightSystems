﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dead", menuName = "Data/Enemy Data/State Data/Dead Data")]
public class D_E_Dead : ScriptableObject
{
    [Header("是否可以重生")] public bool canRebirth = true;
    [Header("重生时间")] public float rebirthTime = 4.0f;
}