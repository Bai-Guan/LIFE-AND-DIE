using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 小兵待机状态 : 小兵状态基类
{
    public 小兵待机状态(小怪状态机AI aIFsm) : base(aIFsm)
    {

    }
    private bool hasDetector=false;
    private float lastDecisionTime = 0f;
    
    private const float BUFFER = 1.5f;   // 缓冲

    private float blockWindow = 0.9f;   // 缓存窗口
    private float blockRemaining = 0f;  // 剩余缓存时间

    private float nextPreBlockTime = 0f;   // 全局锁：多久之后才能再 Roll 预备格挡

    private bool statelock=false;
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







        // 视野中若玩家死亡则转换
        if (AIFsm.射线检测.IsPlayerVisible && 玩家的全局变量.玩家是否死亡 == true)
        {
            if (statelock == true)
            {
                AIFsm.SwitchState(AITypeState.alert);
                return;
            }
            else
            {
                if (statelock == false)
                {
                    AIFsm.SwitchState(AITypeState.relax);
                    statelock = true;
                    return;
                }
            }


        }
        //检测发现玩家并且玩家没死
        if (AIFsm.射线检测.IsPlayerVisible&&玩家的全局变量.玩家是否死亡==false)
        {
            AIFsm.body.SetBackstab(false);
            AIFsm.脱战计时 = 0;
            AIFsm.是否追击 = true;
         


            //如果玩家在7米外 跑过去
            if (AIFsm.水平距离玩家距离>=AIFsm.突刺距离)
            {
                AIFsm.SwitchState(AITypeState.run);
                return;
            }
           

            //如果玩家在突刺范围内 则先看看满不满足突刺
             if(AIFsm.水平距离玩家距离<=AIFsm.突刺距离 && AIFsm.突刺是否在CD==false && AIFsm.rb.velocity.y == 0)
            {
                //概率判定
                if (Random.Range(0f, 1f) <= AIFsm.突刺概率)
                {
                    AIFsm.SwitchState(AITypeState.thrust);
                    return;
                }
            }
            //如果玩家在近身斩击之外
            if (AIFsm.水平距离玩家距离 > AIFsm.靠近玩家最近距离+BUFFER)
            {
                AIFsm.动画事件中心.TriggerRun();
                AIFsm.SwitchState(AITypeState.run);
                return;
            }
            //敌人不突刺且玩家贴脸情况下 进行读指令格挡或者近身斩击
            // 贴脸分支
            if (AIFsm.水平距离玩家距离 <= AIFsm.近身斩击距离)
            {
                // 时间锁 + 概率 Roll 预备格挡
                if (Time.time >= nextPreBlockTime && Random.Range(0f, 1f) <= AIFsm.格挡成功概率)
                {
                    nextPreBlockTime = Time.time + 5f;          // 5 秒内不再 Roll
                    AIFsm.SwitchState(AITypeState.preblock);    // 进入新状态
                    return;
                }

                // 没 Roll 中 → 正常攻击
                if (AIFsm.rb.velocity.y == 0)
                {
                    AIFsm.SwitchState(AITypeState.attack);
                    return;
                }
            }

        }
        else
        {
            AIFsm.脱战计时 += Time.deltaTime;
            if (AIFsm.脱战计时 >= AIFsm.脱战所需要的时)
            {
                AIFsm.是否追击 = false;
            }
        }
    }
}


  

