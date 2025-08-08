
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
        //���ö������� ����״̬ 
        Debug.Log("�������״̬");
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
        
        //TODO:�������״̬

       
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
        Debug.Log("�����ܲ�״̬");
        _ctx.Anim.TriggerRUN();
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
        if (_ctx.KeyDownJump == true  && _ctx.GetJump() == true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
            return;
        }
        //��⳯��
        _ctx.CheckFill();


    }
    public override void FixedUpdate()
    {

        //�������ƶ�

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
        Debug.Log("������׹״̬");
        _ctx.Anim.TriggerFall();
    }
    public override void Update()
    {
        //�ٶ�yΪ0 �ҽ���Ϊ���� ����Ϊ��� �������״̬
        if(_rigidbody.velocity.y < 0.05f&&_ctx.IsGrounded()==true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        }
        //��⳯��
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
//ֻҪ�������� ��������״̬
public class JumpState : PlayerState
{
    public JumpState(PlayerControl ctx) : base(ctx)
    {
        //
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("������Ծ״̬");
        _ctx.Anim.TriggerJump();
        //���н�ɫ��Ծ����
        Jump();
        //ʹ����ͨ��Ծ��������
        _ctx.setJump(false);//�ָ���������ײʵ��

        //��Ծǰ��ˮƽ�ٶ�����
        _rigidbody.velocity=new Vector2(0,_rigidbody.velocity.y);

        

    }
    public override void Update()
    {
        //�ٶ�==0�������?

   //�ٶ�<0��Ϊ��׹
        if(_rigidbody.velocity.y<0)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.fall);
        }
        //����W������

        //��⳯��
        _ctx.CheckFill();
    }
    public override void FixedUpdate()
    {
        //���һֱ��ס����Ծ ��y�ٶȽ�һֱ��ĳһ��С��������ֵ ʹ����Ծ�Ϳ�ʱ��䳤
    if(_ctx.KeyDownJump==true)
        {
            Vector2 up = new Vector2(0, 10f);
            _rigidbody.AddForce(up);
            
        }
        //�����˿����ƶ�
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

