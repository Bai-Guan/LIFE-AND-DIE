using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 设置敌人无敌 : Action
{
    public SharedBool 是否无敌=false;
 
   private DamagedComponent _component;

    public override void OnAwake()
    {
       _component = GetComponent<DamagedComponent>();
    }
    public override TaskStatus OnUpdate()
    {

        _component.SetInvincible(是否无敌.Value);
        return TaskStatus.Success;
    }
}
