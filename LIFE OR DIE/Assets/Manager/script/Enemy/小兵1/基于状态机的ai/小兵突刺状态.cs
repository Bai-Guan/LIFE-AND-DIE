using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 小兵突刺状态 : 小兵状态基类
{
    public 小兵突刺状态(小怪状态机AI aIFsm) : base(aIFsm)
    {
    }
    private int 朝向 = 1;
    private float 冲刺时长 = 2.5f;
    private float scale = 0.7f;
    private float timer = 0;
    public override void Attack()
    {
   
    }

    public override void Block()
    {
       
    }

    public override void Enter()
    {
        timer = 0;
        朝向 = AIFsm.body.isFacingLeft ? -1 : 1;

        TimeManager.Instance.OneTime(0.5f,() =>
        {
            AIFsm.rb.velocity = new Vector2(AIFsm.突刺瞬时速度 * 朝向, 0);
              AudioManager.Instance.PlaySFX("挥击嗖_2");

        }
        );
        AudioManager.Instance.PlaySFX("危");
        AIFsm.动画事件中心.TriggerSpecialAttack();
      //  AIFsm.动画事件中心.OnAnimEventFireAttack("specialAttack");
      
      

        AIFsm.突刺是否在CD = true;
        //冲刺CD
        TimeManager.Instance.OneTime(AIFsm.突刺CD, () =>
        {
            AIFsm.突刺是否在CD = false;
        });
    }

    public override void Exit()
    {
        AIFsm.设置受伤倍率(1);
        AIFsm.spriteRenderer.color = Color.white;
        timer = 0;
    }

    public override void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }

    public override void Update()
    {
        if(timer>冲刺时长*scale)
        {
            AIFsm.设置受伤倍率(100);
            AIFsm.spriteRenderer.color=Color.red;
        }

     
        if (timer > 冲刺时长)
        {
            AIFsm.SwitchState(AITypeState.run);
        }
    }

 
}
