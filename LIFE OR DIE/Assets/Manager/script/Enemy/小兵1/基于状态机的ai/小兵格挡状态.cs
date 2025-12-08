using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class 小兵格挡状态 : 小兵状态基类
{
    public 小兵格挡状态(小怪状态机AI aIFsm) : base(aIFsm)
    {
    }
    private float timer = 0;
    public override void Attack()
    {
      
    }

    public override void Block()
    {
      
    }

    public override void Enter()
    {
       
        AIFsm.僵直条.清空僵直条();
        AIFsm.动画事件中心.TriggerBlock();
    }

    public override void Exit()
    {
        timer = 0;
        AIFsm.SetInvincible(false);
    }

    public override void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
     
    }

    public override void Update()
    {
        if (timer >= 1f)
        {
            AIFsm.SetInvincible(false);
            AIFsm.SwitchState(AITypeState.ldle);
        }

    }
}
