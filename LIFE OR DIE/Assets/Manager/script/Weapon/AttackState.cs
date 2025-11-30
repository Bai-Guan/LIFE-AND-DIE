using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState
{
    private InitWeaponSystem weapon;
   NewPlayerControll _ctx;
    AnimationEventHandler eventHandler;
    bool isExiting = true;

    public AttackState(NewPlayerControll ctx) 
    {
        _ctx=ctx;
    }

    public  void Enter()
    {

        if(isExiting==false) 
        {
            _ctx.SwitchState(TypeState.ldle);
            return;
        }
        Debug.Log("进入攻击模式");
        weapon = _ctx.weapon;
        eventHandler = weapon.EventHandler;
        _ctx.rb.velocity=new Vector2(0,0);
        isExiting = false;
        eventHandler.OnFinish += Exit;

        weapon.Enter();

    }
    public  void Update()
    {

    }
    public  void FixedUpdate()
    {

    }
    public void Exit()
    {
        if (isExiting) return;
        isExiting = true;

        eventHandler.OnFinish -= Exit;
        _ctx.SwitchState(TypeState.ldle);
       // TimeManager.Instance.OneTime(0.02f, ()=>playerControl.SwitchStatus(PlayerControl.PlayerStatus.ldle));
       
        // playerControl.SwitchStatus(PlayerControl.TypeState.ldle);
        // TimeManager.Instance.LaterOneFrame(()=> playerControl.SwitchStatus(PlayerControl.TypeState.ldle));

    }
 




    public void Dodge()
    {
        
    }

    public void ContractPower()
    {
        
    }

    public void Attack()
    {
       
    }

    public void Block()
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
//        //    _ctx.SwitchStatus(PlayerControl.TypeState.ldle);
//        //}
//        //else if (timer > waitTime2 && attackTimes >= 2)
//        //{
//        //    _ctx.SwitchStatus(PlayerControl.TypeState.ldle);
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