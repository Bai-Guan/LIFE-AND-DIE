using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    private InitWeaponSystem weapon;
    PlayerControl playerControl;
    AnimationEventHandler eventHandler;
    bool isExiting = false;

    public AttackState(PlayerControl ctx) : base(ctx)
    {
        playerControl = ctx;
    }

    public override void Enter()
    {
        Debug.Log("进入攻击模式");
        weapon = playerControl.weapon;
        eventHandler = weapon.EventHandler;
        _ctx.SetRB_X(0);
        isExiting = false;
        eventHandler.OnFinish += Exit;

        weapon.Enter();

    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        if (isExiting) return;
        isExiting = true;

        eventHandler.OnFinish -= Exit;
        playerControl.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        // playerControl.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        // TimeManager.Instance.LaterOneFrame(()=> playerControl.SwitchStatus(PlayerControl.PlayerStatus.ldle));

    }
 
    private void Attack()
    {

    }
}
//public class AttackState : PlayerState
//{
//    ////先写死在逻辑里吧 不做多攻击模组 先单纯3连击得了


//    ////攻击时运行轻微移动 攻击时允许随时闪避和格挡 但攻击时不允许跳跃 跳跃时候允许下劈（待做）
//    ////有一个计时器 在X秒内进行攻击操作 就可以进入下一个攻击动画 每段攻击伤害可定义
//    ////若x秒后未进行攻击，则自然退出攻击模式到待机状态
//    ////进入攻击状态时候
//    //public int attackTimes = 0;
//    //bool isAttacking = false;
//    //private float waitTime1 = 0.5f;
//    //private float waitTime2 = 0.7f;
//    //private float timer = 0;
//    //float moveMulNumber = 0.15f;//数值小于1 用于限制攻击时候的移速
//    //Dictionary<int, Action> actions = new Dictionary<int, Action>();

//    //private float attackTime1 = 0.45f;
//    //private float attackTime2 = 0.45f;
//    //private float attackTime3 = 0.6f;


//    public AttackState(PlayerControl ctx) : base(ctx)
//    {

//    }

//    public override void Enter()
//    {
//        Debug.Log("进入连招模式");

//        //timer = 0;
//        //attackTimes = 0;
//        //isAttacking = true;
//        ////水平速度清零
//        //_ctx.SetRB_X(0);
//        ////进入连招状态
//        //_ctx.isAttacking = true;

//        ////设置连招动画
//        //actions.Add(0, _ctx.Anim.TriggerAttack1);
//        //actions.Add(1, _ctx.Anim.TriggerAttack2);
//        //actions.Add(2, _ctx.Anim.TriggerAttack3);

//        //Attack();

//    }
//    public override void Update()
//    {
//        //timer += Time.deltaTime;



//        //if(attackTimes < 2 && _ctx.IsKeyDownAttack == true && timer >= attackTime1)
//        //{
//        //    Attack();
//        //    timer = 0;
//        //}

//        //if (attackTimes >= 2 && _ctx.IsKeyDownAttack == true && timer >= attackTime1)
//        //{
//        //    Attack();
//        //    timer = 0;
//        //}



//        ////检测朝向
//        //_ctx.CheckFill();
//        //if (timer >waitTime1&&attackTimes<2)
//        //{
//        //    _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
//        //}
//        //else if (timer > waitTime2 && attackTimes >= 2)
//        //{
//        //    _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
//        //}

//    }
//    public override void FixedUpdate()
//    {
//        //_ctx.XMove(moveMulNumber);
//    }
//    public override void Exit()
//    {
//        //actions.Clear();
//        //_ctx.isAttacking = false;
//    }

//    private void Attack()
//    {
//        //actions[attackTimes]?.Invoke();
//        //attackTimes++;
//        //attackTimes = attackTimes%3;
//    }
//}