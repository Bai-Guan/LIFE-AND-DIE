
using UnityEngine;


    public  class PlayerState
    {
       
        protected PlayerState(PlayerControl ctx) 
        { 
        _ctx = ctx;
        this.gameObject=ctx.gameObject;
        
        }


        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit() { }
    protected readonly PlayerControl _ctx;
    protected GameObject gameObject;
    }

public class IdleState:PlayerState 
{
    public IdleState(PlayerControl ctx):base(ctx) 
    {
    
    }
    public override void Enter() 
    { 
    //设置动画变量 重置状态 
    }
    public override void Update() 
    {
        if (_ctx.GetVector2().y == 0&&_ctx.GetVector2().x!=0)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.run);
            return;
        }
        if (_ctx.GetVector2().y != 0 )
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
            return;
        }
    }
    public override void FixedUpdate()
    { 
    
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
      
    }
    public override void Update()
    {
        //向量和为0时
        if (_ctx.GetVector2() == Vector2.zero)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
            return;
        }
        _vector.Set(_ctx.moveSpeed  * _ctx.h, 0);
    }
    public override void FixedUpdate()
    {
        _rigidbody.MovePosition(_vector * Time.deltaTime);
    }
    public override void Exit()
    {

    }
  

    private Rigidbody2D _rigidbody;
    private Vector2 _vector;

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
public class JumpState : PlayerState
{
    public JumpState(PlayerControl ctx) : base(ctx)
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
public class DoubleJumpState : PlayerState
{
    public DoubleJumpState(PlayerControl ctx) : base(ctx)
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

