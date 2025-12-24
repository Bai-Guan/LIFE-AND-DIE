using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 骷髅杂兵走路 :骷髅杂兵状态基类
{
    public 骷髅杂兵走路(骷髅杂兵状态机 aIFsm) : base(aIFsm)
    {
    }

    private float lastDecisionTime = 0f;
    int 朝向 = 1;
   

    public override void Attack()
    {

    }

    public override void Block()
    {

    }

    public override void Enter()
    {
        if (AIFsm.是否追击)
        {
            float temp = AIFsm.transform.position.x - AIFsm.MainPlayer.transform.position.x;
            朝向 = temp > 0 ? -1 : 1;

            AIFsm.SetFilp(朝向);
        }
        AIFsm.动画事件中心.TriggerRun();
    }

    public override void Exit()
    {
        AIFsm.rb.velocity = new Vector2(0, AIFsm.rb.velocity.y);
    }

    public override void FixedUpdate()
    {
        if (AIFsm.水平距离玩家距离 > AIFsm.靠近玩家最近距离)
        {
            int dir = AIFsm.body.isFacingLeft ? -1 : 1;
            AIFsm.rb.velocity = new Vector2(AIFsm.跑步移动速度 * dir, AIFsm.rb.velocity.y);

        }
        if (AIFsm.水平距离玩家距离 < AIFsm.靠近玩家最近距离)
        {
            AIFsm.rb.velocity = new Vector2(0, AIFsm.rb.velocity.y);
        }


    }

    public override void Update()
    {


        AIFsm.CheckRb();

        //此判断每个0点几秒检测一次
        if (Time.time - lastDecisionTime < AIFsm.决策间隔)
            return;
        lastDecisionTime = Time.time;





      
            if (AIFsm.是否为初见玩家 == false)
            {
                float dirToPlayer = AIFsm.MainPlayer.transform.position.x - AIFsm.transform.position.x;
                AIFsm.SetFacing(Mathf.Sign(dirToPlayer));   // 正数朝右，负数朝左
            }
        


        //检测
        if (AIFsm.射线检测.IsPlayerVisible)
        {
            AIFsm.body.SetBackstab(false);
            AIFsm.脱战计时 = 0;
            AIFsm.是否追击 = true;
            
            //敌人不突刺且玩家贴脸情况下 进行读指令格挡或者近身斩击
            if (AIFsm.水平距离玩家距离 <= AIFsm.靠近玩家最近距离)
            {

                AIFsm.SwitchState(AITypeState.ldle);
                return;
            }
        }
        else
        {
            AIFsm.脱战计时 += Time.deltaTime;
            if (AIFsm.脱战计时 >= AIFsm.脱战所需要的时)
            {
                Debug.Log("爷不追了");
                AIFsm.是否追击 = false;
                AIFsm.body.SetBackstab(true);
            }
        }
    }
}
