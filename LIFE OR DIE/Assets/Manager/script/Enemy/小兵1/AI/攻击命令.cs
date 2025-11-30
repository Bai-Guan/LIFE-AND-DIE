using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 攻击命令 : Action
{

    private 小怪动画事件 animationManager;
    public SharedString 攻击名称;
    public SharedBool FacingLeft;
    private bool attackStarted = false;
    private bool attackCompleted = false;

    public override void OnAwake()
    {
        animationManager = GetComponent<小怪动画事件>();
    }

    //怪物攻击可以被打断 需要引用变量判断 update中看看当前是否被攻击了，且是否僵直条满了，如果满了则立刻停止动画，跳转到受伤状态
    public override void OnStart()
    {
      
       
        attackStarted = false;
        attackCompleted = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (animationManager == null)
            return TaskStatus.Failure;

        // 触发攻击动画
        if (!attackStarted)
        {
            animationManager.PlayAttackAnimation(攻击名称.Value);
            attackStarted = true;
            return TaskStatus.Running;
        }

        // 第二步：等待攻击完成
        // 这里需要根据你的动画系统来判断攻击是否完成
        // 以下是几种可能的方法：


        if(animationManager.IsAttackComplete)
        {
            
            attackCompleted = true;
        }
           

        // 方法2：使用自定义的完成标志（需要在动画管理器中设置）
        // if (animationManager.IsAttackComplete)
        // {
        //     attackCompleted = true;
        // }

        return attackCompleted ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        // 重置状态
        attackStarted = false;
        attackCompleted = false;
    }
}

