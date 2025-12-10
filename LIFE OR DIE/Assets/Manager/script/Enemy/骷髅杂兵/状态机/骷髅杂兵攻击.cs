using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 骷髅杂兵攻击 : 骷髅杂兵状态基类
{ 
    public 骷髅杂兵攻击(骷髅杂兵状态机 aIFsm) : base(aIFsm)
    {
    }

    private float duringTime = 2.5f;
    private float timer = 0f;
    public override void Attack()
    {

    }

    public override void Block()
    {

    }

    public override void Enter()
    {
        timer = 0f;
        AIFsm.动画事件中心.TriggerAttack();
    }

    public override void Exit()
    {
        timer = 0f;
    }

    public override void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }

    public override void Update()
    {
        AIFsm.CheckRb();


        if (timer > duringTime)
        {
            timer = 0;
            AIFsm.SwitchState(AITypeState.ldle);
        }
    }
}
