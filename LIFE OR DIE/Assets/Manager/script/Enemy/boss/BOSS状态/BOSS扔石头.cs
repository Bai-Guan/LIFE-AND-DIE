using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS扔石头 : BOSS状态基类 
{
    public BOSS扔石头(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }
    private float firstX = 20f;
    private float firstY = -26.7f;
    private float timer = 0;
    private float 前摇 = 0.5f;
    private float 总共 = 1f;
    private bool clock=false;
    public override void Enter()
    {
        clock = false;
        timer = 0;
        AIFsm.AnimtorEvent.SetBool("throw");
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer>=前摇&&timer<总共)
        {
            ThrowRoll();
        }
        if(timer>=总共)
        {
            AIFsm.SwitchState(BOSSAITypeState.ldle);
        }

    }

    private void ThrowRoll()
    {
        if (clock == false)
        {
            clock = true;
         GameObject temp=   GameObject.Instantiate(AIFsm.Boss扔出的石头);
            temp.transform.position = new Vector3(firstX, firstY,0);  
            temp.GetComponent<Boss扔出的石头>().设置方向((int)AIFsm.ReturnDirToPlayer());
        }
    }
 
}
