using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss二阶段待机 :BOSS状态基类
{
    public Boss二阶段待机(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }
    private float timer = 0;
    private float 判断时间 = 0.4f;
    private float 距离中心距离 = 3f;
    public override void Enter()
    {
        AIFsm.body.SetRBvelcoity(new Vector2(0, 0));
        AIFsm.僵直条.加减僵直条(-1f);
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
        AIFsm.CheckRb();

        if (玩家的全局变量.玩家是否死亡 == true) return;
        timer += Time.deltaTime;
        if (timer < 判断时间) return;
        timer = 0;
        float random = Random.Range(0f, 1f);
        //0.2概率砸地 0.3概率跳中间 0.5概率跑
        if (random < 0.2f)
        {
            AIFsm.SwitchState(BOSSAITypeState.jumpAttack);
            return;
        }
        else if (random <= 0.6 && random >= 0.2 && Mathf.Abs(AIFsm.transform.position.x - 20.4f) >= 距离中心距离)
        {
            AIFsm.SwitchState(BOSSAITypeState.jumpMid);
            return;
        }
        else if (random > 0.6f)
        {
            AIFsm.SwitchState(BOSSAITypeState.run);
            return;
        }
    }
}
