
using UnityEngine;


public abstract class PlayerState
{

    protected PlayerState(PlayerControl ctx)
    {
        _ctx = ctx;
        this.gameObject = ctx.gameObject;

    }


    public virtual void Enter()
    {
        _rigidbody = _ctx.GetRigidbody();
        _vector = _ctx.GetVector2();
    }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }


  

    protected readonly PlayerControl _ctx;
    protected GameObject gameObject;
    bool canDoubleJump = true;

    protected Rigidbody2D _rigidbody;
    protected Vector2 _vector;
}

public class IdleState:PlayerState 
{
    public IdleState(PlayerControl ctx):base(ctx) 
    {
        
    }
    public override void Enter() 
    {
       base.Enter();
        //设置动画变量 重置状态 
        Debug.Log("进入待机状态");
        _ctx.Anim.TriggerIdle();
    }
    public override void Update() 
    {
        if (Mathf.Abs(_ctx.h) > 0.1f && _ctx.IsGrounded())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.run);
            return;
        }
        if (_ctx.KeyDownJump==true && _ctx.GetJump()==true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
            return;
        }
        
        //TODO:检测下落状态

       
    }
    public override void FixedUpdate()
    {
        _ctx.XStop();
       
    }
    public override void Exit() 
    {
    
    }
   
}

public class RunState : PlayerState
{
    public RunState(PlayerControl ctx) : base(ctx)
    {
        _rigidbody = _ctx.GetRigidbody();
        _vector = _ctx.GetVector2();
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("进入跑步状态");
        _ctx.Anim.TriggerRUN();
    }
    public override void Update()
    {
        //进入默认状态的条件
        if (Mathf.Abs(_ctx.h) <=0.1 && _ctx.IsGrounded() == true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
            return;
        }
        //进入跳跃状态的条件
        if (_ctx.KeyDownJump == true  && _ctx.GetJump() == true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
            return;
        }
        //检测朝向
        _ctx.CheckFill();


    }
    public override void FixedUpdate()
    {

        //进行了移动

        _ctx.XMove();
       
    }
    public override void Exit()
    {

    }
   



}
public class FallState : PlayerState
{
    public FallState(PlayerControl ctx) : base(ctx)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("进入下坠状态");
        _ctx.Anim.TriggerFall();
    }
    public override void Update()
    {
        //速度y为0 且脚下为地面 则视为落地 进入待机状态
        if(_rigidbody.velocity.y < 0.05f&&_ctx.IsGrounded()==true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        }
        //检测朝向
        _ctx.CheckFill();
    }
    public override void FixedUpdate()
    {
        _ctx.XMove();
    }
    public override void Exit()
    {

    }



}
//只要人在往上 就是上升状态
public class JumpState : PlayerState
{
    public JumpState(PlayerControl ctx) : base(ctx)
    {
        //
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("进入跳跃状态");
        _ctx.Anim.TriggerJump();
        //进行角色跳跃操作
        Jump();
        //使得普通跳跃次数消耗
        _ctx.setJump(false);//恢复部分在碰撞实现

        //跳跃前的水平速度清零
        _rigidbody.velocity=new Vector2(0,_rigidbody.velocity.y);

        

    }
    public override void Update()
    {
        //速度==0进入待机?

   //速度<0则为下坠
        if(_rigidbody.velocity.y<0)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.fall);
        }
        //按键W二段跳

        //检测朝向
        _ctx.CheckFill();
    }
    public override void FixedUpdate()
    {
        //如果一直按住了跳跃 则y速度将一直加某一个小于重力的值 使得跳跃滞空时间变长
    if(_ctx.KeyDownJump==true)
        {
            Vector2 up = new Vector2(0, 10f);
            _rigidbody.AddForce(up);
            
        }
        //进行了空中移动
        _ctx.XMove();
    }
    public override void Exit()
    {

    }
   private void Jump()
    {
        if (_ctx.GetJump() == true)
        {
            float newY = _ctx.jumpSpeed;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, newY);
        }
    }

}


public class HitState : PlayerState
{
    public HitState(PlayerControl ctx) : base(ctx)
    {

    }
    public override void Enter()
    {

    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {

    }

}
public class OtherState : PlayerState
{
    public OtherState(PlayerControl ctx) : base(ctx)
    {

    }
    public override void Enter()
    {

    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {

    }

}

