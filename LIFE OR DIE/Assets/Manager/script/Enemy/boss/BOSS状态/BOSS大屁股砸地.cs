using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS大屁股砸地 : BOSS状态基类
{
    public BOSS大屁股砸地(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }
    private const float 上升距离 = 8f;
    private float 目标位置X;
    private float 目标位置Y;

    private DamageData dropDamage = new DamageData()
    {
        atk = 400,
        hitType = HitType.heavy,

    };

    private const float 上升时间 = 0.5f;
    private const float 等待时间 = 0.7f;
    private const float 下坠时间 = 0.3f;
    private const float 后摇时间 = 2.5f;
    private float timer =0;
    private enum 砸地阶段 {上升,等待,下坠,后摇 }
    private 砸地阶段 current=砸地阶段.上升;
    public override void Enter()
    {
        current = 砸地阶段.上升;
        目标位置Y =AIFsm.transform.position.y+上升距离;
    }

 

    public override void Exit()
    {
      
    }

   

    public override void FixedUpdate()
    {
        switch (current)
        {
            case 砸地阶段.上升:

                break;

            case 砸地阶段.等待:

                break;
            case 砸地阶段.下坠:

                break;
            case 砸地阶段.后摇:

                break;

        }
    }

    public override void Update()
    {
        timer += Time.deltaTime;
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
        }
    }
    private void 下坠()
    {

    }
    private void 后摇()
    {

    }
}
