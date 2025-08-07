
using UnityEngine;


    public abstract class PlayerState
    {
       
        protected PlayerState(PlayerControl ctx) 
        { 
        _ctx = ctx;
        this.gameObject=ctx.gameObject;
        
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
    }
    public override void Update() 
    {
        if (Mathf.Abs(_ctx.h) > 0.1f && _ctx.IsGrounded())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.run);
            return;
        }
        if (_ctx.IsGrounded()==false && _ctx.GetJump()==true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
            return;
        }
        //TODO:检测下落状态

       
    }
    public override void FixedUpdate()
    {
        float temp = Mathf.MoveTowards(_rigidbody.velocity.x, 0, Time.fixedDeltaTime*_ctx.groundFriction);
        _rigidbody.velocity = new Vector2(temp, _rigidbody.velocity.y);
       
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
        if (_ctx.IsGrounded() == false && _ctx.GetJump() == true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
            return;
        }
        //检测朝向
        if(_ctx.h >0.1f)
        {
            _ctx.FacingRight();
        }
        if (_ctx.h < 0.1f)
        {
            _ctx.FacingLeft();
        }


    }
    public override void FixedUpdate()
    {
        
        //进行了移动
      
            float newX = Mathf.MoveTowards(_rigidbody.velocity.x, _ctx.moveSpeed * _ctx.h, Time.fixedDeltaTime * _ctx.moveSpeed*80);
            _rigidbody.velocity = new Vector2(newX, _rigidbody.velocity.y);
       
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
//只要人在往上 就是上升状态
public class JumpState : PlayerState
{
    public JumpState(PlayerControl ctx) : base(ctx)
    {
        //
    }
    public override void Enter()
    {
        Debug.Log("进入跳跃状态");
        //使得普通跳跃次数消耗
        _ctx.setJump(false);

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

