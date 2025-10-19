
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public abstract class PlayerState
{

    protected PlayerState(PlayerControl ctx)
    {
        _ctx = ctx;
        this.gameObject = ctx.gameObject;
        _spriteRenderer = ctx.GetComponent<SpriteRenderer>();
    }


    public virtual void Enter()
    {
        _rigidbody = _ctx.GetRigidbody();
        _vector = _ctx.GetVector2();
    }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }

    public virtual void Other(Transform t)
    {

    }
  

    protected readonly PlayerControl _ctx;
    protected GameObject gameObject;
    protected SpriteRenderer _spriteRenderer;
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
        //�ٶ�==0������� �����������ԾתĬ��
        
        if (Mathf.Abs(_ctx.h) > 0.1f && _ctx.IsGrounded())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.run);
            return;
        }
        if (_ctx.KeyDownJump&& _ctx.canJump)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
            return;
        }
        //�������״̬
        if (_rigidbody.velocity.y < -0.1f && (_ctx.IsGrounded() == false))
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.fall);
        }
        if(_ctx.FinallyPrintCheck())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.sprint);
        }

       
    }
    public override void FixedUpdate()
    {
        _ctx.XMove();
       
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
        if (_ctx.FinallyPrintCheck())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.sprint);
        }
        //�������״̬
        if (_rigidbody.velocity.y < -0.1f&&_ctx.IsGrounded()==false)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.fall);
            return;
        }
        //����Ĭ��״̬������
        if (Mathf.Abs(_ctx.h) <=0.1 && _ctx.IsGrounded() == true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
            return;
        }
        //������Ծ״̬������
        if (_ctx.KeyDownJump && _ctx.canJump)
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
      
    }
    public override void Update()
    {
        if (_ctx.FinallyPrintCheck())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.sprint);
        }
        //����Ϊ���� ����Ϊ��� �������״̬
        if ( _ctx.IsGrounded()==true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        }
        if(_ctx.KeyDownJump && _ctx.FinallyJumpCheck())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
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
        if (_ctx.FinallyPrintCheck())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.sprint);
        }


        //�ٶ�<0��Ϊ��׹
        if (_rigidbody.velocity.y<0)
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

//����״̬�£�������ʱ�����״̬
public class HitState : PlayerState
{
    public HitState(PlayerControl ctx) : base(ctx)
    {

    }
    public override void Enter()
    {
        Debug.Log("��������״̬ ");
        _ctx.Anim.TriggerHurt();
        
        IsFirst = true;
        Timer = 0;
        

        //�յ�һ����ʹ�ý�ɫ������ Ŀǰ �ڼ��޷����κ�����

        //Debug 
        _spriteRenderer.color=Color.red;

        //�޵�֡1.2s
        _ctx.SetInvincible(true);
        TimeManager.Instance.OneTime(_ctx.InvincibleFrameTime,
            () =>
            {
                Debug.Log("�޵�֡������");
                _ctx.SetInvincible(false);
            });
    }
    public override void Update()
    {
        if(IsFirst==true)
        {
            Debug.DrawRay(_ctx.gameObject.transform.position, hitDir, Color.blue, 2f);
            _ctx.SetRB_X(hitDir.x * hitX); _ctx.SetRB_Y(hitDir.y * hitY);
            IsFirst = false;
        }



        Timer += Time.deltaTime;
        if (Timer > hitStun)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        }
    }
    public override void FixedUpdate()
    {
        _ctx.SetRB_X(_ctx.GetRigidbody2D().velocity.x);
        _ctx.SetRB_Y(_ctx.GetRigidbody2D().velocity.y);
    }
    public override void Exit()
    {
        //Debug
        _spriteRenderer.color = Color.white;

        hitDir = Vector3.zero;
        Timer = 0;

        IsFirst = false;
    }

   
    public override void Other(Transform dir)
    {
        Vector3 pos = this.gameObject.transform.position;
       Vector3 Temp = new Vector3(
           pos.x-dir.position.x,
           pos.y-dir.position.y,
           0   
           );
        hitDir = Vector3.Normalize(Temp);
        Debug.Log("��������"+ hitDir);
    }
   Vector2 hitDir = Vector2.zero;
    private float Timer = 0;
    public float hitStun =0.3f;
    private float hitX = 5f;
    private float hitY = 5f;

    bool IsFirst = true;
}

public class SprintState : PlayerState
{
    //���Ŀǰ�趨Ϊ����ˮƽ���� ���ڼ䲻���ƶ� ����Ӱ��
    public SprintState(PlayerControl ctx) : base(ctx)
    {

    }
    public override void Enter()
    {
        _ctx.Anim.TriggerSprint();
        _ctx.isSprint = true;

        //1.ˮƽ�ٶ�����Ϊ0���ٶȷ�Ϊ ���� ���� ���� �����׶Σ����м��ٽ׶�Ϊ͹���� ���ٽ׶�Ϊ������
        _ctx.GetRigidbody2D().velocity = Vector2.zero;
        //��¼���³�̼�ʱ��ʱ�� ��¼��̷���
        PressSprintTime = Time.time;
       
        sprintDir = _ctx.IsFacingLeft ? -1 : 1;
        _currentSpeed =_ctx.moveSpeed;

        //���ò��ܳ�� Э��0.4�����ܳ�
        _ctx.CDcanSprint=false;
        TimeManager.Instance.OneTime(_ctx.sprintCD,
            () =>
            {
                _ctx.CDcanSprint = true;
            }
            );



        //�޵�֡0.4��
        _ctx.SetInvincible(true);
        TimeManager.Instance.OneTime(AccelerationTime+ConstantTime,
            () =>
            {
                _ctx.SetInvincible(false);
            });
        _ctx.InvincibleRendered(AccelerationTime + ConstantTime);
    }
    public override void Update()
    {
      _currentTime = Time.time-PressSprintTime;
        //���ײǽ �ٶ�xΪ0 ���ռ���
        if (_ctx._isTouchingWall == true && _currentTime < AccelerationTime + ConstantTime + DecelerationTime) _currentSpeed = 0;
        else
        {






            if (_currentTime <= AccelerationTime)
            {
                _currentSpeed = EaseInExpo(_ctx.sprintSpeed, AccelerationTime, _currentTime);
                
            }
            else if (_currentTime <= AccelerationTime + ConstantTime && _currentTime > AccelerationTime)
            {
                _currentSpeed = _ctx.sprintSpeed;
            }
            else if (_currentTime > AccelerationTime + ConstantTime && _currentTime <= AccelerationTime + ConstantTime + DecelerationTime)
            {
                _currentSpeed = EaseOutExpo(_currentTime, _ctx.sprintSpeed, DecelerationTime);
                
            }
            else
            {
                _ctx.SetRB_X(0);
                _ctx.SetSprintZeroTimes();
                _ctx.SwitchStatus(PlayerControl.PlayerStatus.run);
            }
        }
    }
    public override void FixedUpdate()
    {
        
        //��������Ӱ��
        _ctx.SetRB_Y(0);
        _ctx.SetRB_X(_currentSpeed*sprintDir);
        

    }
    public override void Exit()
    {
        _currentSpeed = 0;
        _ctx.isSprint = false;
        _ctx.SetInvincible(false);
    }
    //������ټ���ʱ�� 0.08s�ڼ��� 0.1������ɼ��� 0.32s�ں����˶�
    private int sprintDir;
    private float PressSprintTime = 0;
    private float _currentSpeed = 0; 
    private float _currentTime = 0;

    private const float AccelerationTime = 0.08f;
    private const float ConstantTime = 0.09f;
    private const float DecelerationTime = 0.05f;
    

  private float EaseInExpo(float targetVelocity,float needTime,float currentTime)
    {
        float temp=currentTime/needTime;
        return   targetVelocity  * Mathf.Pow(2, 10 * (temp - 1));
    }
    float EaseOutExpo(float currentTime, float start, float needTime)
    {
        float temp = currentTime-AccelerationTime-ConstantTime;
        temp /= needTime;
        return start * (-Mathf.Pow(2, -10 * temp) + 1);
    }

}



public class BlockState : PlayerState
{
    //����ʱ���޻��� ��ʱ����˷�Ϊ������ ǿ���� ���Զ������
    public BlockState(PlayerControl ctx) : base(ctx)
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
public class DieState : PlayerState
{
    //����ʱ���޻��� ��ʱ����˷�Ϊ������ ǿ���� ���Զ������
    public DieState(PlayerControl ctx) : base(ctx)
    {

    }
    public override void Enter()
    {
        Debug.Log("����");
        _ctx.Anim.TriggerDie();
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


