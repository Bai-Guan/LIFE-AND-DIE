using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 骷髅杂兵受伤 : 骷髅杂兵状态基类
{
    public 骷髅杂兵受伤(骷髅杂兵状态机 aIFsm) : base(aIFsm)
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
        timer = 0;
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > AIFsm.僵直时间)
        {
            AIFsm.SwitchState(AITypeState.ldle);
        }
    }
}
