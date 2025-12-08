using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 小兵斩击状态 : 小兵状态基类
{
    public 小兵斩击状态(小怪状态机AI aIFsm) : base(aIFsm)
    {
    }
    private float duringTime = 3f;
    private float timer=0f;
    public override void Attack()
    {
       
    }

    public override void Block()
    {
       
    }

    public override void Enter()
    {
        timer = 0f;
        AudioManager.Instance.PlaySFX("挥击嗖_3");
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
