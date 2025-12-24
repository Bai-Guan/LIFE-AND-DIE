using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class 小兵硬直状态 : 小兵状态基类
{
    public 小兵硬直状态(小怪状态机AI aIFsm) : base(aIFsm)
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
        AIFsm.动画事件中心.TriggerHit();
       AIFsm.僵直条.清空僵直条();
    }

    public override void Exit()
    {
        AIFsm.僵直条.清空僵直条();
        timer = 0;
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        AIFsm.僵直条.清空僵直条();
        timer += Time.deltaTime;
        if (timer > AIFsm.僵直时间)
        {
            AIFsm.SwitchState(AITypeState.ldle);
        }
    }
}
