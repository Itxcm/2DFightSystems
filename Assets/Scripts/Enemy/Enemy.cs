﻿using EpicToonFX;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 核心

    public Core core { get; private set; }
    public Movement movement => core.movement;
    public EnemyCollisionSenses sense => core.enemyCollisionSenses;
    public EnemyState state => core.enemyState;

    public EnemyStats stats => core.enemyStats;

    #endregion 核心

    public D_E_Base entityData => state.entityData;
    public E_StateMachine stateMachine => state.stateMachine;

    public enum EnemyType
    {
        Melee,
        Remote
    }

    public Animator at { get; private set; } // 动画

    public AnimationToScript animationToScript { get; private set; } // 动画事件引用脚本
    public int knockbackDirection { get; private set; } // 眩晕击退方向 1右
    public GameObject stunEffect { get; private set; } // 眩晕特效

    private float attackEffectSpace = 5.0f;// 切换特效间隔
    public float lastAttackEffectTime { get; private set; } // 上次切换特效时间

    [HideInInspector] public bool isUseAbility2 = false; // 是否使用过技能2

    [HideInInspector] public MaterialPropertyBlock mpb; // 渲染材质空值
    [HideInInspector] public Renderer render;  // 渲染

    [HideInInspector] public GameObject ability2Effect1; // 技能2特效1
    [HideInInspector] public GameObject ability2Effect2;// 技能2特效2

    #region 其他函数

    // 更新动画
    private void UpdateAnimation()
    {
        at.SetFloat("yVelocity", movement.rbY);
    }

    // 更新攻击特效
    private void UpdateAttackEffect()
    {
        if (Time.time >= lastAttackEffectTime + attackEffectSpace)
        {
            lastAttackEffectTime = Time.time;
            if (ETFXFireProjectile.Instance)
            {
                ETFXFireProjectile.Instance.SwitchEnemyAttackEffect();
            }
        }
    }

    // 判断是否有该游戏对象 赋值 然后禁用
    private bool JudGeGameObjAlive(string name)
    {
        return transform.Find(name) != null;
    }

    // 初始化某些特效游戏对象
    private void InitEffect()
    {
        if (JudGeGameObjAlive("StunEffect"))
        {
            stunEffect = transform.Find("StunEffect").gameObject;
            stunEffect.SetActive(false);
        }
        if (JudGeGameObjAlive("Ability2Effect"))
        {
            ability2Effect1 = transform.Find("Ability2Effect").gameObject;
            ability2Effect1.SetActive(false);
        }
        if (JudGeGameObjAlive("Ability2BottomEffect"))
        {
            ability2Effect2 = transform.Find("Ability2BottomEffect").gameObject;
            ability2Effect2.SetActive(false);
        }
    }

    #endregion 其他函数

    #region Unity生命周期

    public virtual void Awake()
    {
        core = GetComponentInChildren<Core>();

        at = GetComponent<Animator>();
        mpb = new MaterialPropertyBlock();
        render = GetComponent<Renderer>();
        animationToScript = GetComponent<AnimationToScript>();

        InitEffect();
    }

    private void Start()
    {
        lastAttackEffectTime = Time.time;
    }

    public virtual void Update()
    {
        UpdateAnimation();
        UpdateAttackEffect();
    }

    #endregion Unity生命周期

    #region 设置

    // 设置骨骼动画渲染透明度
    public virtual void SetSpineTransparent(float transparent)
    {
        Color color = new Color(1f, 1f, 1f, transparent);
        mpb.SetColor("_Color", color);
        render.SetPropertyBlock(mpb);
    }

    #endregion 设置

    #region 检测

    // 保护条件
    public virtual bool IsProtect() => !sense.Edge() || sense.Wall();

    // 检测是否可以闪避
    public bool CheckCanDodge() => Time.time >= state.dodge.endDodgeTime + state.dodge.dodgeData.cooldown;

    // 检测技能2
    // private bool CheckAblity2() => stats.currentHealth <= 50.0f && entityData.enemyType == EnemyType.Remote;

    #endregion 检测
}