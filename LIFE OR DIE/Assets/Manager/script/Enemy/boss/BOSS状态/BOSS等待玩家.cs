using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BOSS等待玩家 :BOSS状态基类
{
    private bool 是否发现玩家 = false;
    private float timer = 0;
    private bool clock = false;
    public BOSS等待玩家(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }

    public override void Enter()
    {
        是否发现玩家 = false;
        timer = 0;
    }

    public override void Exit()
    {
        AudioManager.Instance.PlayMusic("冰汽时代");
    }

    public override void FixedUpdate()
    {
        if(AIFsm.射线检测.IsPlayerVisible)
            是否发现玩家 = true;
        if(是否发现玩家==true&&clock==false)
        {
            AIFsm.显示BossUI();
            clock = true;
        }
 
    }

    public override void Update()
    {
        if(是否发现玩家)
        {
            timer += Time.deltaTime;
            if (timer > 2f)
            {
                AIFsm.SwitchState(BOSSAITypeState.ldle);
                return;
            }
        }
        
    }

   
}
