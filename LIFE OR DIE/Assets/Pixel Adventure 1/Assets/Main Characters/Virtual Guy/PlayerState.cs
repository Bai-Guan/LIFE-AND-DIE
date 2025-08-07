
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
        //���ö������� ����״̬ 
        Debug.Log("�������״̬");
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
        //TODO:�������״̬

       
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
        Debug.Log("�����ܲ�״̬");
    }
    public override void Update()
    {
        //����Ĭ��״̬������
        if (Mathf.Abs(_ctx.h) <=0.1 && _ctx.IsGrounded() == true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
            return;
        }
        //������Ծ״̬������
        if (_ctx.IsGrounded() == false && _ctx.GetJump() == true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
            return;
        }
        //��⳯��
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
        
        //�������ƶ�
      
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
//ֻҪ�������� ��������״̬
public class JumpState : PlayerState
{
    public JumpState(PlayerControl ctx) : base(ctx)
    {
        //
    }
    public override void Enter()
    {
        Debug.Log("������Ծ״̬");
        //ʹ����ͨ��Ծ��������
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

