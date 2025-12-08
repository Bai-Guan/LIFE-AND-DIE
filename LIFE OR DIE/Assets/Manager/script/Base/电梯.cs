using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class 电梯 : MonoBehaviour
{
    [Header("固定两点")]
    public Transform pointA;
    public Transform pointB;

    [Header("移动平台")]
    public GameObject platform;

    [Header("参数")]
    public float speed = 2f;
    public bool startAtA = true;

    [Header("事件")]
    public UnityEvent onReachA;
    public UnityEvent onReachB;

    Vector3 target;
    bool isMoving = false;
    bool pendingReverse = false;   // 是否需要先自动返程
    Vector3 pendingTarget;         // 记录真正想去的终点

    private NewPlayerControll _ctx;
    private bool isEnter=false;

    void Awake()
    {
        platform.transform.position = startAtA ? pointA.position : pointB.position;
        target = platform.transform.position;
    }

    /// <summary>
    /// 外部调用：玩家按 E
    /// </summary>
    public void Activate()
    {
        if (isMoving) return;
        if (platform == null) return;
        // 必须站在电梯里且是交互栈顶
        if (!isEnter || StackInteraction.Instance.Peek() != gameObject) return;

        StackInteraction.Instance.PopSomeOne(gameObject);

        // 决定真正目标
        bool atA = Vector3.Distance(platform.transform.position, pointA.position) < 0.01f;
        Vector3 desiredTarget = atA ? pointB.position : pointA.position;

        // 如果已在目标，则先自动返程
        if (Vector3.Distance(platform.transform.position, desiredTarget) < 0.01f)
        {
            pendingReverse = true;
            pendingTarget = desiredTarget;
            target = atA ? pointB.position : pointA.position;
        }
        else
        {
            target = desiredTarget;
        }

        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isEnter = true;
        _ctx=collision.GetComponent<NewPlayerControll>();
        if (_ctx) _ctx.OnInteractPressed += Activate;
    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))return;
        isEnter=false;
        _ctx.OnInteractPressed -= Activate;
    }
    void FixedUpdate()
    {
        if (!isMoving) return;
        if (platform == null) return;
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, target, speed * Time.fixedDeltaTime);

        if (Vector3.Distance(platform.transform.position, target) < 0.005f)
        {
            platform.transform.position = target;
            isMoving = false;

            // 返程结束 → 立即触发真正方向
            if (pendingReverse)
            {
                pendingReverse = false;
                target = pendingTarget;
                isMoving = true;   // 下一帧继续
                return;
            }

            //正常到达
            if (target == pointA.position) onReachA?.Invoke();
            else onReachB?.Invoke();
        }
    }
    private void OnDestroy()
    {
        _ctx.OnInteractPressed -= Activate;
    }
    void OnDrawGizmos()
    {
        if (pointA && pointB)
        {
            Gizmos.color = Color.green; Gizmos.DrawWireSphere(pointA.position, 0.2f);
            Gizmos.color = Color.red; Gizmos.DrawWireSphere(pointB.position, 0.2f);
            Gizmos.color = Color.white; Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
