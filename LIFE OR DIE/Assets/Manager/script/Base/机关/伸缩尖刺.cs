using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 伸缩尖刺 : MonoBehaviour
{
    [Header("要伸缩的尖刺（不填则自己）")]
    [SerializeField] private GameObject spike;

    [Header("伸缩方向")]
    [SerializeField] private Axis extendAxis = Axis.Y;
    [SerializeField] private float extendDistance = 1.5f;

    [Header("节奏（秒）")]
    [SerializeField] private float outTime = 0.15f; // 弹出耗时
    [SerializeField] private float waitTime = 0.8f;  // 停留
    [SerializeField] private float backTime = 0.15f; // 缩回耗时
    [SerializeField] private float idleTime = 1f;    // 缩回后等待

    [Header("延迟几秒")]
    [SerializeField] private float laterTime = 0f;
    private float tempTime = 0f;

    private Vector3 retractedPos;
    private Vector3 extendedPos;
    private float timer;
    private enum Axis { X, Y, Z }

    private void Awake()
    {
        if (spike == null) spike = gameObject;
        retractedPos = spike.transform.position;
        Vector3 off = Vector3.zero;
        switch (extendAxis)
        {
            case Axis.X: off.x = extendDistance; break;
            case Axis.Y: off.y = extendDistance; break;
            case Axis.Z: off.z = extendDistance; break;
        }
        extendedPos = retractedPos + off;
    }

    private void Update()
    {
       
        tempTime += Time.deltaTime;
        if(tempTime<=laterTime) {return;}

        timer += Time.deltaTime;
        float total = outTime + waitTime + backTime + idleTime;
        timer %= total;

        Vector3 p;
        if (timer < outTime)                       // 弹出
            p = Vector3.Lerp(retractedPos, extendedPos, timer / outTime);
        else if (timer < outTime + waitTime)       // 停留
            p = extendedPos;
        else if (timer < outTime + waitTime + backTime) // 缩回
            p = Vector3.Lerp(extendedPos, retractedPos,
                             (timer - outTime - waitTime) / backTime);
        else                                       //  idle
            p = retractedPos;

        spike.transform.position = p;
    }
}
