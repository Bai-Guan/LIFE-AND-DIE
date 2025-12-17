using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 摆锤 : MonoBehaviour
{
    [Header("摆锤（不填则自己）")]
    [SerializeField] private GameObject pendulum;

    [Header("可选：柱子（不填则忽略）")]
    [SerializeField] private GameObject pillar;   // 拖任意柱子进来
    [Tooltip("柱子的根部世界坐标")]
    [SerializeField] private Transform pillarRoot ;

    [Header("悬挂点（世界坐标）")]
    [SerializeField] private Transform anchor ;

    [Header("摆长与角度")]
    [SerializeField] private float ropeLen = 3f;
    [SerializeField] private float maxAngle = 60f;   // 单侧最大角度
    [Header("周期（秒）")]
    [SerializeField] private float period = 2f;

    private void Awake()
    {
        if (pendulum == null) pendulum = gameObject;
    }

    private void Update()
    {
        // 1. 计算当前摆动角度 θ
        float theta = maxAngle * Mathf.Deg2Rad *
                      Mathf.Sin(2f * Mathf.PI * Time.time / period);

        // 2. 摆锤位置
        Vector3 swingDir = new Vector3(Mathf.Sin(theta), -Mathf.Cos(theta), 0);
        pendulum.transform.position = anchor.position + swingDir * ropeLen;

        // 3. 摆锤自身旋转 → 让“刃”永远朝下
        pendulum.transform.rotation =
            Quaternion.FromToRotation(Vector3.up, -swingDir);

        // 4. 柱子联动（如果有）
        if (pillar != null)
        {
            // 柱子顶部 = 悬挂点，根部 = pillarRoot
            Vector3 pillarDir = (anchor.position - pillarRoot.position).normalized;
            // 摆动后的新方向
            Vector3 newPillarDir = Quaternion.AngleAxis(theta * Mathf.Rad2Deg,
                                                       Vector3.forward) * pillarDir;
            Vector3 newTop = pillarRoot.position + newPillarDir * ropeLen;

            // 把柱子两端对齐
            pillar.transform.position = pillarRoot.position;
            pillar.transform.up = newPillarDir;
            // 如有需要可再 scale 拉伸
            pillar.transform.localScale = new Vector3(
                pillar.transform.localScale.x,
                ropeLen,
                pillar.transform.localScale.z);
        }
    }
}
