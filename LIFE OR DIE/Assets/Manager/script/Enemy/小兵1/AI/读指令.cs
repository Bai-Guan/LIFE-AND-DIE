using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 读指令 : Conditional
{
    public override TaskStatus OnUpdate()
    {
        if (PlayerAttackTrigger.AttackPressed)
        {
            PlayerAttackTrigger.AttackPressed = false; // 消费掉
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
public static class PlayerAttackTrigger
{
    public static bool AttackPressed;   
}