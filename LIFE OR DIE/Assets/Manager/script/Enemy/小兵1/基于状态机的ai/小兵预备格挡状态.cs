using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 小兵预备格挡状态 : 小兵状态基类
{
    public 小兵预备格挡状态(小怪状态机AI aIFsm) : base(aIFsm)
    {
    }
    private float waitTimer = 1.5f;          // 最长预备时间
    private bool playerHit = false;

    public override void Enter()
    {
        waitTimer = 2f;
        playerHit = false;
        AIFsm.动画事件中心.TriggerWillBlock(); // 摆架势动画
        AIFsm.SetInvincible(true);
    }

    public override void Update()
    {
        // 1. 倒计时
        waitTimer -= Time.deltaTime;

        // 2. 监听玩家攻击（任意帧）
        if (PlayerAttackTrigger.AttackPressed)
        {
            PlayerAttackTrigger.AttackPressed = false;
            playerHit = true;
        }

        // 3. 分支出口
        if (playerHit)                       // 玩家出招 → 真正格挡
        {
            AIFsm.SwitchState(AITypeState.block);
        }
        else if (waitTimer <= 0)             // 超时 → 回待机
        {
            AIFsm.SwitchState(AITypeState.ldle);
        }
    }

    public override void Attack()
    {

    }

    public override void Block()
    {

    }

 

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    
}
