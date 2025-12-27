using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS移动 : BOSS状态基类
{
    private float _curSpeed = 0f;
    private const float AccelPerSec = 2f;

    private float timer1=0f;
    public BOSS移动(BOSSAI控制器 fsm) : base(fsm) { }

    public override void Enter()
    {
        _curSpeed = 0f;
        AIFsm.rb.velocity = Vector2.zero;
        AIFsm.AnimtorEvent.SetBool("run");
    }

    public override void Exit() { }

    public override void Update()
    {
        timer1 += Time.deltaTime;
        if(timer1<=AIFsm.决策间隔)
        {
            return;
        }
        AIFsm.面朝玩家();
        timer1= 0f;
        if (!AIFsm.isTwoPhase)
        {
            //1阶段路线
            if(AIFsm.水平距离玩家距离<=AIFsm.五连击距离)
            {
                AIFsm.SwitchState(BOSSAITypeState.fiveAttack);
                return;
            }

        }
        else
        {
            //2阶段路线
        }
       
    }

    public override void FixedUpdate()
    {
        if (_curSpeed < AIFsm.最大移动速度)
        {
            _curSpeed += AccelPerSec * Time.fixedDeltaTime;
            _curSpeed = Mathf.Min(_curSpeed, AIFsm.最大移动速度);
        }

       
        AIFsm.rb.velocity = new Vector2(_curSpeed * AIFsm.ReturnDirToPlayer(), AIFsm.rb.velocity.y);
        AIFsm.SetFacing(AIFsm.ReturnDirToPlayer());
    }

  
    

}
