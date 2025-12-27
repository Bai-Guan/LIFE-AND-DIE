using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS待机 : BOSS状态基类
{
    public BOSS待机(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }

    public override void Enter()
    {
        AIFsm.body.SetRBvelcoity(new Vector2(0,0));
        AIFsm.僵直条.清空僵直条();
        AIFsm.AnimtorEvent.SetBool("Idle");
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        if(AIFsm.body.CurrentHP<=AIFsm.body.MaxHp/2)
        {
            AIFsm.isTwoPhase = true;
            AIFsm.SwitchState(BOSSAITypeState.phaseTwoStandby);
            return;
        }
        AIFsm.CheckRb();

        if(玩家的全局变量.玩家是否死亡==true)return;

      float random=  Random.Range(0f,1f);    
        //0.3概率砸地 0.7概率移动
        if(random<=0.3f)
        {
           // AIFsm.SwitchState(BOSSAITypeState.jumpAttack);
            return;
        }
        else if(random>0.7f)
        {
            AIFsm.SwitchState(BOSSAITypeState.run);
            return;
        }
    }
}
