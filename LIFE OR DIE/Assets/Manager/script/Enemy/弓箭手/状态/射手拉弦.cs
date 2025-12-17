using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 射手拉弦 : 弓箭手状态基类
{
    public 射手拉弦(弓箭手状态机 aIFsm) : base(aIFsm)
    {
    }
    private float 拉弦前摇1=1.5f;
    private float 拉弦前摇2 = 3f;
    float 拉弦前摇;
    private float 最大瞄准时间 = 5f;
    private bool 是否上弦=false;

    private float timer2 = 0f;
    private float 检测间隔 = 1f;
    private Vector2 上次玩家位置;

    private float playerMaxDis = 6f;
    private float timer = 0;

    
    public override void Attack()
    {

    }

    public override void Block()
    {

    }

    public override void Enter()
    {
        AIFsm.动画事件中心.TriggerReady();
        AudioManager.Instance.PlaySFX("拉弦");
        是否上弦 = false;
        timer = 0;
        timer2= 0f;
         拉弦前摇 = Random.Range(拉弦前摇1, 拉弦前摇2);
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
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;



        if (AIFsm.是否为初见玩家 == false)
        {
            float dirToPlayer = AIFsm.MainPlayer.transform.position.x - AIFsm.transform.position.x;
            AIFsm.SetFacing(Mathf.Sign(dirToPlayer));   // 正数朝右，负数朝左
        }

        if (timer >= 拉弦前摇)
            是否上弦 = true;
        if (timer2>=检测间隔)
        {
            timer2 = 0;
         float 玩家位移 = Vector2.Distance(AIFsm.射线检测.PlayerPosition, 上次玩家位置);

            //如果玩家位移很小 则射击
            if(玩家位移<=playerMaxDis&&是否上弦==true)
            {
                AIFsm.SwitchState(AITypeState.attack);
            }

           

            上次玩家位置 = AIFsm.射线检测.PlayerPosition;
        }
      
    }
}
