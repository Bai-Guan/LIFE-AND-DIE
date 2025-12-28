using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BOSS大屁股砸地 : BOSS状态基类
{
    public BOSS大屁股砸地(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }
    private const float 上升距离 = 8f;
    private float 目标位置X;
    private float 目标位置Y;
    private float StartY;

    //private DamageData dropDamage = new DamageData()
    //{
    //    atk = 400,
    //    hitType = HitType.heavy,

    //};

    private const float 上升时间 = 0.5f;
    private const float 等待时间 = 0.2f;
    private const float 下坠时间 = 0.2f;
    private const float 后摇时间 = 2.5f;
    private float timer =0;
    private float temp = 0;
    private enum 砸地阶段 {上升,等待,下坠,后摇 }
    private 砸地阶段 current=砸地阶段.上升;
    public override void Enter()
    {
        current = 砸地阶段.上升;
        目标位置Y =AIFsm.transform.position.y+上升距离;
        StartY=AIFsm.transform.position.y;
        temp = AIFsm.rb.gravityScale;
        AIFsm.rb.gravityScale = 0;
        AIFsm.AnimtorEvent.SetBool("jump");
        AudioManager.Instance.PlaySFX("挥击嗖_1");
    }

 

    public override void Exit()
    {
        AIFsm.rb.gravityScale = temp;
    }

   

    public override void FixedUpdate()
    {
        switch (current)
        {
            case 砸地阶段.上升:
                上升();
                break;

            case 砸地阶段.等待:
                等待();
                break;
            case 砸地阶段.下坠:
                下坠();
                break;
            case 砸地阶段.后摇:
                后摇();
                break;

        }
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        AIFsm.面朝玩家();
    }

    private void 上升()
    {
        float scale = timer / 上升时间;
        float TargetX = AIFsm.MainPlayer.transform.position.x;
        float X = Mathf.Lerp(AIFsm.transform.position.x, TargetX, scale);
        float Y = Mathf.Lerp(AIFsm.transform.position.y, 目标位置Y, scale);
        AIFsm.body.transform.position = new Vector2(X,Y); 

        if(timer>=上升时间)
        {
            timer = 0;
            current = 砸地阶段.等待;
        }
    }
    private void 等待()
    {
        if(timer>= 等待时间 )
        {
            timer = 0;
            current = 砸地阶段.下坠;
            AIFsm.CreateHitBox("下砸");
            AIFsm.AnimtorEvent.SetBool("fall");
        }
    }
    private void 下坠()
    {
        float scale = timer / 下坠时间;
        float Y = Mathf.Lerp(目标位置Y, StartY, scale);
        AIFsm.body.transform.position = new Vector2(AIFsm.body.transform.position.x, Y);
        if (timer> 下坠时间)
        {
            timer = 0;
            current = 砸地阶段.后摇;
            CameraManager.Instance.CameraShake(0.5f, 0.6f);
            AudioManager.Instance.PlaySFX("锤击");
        }
      
    }
    private void 后摇()
    {
        if(timer>= 后摇时间)
        {
            timer = 0;
            AIFsm.SwitchState(BOSSAITypeState.ldle);
        }
    }
}
