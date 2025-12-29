using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS五连击 : BOSS状态基类
{
    public BOSS五连击(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }
    private float timer = 0;
    public override void Enter()
    {
        AIFsm.AnimtorEvent.SetBool("FiveAttack");
        AIFsm.僵直条.加减僵直条(-2);
    }

    public override void Exit()
    {
        timer = 0;
        AIFsm.僵直条.清空僵直条();
    }

    public override void FixedUpdate()
    {
        AIFsm.面朝玩家();
        if (timer <= 2.5f && AIFsm.水平距离玩家距离 > 0.5f) 
        AIFsm.rb.velocity = new Vector2(AIFsm.ReturnDirToPlayer() * 0.2f,AIFsm.rb.velocity.y);
    }

    public override void Update()
    {
        AIFsm.CheckRb();
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            if(!AIFsm.isTwoPhase)
            AIFsm.SwitchState(BOSSAITypeState.ldle);
            else
                AIFsm.SwitchState(BOSSAITypeState.phaseTwoStandby);
        }
    }


}
