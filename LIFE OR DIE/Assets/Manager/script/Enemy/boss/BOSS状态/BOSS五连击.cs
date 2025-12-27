using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS五连击 : BOSS状态基类
{
    public BOSS五连击(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }

    public override void Enter()
    {
        AIFsm.AnimtorEvent.SetBool("FiveAttack");
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
       
    }

    public override void Update()
    {
    
    }


}
