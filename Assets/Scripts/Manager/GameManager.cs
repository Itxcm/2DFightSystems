﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using EpicToonFX;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Header("重生时间")] private float rebirthTime;
    public static GameManager Instance { get; private set; } // 单例
    private float startRebirthTime; //  开始重生时间
    private bool canRebirth; // 是否可以重生
    private GameObject playerRes; // 玩家资源
    private CinemachineVirtualCamera playerCM; // 玩家虚拟摄像机
    private Vector2 size; // 初始重生点
    private BoxCollider2D randomRang; // 重生碰撞(确定重生范围)
    private GameObject rebirthPos;
    [HideInInspector] public TilemapCollider2D tileMap2D; // 地图碰撞(修改复合碰撞)

    private void Awake()
    {
        Instance = this;
        playerRes = Resources.Load<GameObject>("Perfabs/Player/Player");
        playerCM = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();

        randomRang = GameObject.Find("RebirthRang").GetComponent<BoxCollider2D>();
        tileMap2D = GameObject.Find("Map").GetComponent<TilemapCollider2D>();
        size = randomRang.bounds.extents;

        rebirthPos = transform.Find("RebirthPos").gameObject;
    }

    private void Update()
    {
        CheckRebirthState();
    }

    // 检查重生状态
    private void CheckRebirthState()
    {
        if (canRebirth && Time.time >= startRebirthTime + rebirthTime)
        {
            canRebirth = false;
            var player = Instantiate(playerRes, rebirthPos.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
            player.transform.SetSiblingIndex(4);
            var playerScript = player.GetComponent<Player>();
            playerScript.stats.currentHealth = playerScript.stats.maxHealth;
            CanvasHandle.Instance.SetDeadTimer(false);
            if (Application.platform == RuntimePlatform.Android)
            {
                CanvasHandle.Instance.SetCanvasBtnState(true);
            }
            playerCM.Follow = player.transform;
        }
    }

    // 重生
    public void Rebirth()
    {
        canRebirth = true;
        startRebirthTime = Time.time;
    }

    // 获取地图内的随机坐标
    public Vector2 GetRandPos()
    {
        var startPos = Vector2Int.RoundToInt(new Vector2(UnityEngine.Random.Range(-size.x, size.x), UnityEngine.Random.Range(-size.y, size.y)));
        return FindCanAlivePos(startPos);
    }

    // 从指定位置开始 找到最近的可重生坐标
    public Vector2 FindCanAlivePos(Vector2 StartPos)
    {
        tileMap2D.usedByComposite = false;
        float width = 2f;
        float delet = 1f;
        bool loop = true;
        while (loop)
        {
            loop = false;
            Vector2 startPos = StartPos + new Vector2(delet, delet);
            for (float i = 0; i <= width; i += 1f)
            {
                for (float j = 0; j <= width; j += 1f)
                {
                    if ((i != 0 && i != width) && (j != 0 && j != width))
                    {
                        continue;
                    }
                    var checkPos = startPos - new Vector2(j, i);
                    Debug.Log("检查重生" + checkPos);
                    if (!(Math.Abs(checkPos.x) < size.x && Math.Abs(checkPos.y) < size.y))
                    {
                        continue;
                    }
                    loop = true;
                    var hit = Physics2D.OverlapBox(checkPos, new Vector2(0.65f, 1.65f), 0, LayerMask.GetMask("Ground"));
                    if (hit == null)
                    {
                        tileMap2D.usedByComposite = true;
                        return checkPos;
                    }
                }
            }
            width += 2f;
            delet += 1f;
        }
        throw new Exception("找不到空位");
    }
}