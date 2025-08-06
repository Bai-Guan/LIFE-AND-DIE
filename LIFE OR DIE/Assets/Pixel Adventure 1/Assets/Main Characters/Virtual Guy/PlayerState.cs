
using UnityEngine;


    public abstract class PlayerState
    {
        protected readonly PlayerControl _ctx;
        protected PlayerState(PlayerControl ctx) { _ctx = ctx; }


        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit() { }
    }

public class IdleState:PlayerState 
{
    public IdleState(PlayerControl ctx):base(ctx) 
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

public class RunState : PlayerState
{
    public RunState(PlayerControl ctx) : base(ctx)
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

