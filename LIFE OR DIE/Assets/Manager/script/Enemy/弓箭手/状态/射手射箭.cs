using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class 射手射箭 : 弓箭手状态基类
{
    public 射手射箭(弓箭手状态机 aIFsm) : base(aIFsm)
    {
    }
    private float timer = 0f;
    public override void Attack()
    {
       
    }

    public override void Block()
    {

    }

    public override void Enter()
    {
        timer = 0f;
        AIFsm.动画事件中心.TriggerAttack();
        AudioManager.Instance.PlaySFX("射箭");
        // 1. 目标抬高 1.5
        Vector2 rawTarget = AIFsm.射线检测.PlayerPosition;
        Vector2 elevatedTarget = rawTarget + Vector2.up * 1.5f;

        // 2. 再算方向
        Vector2 dir = (elevatedTarget - (Vector2)AIFsm.transform.position).normalized;

        // 3. 角度、发射
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        AIFsm.放箭(dir,angle);
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
        if (timer > 4f) AIFsm.SwitchState(AITypeState.ldle);
    }
}
