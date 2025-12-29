using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 随设定的物体移动 : MonoBehaviour
{
    [Tooltip("调试用，看当前假父")]
    public GameObject Parent;

    private Vector3 initLocalPos;   // 初始时自己在世界中的位置
    private Transform fakeParent;
    private Vector3 lastParentPos;  // 上一帧假父的世界位置
    public void SetParent(GameObject obj)
    {
        if (obj == null) return;

        fakeParent = obj.transform;
        Parent = obj;

        initLocalPos = transform.position;         
        lastParentPos = fakeParent.position;        
    }

    private void LateUpdate()
    {
        if (fakeParent == null) return;

        Vector3 parentDelta = fakeParent.position - lastParentPos; // 父这一帧移动了多少
        transform.position +=  parentDelta;           // 自己同步移动

        lastParentPos = fakeParent.position;                       // 更新上一帧记录
    }
}
