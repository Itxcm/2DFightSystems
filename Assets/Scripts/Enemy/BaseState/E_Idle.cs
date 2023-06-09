﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Idle : E_State
{
    protected D_E_Idle idleData; // 空闲数据
    protected bool isIdleTimeOver; // 空闲时间是否结束

    protected float idleTime; // 空闲时间
    protected bool canTurn; // 能否转身

    public E_Idle(E_StateMachine stateMachine, Enemy entity, string anmName, D_E_Idle idleData) : base(stateMachine, entity, anmName)
    {
        this.idleData = idleData;
    }

    public override void Enter()
    {
        base.Enter();
        SetRandomIdleTime();
        isIdleTimeOver = false;
        canTurn = true;
    }

    public override void Exit()
    {
        base.Exit();
        if (canTurn)
        {
            movement.SetTurn();
        }
    }

    public override void FixUpdate()
    {
        base.FixUpdate();
        movement.SetVelocityX(0.0f);
    }

    public override void Update()
    {
        base.Update();
        if (!isIdleTimeOver && Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    // 设置随机空闲时间
    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(idleData.minIdleTime, idleData.maxIdleTime);
    }

    // 设置能否转身
    public void SetTurn(bool value)
    {
        canTurn = value;
    }
}