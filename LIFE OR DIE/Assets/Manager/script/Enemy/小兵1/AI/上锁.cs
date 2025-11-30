using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Threading;
using UnityEngine;

public class 上锁 : Action
{
    public SharedFloat 上锁时间=6f;
 
    private bool islocked=false;
    public override void OnAwake()
    {
        base.OnAwake();
    }
    public override void OnStart()
    {
        base.OnStart();
    }
    public override TaskStatus OnUpdate()
    {
        if (islocked == false)
        {
            islocked = true;
            TimeManager.Instance.OneTime(上锁时间.Value, () =>
            {
                islocked = false;
            });
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
