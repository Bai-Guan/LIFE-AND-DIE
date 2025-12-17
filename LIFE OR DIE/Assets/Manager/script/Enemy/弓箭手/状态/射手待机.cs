using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 射手待机 : 弓箭手状态基类
{
    public 射手待机(弓箭手状态机 aIFsm) : base(aIFsm)
    {
    }

    private bool hasDetector = false;
    private float lastDecisionTime = 0f;

    private const float BUFFER = 1.5f;   // 缓冲

    private float blockWindow = 0.9f;   // 缓存窗口
    private float blockRemaining = 0f;  // 剩余缓存时间



    private bool statelock = false;
    public override void Attack()
    {

    }

    public override void Block()
    {

    }

    public override void Enter()
    {

        AIFsm.SetInvincible(false);
        AIFsm.动画事件中心.TriggerIdle();
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


        //此判断每个0点几秒检测一次
        if (Time.time - lastDecisionTime < AIFsm.决策间隔)
            return;
        lastDecisionTime = Time.time;




        if (AIFsm.是否为初见玩家 == false)
        {
            float dirToPlayer = AIFsm.MainPlayer.transform.position.x - AIFsm.transform.position.x;
            AIFsm.SetFacing(Mathf.Sign(dirToPlayer));   // 正数朝右，负数朝左
        }




        //检测发现玩家并且玩家没死
        if (AIFsm.射线检测.IsPlayerVisible && 玩家的全局变量.玩家是否死亡 == false)
        {
            AIFsm.body.SetBackstab(false);
            AIFsm.是否为初见玩家 = false;
            AIFsm.SwitchState(AITypeState.preblock);
          
        }
       
    }
}
