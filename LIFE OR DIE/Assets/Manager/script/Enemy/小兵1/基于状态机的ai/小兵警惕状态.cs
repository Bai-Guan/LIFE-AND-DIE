using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 小兵警惕状态 : 小兵状态基类
{
    public 小兵警惕状态(小怪状态机AI aIFsm) : base(aIFsm)
    {

    }
    private int phase = 0;   // 0-跑近 2m 1-等2s 2-背退5m 3-转身待机
    private float timer = 0f;
    private Vector3 startPos;
    private const float RunRange = 3f;
    private const float BackDist = 5f;

    private float DirToPlayer =>
       (AIFsm.MainPlayer.transform.position.x - AIFsm.transform.position.x) > 0 ? 1f : -1f;
    public override void Enter()
    {
        phase = 0; timer = 0f;
        AIFsm.body.SetBackstab(false);
    }

    public override void Update()
    {
        timer += Time.deltaTime;
     
        float dist = AIFsm.水平距离玩家距离;

        switch (phase)
        {
            case 0: // 跑近到 2m
                if (dist <= RunRange)
                    GotoWait();
                else
                {
                    AIFsm.SetFacing(DirToPlayer);
                    AIFsm.rb.velocity = new Vector2(DirToPlayer * AIFsm.跑步移动速度 * 2, AIFsm.rb.velocity.y);
                    AIFsm.动画事件中心.TriggerRun();
                }
                break;

            case 1: // 等 2s
                AIFsm.rb.velocity = Vector2.zero;
                AIFsm.动画事件中心.TriggerIdle();
                if (timer >= 2f) GotoBack();
                break;

            case 2: // 背退 5m
                if (Vector3.Distance(AIFsm.transform.position, startPos) >= BackDist)
                    GotoTurn();
                else
                {
                    AIFsm.动画事件中心.TriggerRun();
                    AIFsm.SetFacing(-DirToPlayer);
                    AIFsm.rb.velocity = new Vector2(-DirToPlayer * AIFsm.跑步移动速度*1.2f, AIFsm.rb.velocity.y);
                    
                }
                break;

            case 3: // 已转身，玩家一复活就冲
               
                if (AIFsm.射线检测.IsPlayerVisible&&玩家的全局变量.玩家是否死亡==false)
                {
                   
                    AIFsm.SwitchState(AITypeState.run);
                }    
                   
                break;
        }
        }
    public override void Attack()
    {

    }

    public override void Block()
    {

    }

 

  

    public override void FixedUpdate()
    {

    }
    private void GotoWait() { phase = 1; timer = 0f; }
    private void GotoBack() { phase = 2; timer = 0f; startPos = AIFsm.transform.position; AIFsm.动画事件中心.TriggerRun(); }
    private void GotoTurn() { phase = 3; timer = 0f; AIFsm.rb.velocity = Vector2.zero; AIFsm.SetFacing(DirToPlayer);
        AIFsm.动画事件中心.TriggerIdle(); 
    }

    public override void Exit() => AIFsm.rb.velocity = Vector2.zero;

}
