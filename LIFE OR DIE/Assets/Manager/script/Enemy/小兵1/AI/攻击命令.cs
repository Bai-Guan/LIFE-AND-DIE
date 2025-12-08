using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 攻击命令 : Action
{

    private 小怪动画事件 animationManager;
    public SharedString 攻击名称;
    public SharedBool FacingLeft;
    public SharedFloat 攻击持续时间 = 3f;

    // 状态变量
    private bool attackStarted = false;
    private float attackStartTime;
    private bool isAttackFinished = false;

    private EnemyRigidbar EnemyRigidbar;
    private BehaviorTree bt;

    public override void OnAwake()
    {
        animationManager = GetComponent<小怪动画事件>();
        EnemyRigidbar = GetComponent<EnemyRigidbar>();
        bt = GetComponent<BehaviorTree>();
    }

    public override void OnStart()
    {
      //  Debug.Log("update前的start");
        // 重置状态
        attackStarted = true;
        isAttackFinished = false;
        attackStartTime = Time.time;

        // 通知行为树正在攻击
        bt.SetVariable("在攻击", (SharedBool)true);

        // 播放攻击动画
        if (animationManager != null)
        {
          //  animationManager.PlayAttackAnimation(攻击名称.Value);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (animationManager == null)
        {
            Debug.Log("小怪动画事件捕获失败");
            return TaskStatus.Failure;
        }
   

        // 检查是否被打断（僵直）
        if (EnemyRigidbar != null && EnemyRigidbar.检测是否僵直())
        {
            Debug.Log("攻击被打断！");
            // 被打断，结束攻击
            OnAttackInterrupted();
            return TaskStatus.Failure;
        }

        // 检查攻击是否超时
        if (Time.time - attackStartTime >= 攻击持续时间.Value)
        {
            isAttackFinished = true;
            bt.SetVariable("在攻击", (SharedBool)false);
            return TaskStatus.Success;
        }

        // 攻击仍在进行中
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        // 无论成功还是失败，都需要清理状态
        if (isAttackFinished || !attackStarted)
        {
            // 正常结束攻击
            bt.SetVariable("在攻击", (SharedBool)false);
        }

        attackStarted = false;
        isAttackFinished = false;
    }

    private void OnAttackInterrupted()
    {
        // 被打断时的清理
        attackStarted = false;
        isAttackFinished = false;
        bt.SetVariable("在攻击", (SharedBool)false);

        // 可以在这里添加被打断时的动画处理
        // 例如：animationManager.StopAttackAnimation();
    }
}

