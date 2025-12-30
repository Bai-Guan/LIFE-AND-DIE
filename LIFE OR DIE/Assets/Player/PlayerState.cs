
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

    public virtual void Other(UnityEngine.Transform t)
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
    int enemyLayer = LayerMask.GetMask("Enemy");
    public void Enter() 
    {
     
        //设置动画变量 重置状态 
        Debug.Log("进入待机状态");
        _ctx.rb.velocity=new Vector2(0,0);
        enemyLayer = LayerMask.GetMask("Enemy");
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
        // 根据 lastFacing 决定射线方向
        Vector3 pos = _ctx.transform.position;
        pos.y += 1f;
        Vector2 dir = _ctx.isFacingLeft ? Vector2.left : Vector2.right;
        Debug.DrawRay(pos, dir * _ctx.DataMan.背刺射线长度, Color.white,5f);
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, _ctx.DataMan.背刺射线长度, enemyLayer);
        if(hit.collider!=null)
        {
            Debug.Log("背刺射线找到目标!");
            var enemy = hit.collider.GetComponent<InitEnemySystem>();
            if (enemy != null && enemy.是否可以背刺)
            {
                _ctx.SwitchState(TypeState.backstab);
                return;
            }
        }


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
    public void Update()
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
        if (_ctx.InputY > 0 && _ctx.DataMan.canJump)
        {
            _ctx.SwitchState(TypeState.jump);
            return;
        }



        timer += Time.deltaTime;
        //检测朝向
        _ctx.CheckFill();


    }
    public void FixedUpdate()
    {

        //进行了移动

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
        float scale;
        if (timer / 0.25f > 0)
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
        _ctx.SwitchState(TypeState.block);
    }
   }
    public class BackstabState : IPlayerState
    {
        private NewPlayerControll _ctx;
        private float timer = 0;
    private float duringTime = 3f;
        public BackstabState(NewPlayerControll playerControll)
        {
            _ctx = playerControll;
        }

        public void Enter()
        {

            Debug.Log("进入背刺状态");
            timer = 0;
        _ctx.Anim.TriggerAttack2();

    }
        public void Update()
        {

           if(timer>duringTime) 
            _ctx.SwitchState(TypeState.ldle);



        }
        public void FixedUpdate()
        {
        timer += Time.fixedDeltaTime;

        }
        public void Exit()
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
          
        }
        private void MoveX()
        {
          
        }
        public void Block()
         {
      
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
    private Vector2 _originalColliderSize;
    private Vector2 _originalColliderOffset;
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
       // _ctx.DataMan.SetInvencibleAndStart(0.2f);

        //碰撞变小
        // 保存原始碰撞箱设置
        _originalColliderSize = _ctx.boxCollider2D.size;
        _originalColliderOffset = _ctx.boxCollider2D.offset;

        // 设置新的碰撞箱大小
        _ctx.boxCollider2D.size = new Vector2(0.6f, 0.5f);

        // 计算并设置新的偏移量，保持底部位置不变
        float heightDifference = _originalColliderSize.y - 0.5f; // 高度差
        float offsetY = _originalColliderOffset.y - (heightDifference / 2f); // 向下移动一半的高度差
        _ctx.boxCollider2D.offset = new Vector2(_originalColliderOffset.x, offsetY);

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

        // 恢复原始碰撞箱设置
        _ctx.boxCollider2D.size = _originalColliderSize;
        _ctx.boxCollider2D.offset = _originalColliderOffset;
    }
    //定义加速减速时间 0.08s内加速 0.1秒内完成减速 0.32s内恒速运动
    private int sprintDir;
    private float PressSprintTime = 0;
    private float _currentSpeed = 0; 
    private float _currentTime = 0;

    private const float AccelerationTime = 0.08f;
    private const float ConstantTime = 0.15f;
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
       _ctx.rb.velocity=new Vector2(0,_ctx.rb.velocity.y);
        
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
    private bool clock=false;

    private float dragTimer = 0f;          // 水平衰减计时器
    private float noGravityTimer = 0f;     // 免重力计时器
    private Vector2 enterVelocity;         // 进入时的速度快照


    private Vector2 _originalColliderSize;
    private Vector2 _originalColliderOffset;
    public DieState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }
    public  void Enter()
    {
        Debug.Log("死了");
        AudioManager.Instance.PlaySFX("玩家被杀");
        玩家的全局变量.玩家是否死亡 = true;

        _ctx.rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _ctx.rb.interpolation = RigidbodyInterpolation2D.Interpolate; // 顺便补帧，更顺滑

        //碰撞变小
        // 保存原始碰撞箱设置
        _originalColliderSize = _ctx.boxCollider2D.size;
        _originalColliderOffset = _ctx.boxCollider2D.offset;

        // 设置新的碰撞箱大小
        _ctx.boxCollider2D.size = new Vector2(0.6f, 0.5f);

        // 计算并设置新的偏移量，保持底部位置不变
        float heightDifference = _originalColliderSize.y - 0.5f; // 高度差
        float offsetY = _originalColliderOffset.y - (heightDifference / 2f); // 向下移动一半的高度差
        _ctx.boxCollider2D.offset = new Vector2(_originalColliderOffset.x, offsetY);


        _ctx.Anim.TriggerDie();

        //速度设置
        _ctx.rb.velocity=new Vector2(_ctx.rb.velocity.x, _ctx.rb.velocity.y);
        enterVelocity = _ctx.rb.velocity;     // 记录进入瞬间速度
        dragTimer = 0f;
        noGravityTimer = 0f;
        //判断还有没有命 没命就真的死了
        if (_ctx.DataMan.currentHP <= 0)
        {
            TimeManager.Instance.OneTime(3f, () =>
            {
                玩家的全局变量.玩家是否真正死亡 = true;
                AudioManager.Instance.PlaySFX("死");
            });
            
        }
            
        timer= 0f;
    }
    public  void Update()
    {
        timer+= Time.deltaTime;
    }
    public void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector2 v = _ctx.rb.velocity;

        /* -------- 1. 水平速度处理 -------- */
        float vx;
        dragTimer += dt;
        if (dragTimer < 0.05f)
        {
            // 前 0.2 s 完全保留进入时的水平速度
            vx = enterVelocity.x;
        }
        else
        {
            const float a = 3.0f;  // 恒定减速度，单位：速度单位/秒
            float sign = Mathf.Sign(enterVelocity.x);
            float newSpeed = Mathf.Abs(v.x) - a * dt;

            const float minSpeed = 0.05f;
            vx = newSpeed < minSpeed ? 0 : sign * newSpeed;
        }

        /* -------- 2. 前 0.5 s 无视重力 -------- */
        noGravityTimer += dt;
        //float vy = (noGravityTimer < 0.3f)
        //           ? enterVelocity.y
        //           : v.y + Physics2D.gravity.y * _ctx.rb.gravityScale * dt;

        _ctx.rb.velocity = new Vector2(vx, _ctx.rb.velocity.y);
    
    
 
    }
    public  void Exit()
    {
        _ctx.rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        _ctx.rb.interpolation = RigidbodyInterpolation2D.None;
        // 恢复原始碰撞箱设置
        _ctx.boxCollider2D.size = _originalColliderSize;
        _ctx.boxCollider2D.offset = _originalColliderOffset;
        clock =false;
    }

    public void Attack()
    {
        
    }

    public void Dodge()
    {
       
    }

    public void ContractPower()
    {
      
        if (timer >= 1.5f&&clock==false)
        { 
            clock = true;
        if (_ctx.DataMan.currentHP > 0)
        {
            _ctx.Anim.TrigererResurgence(true);
            AudioManager.Instance.PlaySFX("复活");
            TimeManager.Instance.OneTime(1.1f, () =>
            {
                _ctx.Anim.TrigererResurgence(false);
                玩家的全局变量.玩家是否死亡 = false;
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



