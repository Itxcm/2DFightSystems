﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Combat
{
    public override void CheckKncokBack()
    {
        base.CheckKncokBack();
        if (isKnockbacking)
        {
            if ((movement.rbY <= 0.01f && playerSense.Ground()) || Time.time >= knockbackStartTime + maxKnockbackTime)
            {
                isKnockbacking = false;
                movement.canSetVelocity = true;
            }
        }
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);
        playerStats.DecreaseHealth(amount);
    }

    public override void Knckback(float velocity, Vector2 angle, int direction)
    {
        base.Knckback(velocity, angle, direction);
        movement.SetVelocity(velocity, angle, direction);
        knockbackStartTime = Time.time;
        movement.canSetVelocity = false;
        isKnockbacking = true;
    }
}