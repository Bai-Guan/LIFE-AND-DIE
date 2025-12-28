using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss冲刺攻击 :BOSS状态基类
{
    public Boss冲刺攻击(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }
    private float runTime = 0.5f;
    private float allTime = 2f;
    private float timer = 0;
    private float distance = 0.5f;

    private int dir = 0;
    private float lockX;        // 0.5 s 时锁定的最终 X
    private bool 已锁定;

    private bool 转向锁 = false;

    private Vector2 targetPos; // 冲刺的目标位置
    public override void Enter()
    {

        AIFsm.面朝玩家();
        timer = 0f;
        已锁定 = false;
        转向锁 = false;
        AIFsm.AnimtorEvent.SetBool("quick");

        // 随机选择出现在玩家左侧(-1)或右侧(1)
        dir = Random.Range(0, 2) == 0 ? -1 : 1;

        //// 计算玩家当前的位置
        //Vector2 playerPos = AIFsm.MainPlayer.transform.position;

        //// 设置目标位置：玩家当前位置的左侧或右侧distance距离
        //targetPos = new Vector2(
        //    playerPos.x + (dir * distance),
        //    AIFsm.transform.position.y // 保持Boss当前的高度
        
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

        //if (timer <= runTime )
        //{
        //    // 向目标位置移动
        //    Vector2 currentPos = AIFsm.rb.position;

        //    // 计算移动速度（线性插值）
        //    float t = timer / runTime; // 0到1的进度


        //    // 或者使用平滑移动
        //     Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, 
        //         (Vector2.Distance(currentPos, targetPos) / runTime) * Time.fixedDeltaTime);

        //    AIFsm.rb.MovePosition(newPos);
        //}
        if (timer <= runTime)
        {
            float playerX = AIFsm.MainPlayer.transform.position.x;
            float targetX = playerX + dir * distance;

            if (!已锁定 && timer + Time.fixedDeltaTime >= runTime)
            {
                lockX = targetX;
                已锁定 = true;
            }

            float chaseX = 已锁定 ? lockX : targetX;
            Vector2 p = AIFsm.transform.position;


            p.x = Mathf.MoveTowards(p.x, chaseX,
                    Mathf.Abs(chaseX - p.x) * (Time.fixedDeltaTime / runTime));
            AIFsm.rb.MovePosition(p);
        }
    }

    public override void Update()
    {
    timer += Time.deltaTime;
        if (timer > runTime&&转向锁==false)
        {
            转向锁 = true;
            AIFsm.面朝玩家();
        }
        if (timer > allTime)
        {
            AIFsm.SwitchState(BOSSAITypeState.ldle);
        }
    }


}
