using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class 锤子 : MonoBehaviour
{
    [Header("旋转参数")]
    [SerializeField] float targetAngle = -90f; // 打到球的那一侧
    [SerializeField] float swingTime = 0.25f; // 挥到最低点用时
    [SerializeField] float backTime = 0.4f;  // 收回用时
    [SerializeField] float idleTime = 2f;    // 等待下一次

    [Header("击飞参数")]
    [SerializeField] float hitForce = 25f;      // 给球加的冲击力
    [SerializeField] Vector3 hitDir = Vector3.forward; // 本地坐标，通常=Pivot 的 Z 轴

    [Header("事件")]
    public UnityEvent OnSwing;   // 可让按钮、音效、粒子监听

    HingeJoint hj;
    JointSpring spr;
    float timer;
    bool swinging;

    void Awake()
    {
        // 用 HingeJoint 做“绕根旋转”最稳，也能被物理响应
        hj = GetComponent<HingeJoint>();
        hj.axis = Vector3.right;          // 绕本地 X 轴旋转（水平锤）
        spr = hj.spring;
        spr.spring = 10000;               // 足够硬，保证能跟得动
        hj.spring = spr;
        GoIdle();                         // 初始收拢
    }

    void Update()
    {
        if (!swinging) return;

        timer += Time.deltaTime;
        if (timer < swingTime)
        {
            float t = timer / swingTime;
            SetTargetAngle(Mathf.Lerp(0, targetAngle, t));
        }
        else if (timer < swingTime + backTime)
        {
            float t = (timer - swingTime) / backTime;
            SetTargetAngle(Mathf.Lerp(targetAngle, 0, t));
        }
        else
        {
            GoIdle();
        }
    }

    public void Activate()      // 被按钮/开关调用
    {
        if (swinging) return;   // 别重复激活
        swinging = true;
        timer = 0;
        OnSwing.Invoke();
    }

    void GoIdle()
    {
        swinging = false;
        SetTargetAngle(0);
        Invoke(nameof(AutoRearm), idleTime); // 可选：自动重新上膛
    }

    void AutoRearm() { /* 留空，或把锤子复位逻辑写这里 */ }

    void SetTargetAngle(float ang)
    {
        spr.targetPosition = ang;
        hj.spring = spr;
    }

    // 物理撞击
    void OnTriggerEnter(Collider other)
    {
        if (!swinging) return;           // 没收回来别误伤
        if (timer > swingTime) return;   // 只在挥过去那一下有效

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 worldDir = transform.TransformDirection(hitDir).normalized;
        rb.AddForce(worldDir * hitForce, ForceMode.Impulse);
    }
}
