using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 身体发光预警 : Action
{
    public override void OnAwake()
    {
    
    }
    public override void OnStart()
    {
        
    }
    public override TaskStatus OnUpdate()
    {
      

        return TaskStatus.Failure;
    }
}
