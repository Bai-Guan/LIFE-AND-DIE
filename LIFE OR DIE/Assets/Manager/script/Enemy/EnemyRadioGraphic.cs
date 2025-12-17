using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadioGraphic : MonoBehaviour
{
    [Header("检测设置")]
    public float checkInterval = 0.4f;    // 检测间隔
    public float detectionRange = 10f;     // 检测范围
    public float fieldOfView = 60f;       // 视野角度
    public LayerMask obstacleLayer;       // 障碍物层级
    public float 射线Y偏移量 = 1f;
    // 对外公开的检测结果
  [SerializeField] private bool 是否看见了玩家 = false;
    public bool IsPlayerVisible { get => 是否看见了玩家; }
    public Vector2 PlayerPosition { get; private set; }

    private Transform player;
    private Coroutine detectionCoroutine;

    private InitEnemySystem body;
    private int flip
    {
        get {
            if (body == null) return 0; 
            if (body.isFacingLeft)
                return -1;
            else
                return 1;
                    }
    }// 1=向右, -1=向左

    private void Awake()
    {
        body = GetComponent<InitEnemySystem>();
        
    }
    void Start()
    {
        // 查找玩家
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player != null)
        {
            // 开始检测协程
            detectionCoroutine = StartCoroutine(DetectionLoop());
        }
        else
        {
            Debug.LogWarning("未找到玩家对象！请确保玩家有Player标签");
        }
    }

    void OnDestroy()
    {
        if (detectionCoroutine != null)
            StopCoroutine(detectionCoroutine);
    }

    IEnumerator DetectionLoop()
    {
        while (true)
        {
            CheckForPlayer();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void CheckForPlayer()
    {
        if (player == null) return;
        Vector3 Temp=new Vector2(player.position.x,player.position.y+ 射线Y偏移量);
        Vector2 directionToPlayer = (Temp - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 1. 距离检查
        if (distanceToPlayer > detectionRange)
        {
            UpdateDetection(false, Vector2.zero);
            return;
        }

        // 2. 视野方向检查 - 基于怪物朝向
        Vector2 forward = new Vector2(flip, 0); // 根据朝向确定前方
        float dotProduct = Vector2.Dot(forward, directionToPlayer);

        // 点积小于0表示玩家在怪物后方
        if (dotProduct < 0)
        {
            UpdateDetection(false, Vector2.zero);
            return;
        }

        // 3. 视野角度检查
        float angleToPlayer = Vector2.Angle(forward, directionToPlayer);
        if (angleToPlayer > fieldOfView * 0.5f)
        {
            UpdateDetection(false, Vector2.zero);
            return;
        }
        //
        RaycastHit2D[] hits = Physics2D.RaycastAll(
        transform.position,
        directionToPlayer,
        distanceToPlayer,
        obstacleLayer
    );

        // 按距离排序
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        bool canSeePlayer = true;

        // 检查所有碰撞体，如果第一个碰撞体不是玩家，则看不到玩家
        if (hits.Length > 0)
        {
            // 找到第一个不是玩家的碰撞体
            foreach (var hit in hits)
            {
                // 跳过触发器
                if (hit.collider.isTrigger) continue;

                // 如果第一个碰撞体是玩家，那么可以看到玩家
                if (hit.collider.CompareTag("Player"))
                {
                    canSeePlayer = true;
                    break;
                }
                // 如果第一个碰撞体不是玩家（是障碍物），则看不到玩家
                else
                {
                    canSeePlayer = false;
                    break;
                }
            }
        }


        if (canSeePlayer)
        {
            UpdateDetection(true, player.position);
        }
        else
        {
            UpdateDetection(false, Vector2.zero);
        }

        // 调试绘制
        Debug.DrawRay(transform.position, directionToPlayer * detectionRange,
                     canSeePlayer ? Color.green : Color.red, checkInterval);

        // 绘制视野锥形
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        Vector2 origin = transform.position;
        Vector2 forward = new Vector2(flip, 0);

        // 计算视野边界
        Vector2 leftBoundary = Quaternion.Euler(0, 0, fieldOfView * 0.5f) * forward * detectionRange;
        Vector2 rightBoundary = Quaternion.Euler(0, 0, -fieldOfView * 0.5f) * forward * detectionRange;

        Debug.DrawLine(origin, origin + leftBoundary, Color.yellow, checkInterval);
        Debug.DrawLine(origin, origin + rightBoundary, Color.yellow, checkInterval);
    }

    void UpdateDetection(bool detected, Vector2 position)
    {
        是否看见了玩家 = detected;
        PlayerPosition = position;
    }

    // 公共方法：手动重新开始检测
    public void RestartDetection()
    {
        if (detectionCoroutine != null)
            StopCoroutine(detectionCoroutine);

        detectionCoroutine = StartCoroutine(DetectionLoop());
    }

    // 公共方法：设置检测间隔
    public void SetCheckInterval(float interval)
    {
        checkInterval = interval;
        RestartDetection();
    }

    // 设置怪物朝向
    public void SetFlip(float isFacingLeft)
    {
        body.SetFilp(isFacingLeft);
    }

    private void OnEnable()
    {
     
        if (player == null)
        {
            // 每次激活都重新找玩家（场景里可能暂时删过）
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
                return;
        }

        // 重新启动检测协程
        if (detectionCoroutine != null) StopCoroutine(detectionCoroutine);
        detectionCoroutine = StartCoroutine(DetectionLoop());
    }

    private void OnDisable()
    {
        // 失活时停掉协程
        if (detectionCoroutine != null)
        {
            StopCoroutine(detectionCoroutine);
            detectionCoroutine = null;
        }
    }
}
