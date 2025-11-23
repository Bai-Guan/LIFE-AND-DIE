
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





public class IdleState:IPlayerState
{
    private NewPlayerControll _ctx;
   public IdleState(NewPlayerControll playerControll)
    {
        _ctx= playerControll;
    }

    public void Enter() 
    {
     
        //设置动画变量 重置状态 
        Debug.Log("进入待机状态");
        _ctx.rb.velocity=new Vector2(0,0);  
       
    }
    public void Update() 
    {
        //速度==0进入待机 极端情况下跳跃转默认

        if (Mathf.Abs(_ctx.InputX) > 0.1f && _ctx.DataMan.isGround)
        {
            _ctx.SwitchState(TypeState.run);
            return;
        }
        if (_ctx.InputY>0&&_ctx.DataMan.canJump)
        {
            _ctx.SwitchState(TypeState.jump);
            return;
        }
        //检测下落状态
        if (_ctx.rb.velocity.y < -0.1f && _ctx.DataMan.isGround == false)
        {
            _ctx.SwitchState(TypeState.fall);
        }


    }
    public  void FixedUpdate()
    {
       MoveX();
       
    }
    public void Exit() 
    {
    
    }

    public void Attack()
    {
       _ctx.SwitchState(TypeState.attack);
    }

    public void Dodge()
    {
        if (_ctx.DataMan.isDodgeTimeReady(Time.time))
        {
            _ctx.SwitchState(TypeState.sprint);
        }
    }

    public void ContractPower()
    {
        
    }

    private void MoveX()
    {
        // 正常移动
        float targetVelocityX = _ctx.InputX * _ctx.DataMan.moveSpeed;
        float newVelocityX = Mathf.MoveTowards(
            _ctx.rb.velocity.x,
            targetVelocityX,
             Time.fixedDeltaTime * 10
        );
        _ctx.rb.velocity = new Vector2(newVelocityX, _ctx.rb.velocity.y);
    }
}

public class RunState : IPlayerState
{
    private NewPlayerControll _ctx;
    private float timer = 0;
    public RunState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }

    public void Enter()
    {
     
        Debug.Log("进入跑步状态");
        timer = 0;


    }
    public  void Update()
    {
    
        //检测下落状态
        if (_ctx.rb.velocity.y < -0.1f && _ctx.DataMan.isGround == false)
        {
            _ctx.SwitchState(TypeState.fall);
            return;
        }
        //进入默认状态的条件
        if (Mathf.Abs(_ctx.InputX) <= 0.1 && _ctx.DataMan.isGround == true)
        {
            _ctx.SwitchState(TypeState.ldle);
            return;
        }
        //进入跳跃状态的条件
        if (_ctx.InputY>0 && _ctx.DataMan.canJump)
        {
            _ctx.SwitchState(TypeState.jump);
            return;
        }



        timer += Time.deltaTime;
        //检测朝向
        _ctx.CheckFill();


    }
    public  void FixedUpdate()
    {
       
        //进行了移动

        MoveX();
       
    }
    public  void Exit()
    {

    }

    public void Attack()
    {
        _ctx.SwitchState(TypeState.attack);
    }

    public void Dodge()
    {
        if (_ctx.DataMan.isDodgeTimeReady(Time.time))
        {
            _ctx.SwitchState(TypeState.sprint);
        }
    }

    public void ContractPower()
    {
        
    }
    private void MoveX()
    {
        float scale;
        if(timer/0.25f>0)
        {
            scale=1;
        }
        else
        {
            scale = timer / 0.25f;
        }
       
        // 正常移动
        float targetVelocityX = _ctx.InputX * _ctx.DataMan.moveSpeed;
        float newVelocityX = Mathf.Lerp(
            _ctx.rb.velocity.x,
            targetVelocityX,
             scale
        );
        _ctx.rb.velocity = new Vector2(newVelocityX, _ctx.rb.velocity.y);
    }
}
public class FallState : IPlayerState
{
    private NewPlayerControll _ctx;
    private float timer=0;
    public FallState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }
    public  void Enter()
    {
       
        Debug.Log("进入下坠状态");
        timer = 0;
    }
    public  void Update()
    {
       
        //脚下为地面 则视为落地 进入待机状态
        if (_ctx.DataMan.isGround)
        {
            _ctx.SwitchState(TypeState.ldle);
        }
        if (_ctx.InputY>0 && _ctx.DataMan.canJump)
        {
            _ctx.SwitchState(TypeState.jump);
        }
        //检测朝向
        _ctx.CheckFill();
        timer += Time.deltaTime;
    }
    public  void FixedUpdate()
    {
       
      MoveX();
    }
    public void Exit()
    {

    }

    public void Attack()
    {
        throw new NotImplementedException();
    }

    public void Dodge()
    {
     
            if (_ctx.DataMan.isDodgeTimeReady(Time.time))
            {
                _ctx.SwitchState(TypeState.sprint);
            }
        
    }

    public void ContractPower()
    {
        //TODO:特殊效果 坠落
        _ctx.SwitchState(TypeState.collision);
    }



    private void MoveX()
    {
        float scale;
        if (timer / 0.05f > 0)
        {
            scale = 1;
        }
        else
        {
            scale = timer / 0.25f;
        }
        // 正常移动
        float targetVelocityX = _ctx.InputX * _ctx.DataMan.moveSpeed;
        float newVelocityX = Mathf.Lerp(
            _ctx.rb.velocity.x,
            targetVelocityX,
             scale
        );
        _ctx.rb.velocity = new Vector2(newVelocityX, _ctx.rb.velocity.y);
    }
}
//只要人在往上 就是上升状态
public class JumpState : IPlayerState
{
    private NewPlayerControll _ctx;
    private float timer = 0;
    public JumpState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }
    public void Enter()
    {
        
        Debug.Log("进入跳跃状态");
        _ctx.Anim.TriggerJump();
        //进行角色跳跃操作
        Jump();
        //使得普通跳跃次数消耗
        // _ctx.setJump(false);//恢复部分在碰撞实现

        //跳跃前的水平速度清零
        timer = 0;
       _ctx.rb.velocity=new Vector2(0, _ctx.rb.velocity.y);

        

    }
    public void Update()
    {
    


        //速度<0则为下坠
        if (_ctx.rb.velocity.y < 0)
        {
            _ctx.SwitchState(TypeState.fall);
        }
       // timer += Time.deltaTime;
        //检测朝向
        _ctx.CheckFill();
    }
    public  void FixedUpdate()
    {
        //如果一直按住了跳跃 则y速度将一直加某一个小于重力的值 使得跳跃滞空时间变长
    if(_ctx.InputY>0)
        {
            Vector2 up = new Vector2(0, 10f);
           _ctx.rb.AddForce(up);
            
        }
        
        //进行了空中移动
        MoveX();
    }
    public  void Exit()
    {

    }
   private void Jump()
    {
        if (_ctx.InputY >0)
        {
            float newY = _ctx.DataMan.jumpSpeed;
           _ctx.rb.velocity = new Vector2(_ctx.rb.velocity.x, newY);
        }
    }

    public void Attack()
    {
      
    }

    public void Dodge()
    {
        if (_ctx.DataMan.isDodgeTimeReady(Time.time))
        {
            _ctx.SwitchState(TypeState.sprint);
        }
    }

    public void ContractPower()
    {
        //TODO:特殊效果 坠落
        _ctx.SwitchState(TypeState.collision);
    }


    private void MoveX()
    {
        //float scale;
        //if (timer / 0.05f > 0)
        //{
        //    scale = 1;
        //}
        //else
        //{
        //    scale = timer / 0.25f;
        //}
        // 正常移动
        float targetVelocityX = _ctx.InputX * _ctx.DataMan.moveSpeed;
    
        _ctx.rb.velocity = new Vector2(targetVelocityX, _ctx.rb.velocity.y);
    }
}

//任意状态下，被攻击时进入此状态
public class UnexpectedState : IPlayerState
{
    private NewPlayerControll _ctx;
    public UnexpectedState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }
    public  void Enter()
    {
    //    Debug.Log("进入奇招状态 ");
    //    //_ctx.Anim.TriggerHurt();
        
    //    IsFirst = true;
    //    Timer = 0;
    
      
    //     //增加事件的监听
    //     _ctx.PlayerInput.currentActionMap





    //    //Debug 
    //    _spriteRenderer.color=Color.red;

    //    //无敌帧1.2s
    //    _ctx.SetInvincible(true);
    //    TimeManager.Instance.OneTime(_ctx.InvincibleFrameTime,
    //        () =>
    //        {
    //            Debug.Log("无敌帧结束！");
    //            _ctx.SetInvincible(false);
    //        });
    }
    public  void Update()
    {
        //监听是否按攻击键 


        //if (IsFirst==true)
        //{
        //    Debug.DrawRay(_ctx.gameObject.transform.position, hitDir, Color.blue, 2f);
        //    _ctx.SetRB_X(hitDir.x * hitX); _ctx.SetRB_Y(hitDir.y * hitY);
        //    IsFirst = false;
        //}



        //Timer += Time.deltaTime;
        //if (Timer > hitStun)
        //{
        //    _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        //}
    }
    public void FixedUpdate()
    {
        //_ctx.SetRB_X(_ctx.GetRigidbody2D().velocity.x);
        //_ctx.SetRB_Y(_ctx.GetRigidbody2D().velocity.y);
    }
    public void Exit()
    {
        //Debug
        //_spriteRenderer.color = Color.white;

        //hitDir = Vector3.zero;
        //Timer = 0;

        //IsFirst = false;
    }

   
    //public override void Other(Transform dir)
    //{
    //    Vector3 pos = this.gameObject.transform.position;
    //   Vector3 Temp = new Vector3(
    //       pos.x-dir.position.x,
    //       pos.y-dir.position.y,
    //       0   
    //       );
    //    hitDir = Vector3.Normalize(Temp);
    //    Debug.Log("受伤向量"+ hitDir);
    //}

    public void Attack()
    {
       
    }

    public void Dodge()
    {
       
    }

    public void ContractPower()
    {
        
    }

    //Vector2 hitDir = Vector2.zero;
    //private float Timer = 0;
    //public float hitStun =0.3f;
    //private float hitX = 5f;
    //private float hitY = 5f;

    //bool IsFirst = true;
}

public class SprintState : IPlayerState
{
    private NewPlayerControll _ctx;
    public SprintState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }
    public  void Enter()
    {
        _ctx.Anim.TriggerSprint();
        //_ctx.isSprint = true;

        //1.水平速度先设为0，速度分为 加速 恒速 减速 三个阶段，其中加速阶段为凸函数 减速阶段为凹函数
        _ctx.rb.velocity = Vector2.zero;
        //记录按下冲刺键时的时间 记录冲刺方向
        PressSprintTime = Time.time;
       
        sprintDir = _ctx.isFacingLeft ? -1 : 1;
        _currentSpeed = _ctx.DataMan.sprintSpeed;



        //无敌帧
        _ctx.DataMan.SetInvencibleAndStart(0.2f);
    }
    public  void Update()
    {
        _currentTime = Time.time - PressSprintTime;
      
       
       
        






            if (_currentTime <= AccelerationTime)
            {
                _currentSpeed = EaseInExpo(_ctx.DataMan.sprintSpeed, AccelerationTime, _currentTime);

            }
            else if (_currentTime <= AccelerationTime + ConstantTime && _currentTime > AccelerationTime)
            {
                _currentSpeed = _ctx.DataMan.sprintSpeed;
            }
            else if (_currentTime > AccelerationTime + ConstantTime && _currentTime <= AccelerationTime + ConstantTime + DecelerationTime)
            {
                _currentSpeed = EaseOutExpo(_currentTime, _ctx.DataMan.sprintSpeed, DecelerationTime);

            }
            else
            {
                _ctx.rb.velocity = new Vector2(0, 0);
                _ctx.SwitchState(TypeState.run);
            }
        
    }
    public void FixedUpdate()
    {
        
        //不受重力影响
        _ctx.rb.velocity = new Vector2(_currentSpeed * sprintDir, 0); 
        

    }
    public  void Exit()
    {
        _currentSpeed = 0;
       
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

    public void Attack()
    {
       
    }

    public void Dodge()
    {
       
    }

    public void ContractPower()
    {
       
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
public class DieState : IPlayerState
{
    private NewPlayerControll _ctx;
    public DieState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }
    public  void Enter()
    {
        Debug.Log("死了");
        _ctx.Anim.TriggerDie();

        //判断还有没有命 没命就真的死了
    }
    public  void Update()
    {

    }
    public  void FixedUpdate()
    {

    }
    public  void Exit()
    {

    }

    public void Attack()
    {
        
    }

    public void Dodge()
    {
       
    }

    public void ContractPower()
    {
       if(_ctx.DataMan.currentHP>0)
        {
            _ctx.SwitchState(TypeState.ldle);
        }
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


