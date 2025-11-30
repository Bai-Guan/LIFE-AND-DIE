
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
        _ctx.开启奇点时刻();
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

    public void Block()
    {
        _ctx.SwitchState(TypeState.block);
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
        _ctx.开启奇点时刻();
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

    public void Block()
    {
        _ctx.SwitchState(TypeState.block);
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

    public void Block()
    {
      
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

    public void Block()
    {
    
    }
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

    public void Block()
    {
        
    }
}



public class BlockState : IPlayerState
{
    private NewPlayerControll _ctx;
    public BlockState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }
    float timer = 0;
    public void Enter()
    {
        Debug.Log("进入格挡状态");
        _ctx.Anim.TriggerBlock(true);
        _ctx.rb.velocity = new Vector2(0, 0);
        timer = 0;
    }
    public  void Update()
    {
        timer += Time.deltaTime;
        if(timer<=_ctx.DataMan.PerfectBlock)_ctx.DataMan.isPerfectBlock=true;
        else _ctx.DataMan.isPerfectBlock = false;

        if (_ctx.DataMan.isPressBlock == false)
        {
            //切换要延迟切换
            _ctx.Anim.TriggerBlock(false);
            _ctx.SwitchState(TypeState.ldle);
        }
    }
    public  void FixedUpdate()
    {

    }
    public  void Exit()
    {
        _ctx.Anim.TriggerBlock(false);
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

    public void Block()
    {
       
    }
}
public class DieState : IPlayerState
{
    private NewPlayerControll _ctx;
    private float timer=0f;
    public DieState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }
    public  void Enter()
    {
        Debug.Log("死了");
        AudioManager.Instance.PlaySFX("玩家被杀");
        _ctx.Anim.TriggerDie();
        _ctx.rb.velocity=new Vector2(0,_ctx.rb.velocity.y);
        //判断还有没有命 没命就真的死了
        if (_ctx.DataMan.currentHP <= 0)
        {
            TimeManager.Instance.OneTime(3f, () =>
            {
                AudioManager.Instance.PlaySFX("死");
            });
            
        }
            
        timer= 0f;
    }
    public  void Update()
    {
        timer+= Time.deltaTime;
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
        if (timer >= 1.5f)
        { 
        if (_ctx.DataMan.currentHP > 0)
        {
            _ctx.Anim.TrigererResurgence(true);
            AudioManager.Instance.PlaySFX("复活");
            TimeManager.Instance.OneTime(1.1f, () =>
            {
                _ctx.Anim.TrigererResurgence(false);
                _ctx.SwitchState(TypeState.ldle);



            });
        }
    }
    }

    public void Block()
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


