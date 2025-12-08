using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 设置巡逻点 : Action
{
    //public SharedFloat 巡逻点A;
    //public SharedFloat 巡逻点B;
    //public SharedFloat 移动速度 = 3f;
    //public SharedFloat 到达距离 = 0.1f;
    //public SharedFloat 等待时间 = 2f;

    //public SharedBool isfacingLeft;

    //private InitEnemySystem body;
    //private Transform 当前目标;
    //private float 等待计时器;
    //private bool 正在等待;

   

    //public override void OnStart()
    //{
    //    body=GetComponent<InitEnemySystem>();   
    //    isfacingLeft=body.isFacingLeft;
    //    // 初始目标设为A点
    //  //  当前目标 = 巡逻点A.Value;
    //    正在等待 = false;
    //}

    //public override TaskStatus OnUpdate()
    //{
    //    //if(是否发现玩家.Value)
    //    //    return TaskStatus.Success;

    //    if (正在等待)
    //    {
    //        等待计时器 += Time.deltaTime;
    //        if (等待计时器 >= 等待时间.Value)
    //        {
    //            正在等待 = false;
    //            // 切换目标点
    //          //  当前目标 = (当前目标 == 巡逻点A.Value) ? 巡逻点B.Value : 巡逻点A.Value;
    //        }
    //        return TaskStatus.Running;
    //    }

    //    // 移动到目标点
    //    Vector2 移动方向 = (当前目标.position - transform.position).normalized;
    //    transform.position += (Vector3)移动方向 * 移动速度.Value * Time.deltaTime;

    //    // 更新朝向
    //    bool 面向左 = 移动方向.x < 0;
    //  body.SetFilp(面向左);

    //    // 检查是否到达
    //    float 距离 = Vector2.Distance(transform.position, 当前目标.position);
    //    if (距离 <= 到达距离.Value)
    //    {
    //        正在等待 = true;
    //        等待计时器 = 0f;
    //    }

    //    return TaskStatus.Running;
    //}
}
