using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 简单移动 : MonoBehaviour
{
    [Header("要移动的物体（不填则自己）")]
    [SerializeField] private GameObject target;

    [Header("方向与距离")]
    [SerializeField] private Axis moveAxis = Axis.X;
    [SerializeField] private float amplitude = 2f;      // 单向最大位移
    [SerializeField] private float cycleTime = 2f;      // 完整来回一次耗时

    private Vector3 startPos;

    private enum Axis { X, Y, Z }

    private void Awake()
    {
        if (target == null) target = gameObject;
        startPos = target.transform.position;
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time, cycleTime) / cycleTime; // 0~1
        float offset = Mathf.Lerp(-amplitude, amplitude, t);

        Vector3 p = startPos;
        switch (moveAxis)
        {
            case Axis.X: p.x += offset; break;
            case Axis.Y: p.y += offset; break;
            case Axis.Z: p.z += offset; break;
        }
        target.transform.position = p;
    }
}
