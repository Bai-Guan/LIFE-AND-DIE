
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
        //设置动画变量 重置状态 
        Debug.Log("进入待机状态");
       
    }
    public override void Update() 
    {
        //速度==0进入待机 极端情况下跳跃转默认
        
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
        //检测下落状态
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
        Debug.Log("进入跑步状态");
       
    }
    public override void Update()
    {
        if (_ctx.FinallyPrintCheck())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.sprint);
        }
        //检测下落状态
        if (_rigidbody.velocity.y < -0.1f&&_ctx.IsGrounded()==false)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.fall);
            return;
        }
        //进入默认状态的条件
        if (Mathf.Abs(_ctx.h) <=0.1 && _ctx.IsGrounded() == true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
            return;
        }
        //进入跳跃状态的条件
        if (_ctx.KeyDownJump && _ctx.canJump)
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
      
    }
    public override void Update()
    {
        if (_ctx.FinallyPrintCheck())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.sprint);
        }
        //脚下为地面 则视为落地 进入待机状态
        if ( _ctx.IsGrounded()==true)
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        }
        if(_ctx.KeyDownJump && _ctx.FinallyJumpCheck())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.jump);
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
        if (_ctx.FinallyPrintCheck())
        {
            _ctx.SwitchStatus(PlayerControl.PlayerStatus.sprint);
        }


        //速度<0则为下坠
        if (_rigidbody.velocity.y<0)
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

//任意状态下，被攻击时进入此状态
public class HitState : PlayerState
{
    public HitState(PlayerControl ctx) : base(ctx)
    {

    }
    public override void Enter()
    {
        Debug.Log("进入受伤状态 ");
        _ctx.Anim.TriggerHurt();
        
        IsFirst = true;
        Timer = 0;
        

        //收到一个力使得角色被击退 目前 期间无法做任何事情

        //Debug 
        _spriteRenderer.color=Color.red;

        //无敌帧1.2s
        _ctx.SetInvincible(true);
        TimeManager.Instance.OneTime(_ctx.InvincibleFrameTime,
            () =>
            {
                Debug.Log("无敌帧结束！");
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
        Debug.Log("受伤向量"+ hitDir);
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
    //冲刺目前设定为仅限水平方向 此期间不受移动 重力影响
    public SprintState(PlayerControl ctx) : base(ctx)
    {

    }
    public override void Enter()
    {
        _ctx.Anim.TriggerSprint();
        _ctx.isSprint = true;

        //1.水平速度先设为0，速度分为 加速 恒速 减速 三个阶段，其中加速阶段为凸函数 减速阶段为凹函数
        _ctx.GetRigidbody2D().velocity = Vector2.zero;
        //记录按下冲刺键时的时间 记录冲刺方向
        PressSprintTime = Time.time;
       
        sprintDir = _ctx.IsFacingLeft ? -1 : 1;
        _currentSpeed =_ctx.moveSpeed;

        //设置不能冲刺 协程0.4秒后才能冲
        _ctx.CDcanSprint=false;
        TimeManager.Instance.OneTime(_ctx.sprintCD,
            () =>
            {
                _ctx.CDcanSprint = true;
            }
            );



        //无敌帧0.4秒
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
        //如果撞墙 速度x为0 浮空继续
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
        
        //不受重力影响
        _ctx.SetRB_Y(0);
        _ctx.SetRB_X(_currentSpeed*sprintDir);
        

    }
    public override void Exit()
    {
        _currentSpeed = 0;
        _ctx.isSprint = false;
        _ctx.SetInvincible(false);
    }
    //定义加速减速时间 0.08s内加速 0.1秒内完成减速 0.32s内恒速运动
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
    //弹反时候无击退 格挡时候击退分为弱击退 强击退 和自定义击退
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
    //弹反时候无击退 格挡时候击退分为弱击退 强击退 和自定义击退
    public DieState(PlayerControl ctx) : base(ctx)
    {

    }
    public override void Enter()
    {
        Debug.Log("死了");
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


