using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class 小兵松懈状态 :小兵状态基类
{
    public 小兵松懈状态(小怪状态机AI aIFsm) : base(aIFsm)
    {
    }
    private int phase = 0;   // 0-跑近 1-等2s 2-背走3m 3-停留10s 4-转身0.3s
    private float timer = 0f;
    private Vector3 startPos;          // 用于背走测距
    private const float RunRange = 3f; // 跑近目标
    private const float BackDist = 7f;
    private const float StayTime = 10f;
    private const float TurnTime = 0.3f;
 
    private float DirToPlayer =>
        (AIFsm.MainPlayer.transform.position.x -AIFsm.transform.position.x) > 0 ? 1f : -1f;
    public override void Attack()
    {

    }

    public override void Block()
    {

    }

    public override void Enter()
    {
        phase = 0; timer = 0f;
        startPos = AIFsm.transform.position;
        AIFsm.是否追击=false;
        AIFsm.body.SetBackstab(true);
    }

    public override void Exit()
    {
        AIFsm.rb.velocity = Vector2.zero;
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        timer += Time.deltaTime;
        float dist = AIFsm.水平距离玩家距离;

        switch (phase)
        {
            case 0: // 跑近到 2m 内
                if (dist <= RunRange)
                    GotoWait();
                else
                {
                    AIFsm.SetFacing(DirToPlayer);
                    //int dir = AIFsm.body.isFacingLeft ? -1 : 1;
                    AIFsm.rb.velocity = new Vector2(DirToPlayer * AIFsm.跑步移动速度, AIFsm.rb.velocity.y);
                    AIFsm.动画事件中心.TriggerRun();

                    if (AIFsm.射线检测.IsPlayerVisible && 玩家的全局变量.玩家是否死亡 == false)
                    {

                        AIFsm.是否为初见玩家 = false;

                        AIFsm.是否追击 = true;
                        AIFsm.SwitchState(AITypeState.ldle);
                    }
                }
                break;

            case 1: // 等 2 秒（待机动画）
                AIFsm.rb.velocity = Vector2.zero;
                AIFsm.动画事件中心.TriggerIdle();
                if (timer >= 2f) GotoBackWalk();
                break;

            case 2: // 背走 3 米
                if (Vector3.Distance(AIFsm.transform.position, startPos) >= BackDist)
                    GotoStay();
                else
                {
                    AIFsm.body.SetBackstab(true);
                    AIFsm.SetFacing(-DirToPlayer); // 背对
                  //  int dir = AIFsm.body.isFacingLeft ? -1 : 1;
                    AIFsm.rb.velocity = new Vector2(-DirToPlayer * AIFsm.跑步移动速度, AIFsm.rb.velocity.y);
                    AIFsm.动画事件中心.TriggerRun();
                }
                break;

            case 3: // 停留 10 秒
                AIFsm.rb.velocity = Vector2.zero;
                AIFsm.动画事件中心.TriggerIdle();
                if (timer >= StayTime) GotoTurn();
                break;

            case 4: // 转身 0.3 秒
               
                if (AIFsm.射线检测.IsPlayerVisible&&玩家的全局变量.玩家是否死亡==false)
                { 
                AIFsm.SwitchState(AITypeState.run);
               
                    return;
                 }
                break;
        }
    }

    private void GotoWait() { phase = 1; timer = 0f; }
    private void GotoBackWalk() { phase = 2; timer = 0f; startPos = AIFsm.transform.position; }
    private void GotoStay() { phase = 3; timer = 0f; AIFsm.rb.velocity = Vector2.zero; }
    private void GotoTurn() { phase = 4; timer = 0f; AIFsm.SetFacing(DirToPlayer);  }

}
