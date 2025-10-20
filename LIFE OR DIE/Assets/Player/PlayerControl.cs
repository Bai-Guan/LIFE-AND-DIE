using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;




public class PlayerControl : MonoBehaviour
{
    //编写 进入某个状态 某个状态update 离开某个状态  选择某个状态 这些方法
    public enum PlayerStatus
    {
        ldle,
        run,
        jump,
        fall,
        hit,
        sprint,
        block,
        attack,
        died,
        other
    }
    private PlayerStatus currentStatus;
    private PlayerState playerState;
    private Dictionary<PlayerStatus, PlayerState> _states = new Dictionary<PlayerStatus, PlayerState>();
    private PlayerEvent playerEvent;
    private  SpriteRenderer spriteRenderer;
    private Material material;
    public InitWeaponSystem weapon;
    public EdgeCheck Highcheck;
    public EdgeCheck Lowcheck;

    [HideInInspector] public PlayerAnimControl Anim;
    private void Awake()
    {
        _states[PlayerStatus.ldle] = new IdleState(this);
        _states[PlayerStatus.run] = new RunState(this);
        _states[PlayerStatus.jump] = new JumpState(this);
        _states[PlayerStatus.fall] = new FallState(this);
        _states[PlayerStatus.other] = new OtherState(this);
        _states[PlayerStatus.sprint] = new SprintState(this);
        _states[PlayerStatus.hit] = new HitState(this);
        _states[PlayerStatus.attack] = new AttackState(this);
        _states[PlayerStatus.block] = new BlockState(this);
        _states[PlayerStatus.died] = new DieState(this);
        rb = GetComponent<Rigidbody2D>();
        playerEvent = GetComponent<PlayerEvent>();
        spriteRenderer=GetComponent<SpriteRenderer>();
        Anim=GetComponent<PlayerAnimControl>();
        weapon = transform.Find("Weapon").GetComponent<InitWeaponSystem>();
        material=spriteRenderer.material;
    }

    void Start()
    {
        IsTouchingWall = false;
        isground = false;KeyDownJump=false;
        _LastpressSprintKey = Time.time-0.9f;
        currentStatus = PlayerStatus.other;
        SwitchStatus(PlayerStatus.ldle);

    }


    void Update()
    {
        //更新
        h = Input.GetAxisRaw("Horizontal");   // -1, 0, 1
        v = Input.GetAxisRaw("Vertical");
        moveInput.x = h; moveInput.y = v;

        //跳跃检测
        if (v>0.1f)
        {
            KeyDownJump = true;
        }
        else if (v<0.1f)
        {
            KeyDownJump = false;
        }
        //冲刺检测
        if(Input.GetKeyDown(KeyCode.L))
        {
            KeyDownSprint = true;
            
        }
        else if(Input.GetKeyUp(KeyCode.L)) KeyDownSprint = false;

        //攻击检测
        if (Input.GetKeyDown(KeyCode.J))
        {
            isKeyDownAttack= true;

        }
        else if (Input.GetKeyUp(KeyCode.J)) isKeyDownAttack = false;



        if(currentStatus==PlayerStatus.died)//todo
            return;


        // PredictiveJumpTime();
        FinallyJumpCheck();
        FinallyPrintCheck();
        FinallyAttackCheck();
        isTouchingWall();
        playerState?.Update();
       

    }
    //物理效果写在FixedUpdate
    private void FixedUpdate()
    {
        EdgeDetection();
        playerState?.FixedUpdate();
    }
    public void SwitchStatus(PlayerStatus newStatus)
    {
        if (currentStatus == newStatus) return;
        playerState?.Exit();    // 先离开当前状态
        currentStatus = newStatus;
        playerState = _states[newStatus];
        playerState.Enter();    // 再进入新状态
    }



    public Rigidbody2D GetRigidbody() { return rb; }
    public Vector2 GetVector2() { return moveInput; }
    public SpriteRenderer GetSpriteRenderer() { return spriteRenderer; }



    protected void FacingLeft()
    {
        spriteRenderer.flipX = true;
        isfacingleft = true;
    }
    protected void FacingRight()
    {
        spriteRenderer.flipX = false;
        isfacingleft = false;
    }
    public bool GetJump()
    {
        return canJump;
    }

    public void setJump(bool yesOrNo)
    {
        canJump = yesOrNo;
    }
    
    public void CheckFill()
    {

        //检测朝向
        if (h > 0.5f)
        {
           FacingRight();
        }
        if (h < -0.5f)
        {
            //Debug.LogWarning("向左了！");
            FacingLeft();
        }
    }
    public void XStop()
    {
        float temp = Mathf.MoveTowards(rb.velocity.x, 0, Time.fixedDeltaTime * groundFriction);
        rb.velocity = new Vector2(temp, rb.velocity.y);
    }
    public void XMove()
    {
        // 更新墙壁检测
        bool isTouchingWall = IsTouchingWall;

        // 只有当试图向墙壁方向移动时才限制
        if (isTouchingWall && Mathf.Sign(h) == (isfacingleft ? -1 : 1))
        {
        return;
        }
        else
        {
            // 正常移动
            float targetVelocityX = h * moveSpeed;
            float newVelocityX = Mathf.MoveTowards(
                rb.velocity.x,
                targetVelocityX,
                groundFriction * Time.fixedDeltaTime * 10
            );
            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);
        }
    }
    public void XMove(float runSpeedMulThisX)
    {
        // 更新墙壁检测
        bool isTouchingWall = IsTouchingWall;

        // 只有当试图向墙壁方向移动时才限制
        if (isTouchingWall && Mathf.Sign(h) == (isfacingleft ? -1 : 1))
        {
            return;
        }
        else
        {
            // 正常移动
            float targetVelocityX = h * moveSpeed*runSpeedMulThisX;
            float newVelocityX = Mathf.MoveTowards(
                rb.velocity.x,
                targetVelocityX,
                groundFriction * Time.fixedDeltaTime * 10
            );
            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);
        }
    }

    LayerMask layer = 1 << 6;
    [SerializeField] float RayLength = 0.5f;
    [SerializeField] private bool IsTouchingWall = false;
    public bool _isTouchingWall { get { return IsTouchingWall; } }
    private bool isTouchingWall()
    {
        int dir = isfacingleft ? -1 : 1;

        // 使用BoxCast检测一个区域而不是单个点
        Vector2 boxSize = new Vector2(0.1f, 1f); // 检测区域大小
        Vector2 boxOrigin = new Vector2(transform.position.x, transform.position.y+1f);

        RaycastHit2D hit = Physics2D.BoxCast(
            boxOrigin,
            boxSize,
            0f,
            Vector2.right * dir,
            RayLength,
            layer
        );

        // 可视化调试
        Debug.DrawRay(boxOrigin, Vector2.right * dir * RayLength, Color.red);
       // Debug.DrawRay(boxOrigin, Vector2.down * boxSize.y, Color.red);
        if (hit.collider == null)
        {


            IsTouchingWall = false;
            return IsTouchingWall;
        }
        if(hit.collider.tag=="Ground")
            IsTouchingWall= true;
        else IsTouchingWall =false;
        return IsTouchingWall;
    }


     public Rigidbody2D GetRigidbody2D() 
    {
        return rb;
    }
    public void SetRB_Y(float y)
    {
        this.rb.velocity = new Vector2(rb.velocity.x,y);
    }
    public void SetRB_X(float x)
    {
        this.rb.velocity = new Vector2(x, rb.velocity.y);
    }

    public void SetGrounded(bool grounded)
    {
        isground = grounded;
    }


    public bool IsGrounded()
    {
        return isground;    
    }

    //土狼跳跃相关参数
    
    [SerializeField] private float lastGroundtime = 0;
    [SerializeField] private float lastPressKey = 0;
    //土狼允许的滞空时间
  [SerializeField]  public float coyoteTime = 0.15f;
    //预输入时间
    public float JumpPredictiveTime = 0.5f;
    //用于在一段时间内保证跳跃可以被提前输入
    //用于检测是否可以跳跃  以及土狼时间
    public bool FinallyJumpCheck()
    {


        if (isground)
        {
            lastGroundtime = Time.time;
            canJump = true;
            
        }
        else
        {
            //不在地面时候 土狼时间内允许跳跃 土狼时间以外 不可跳跃
            if (Time.time - lastGroundtime <= coyoteTime) canJump = true;
            else canJump = false;
        }
     

        return canJump;
    }

 private float waitTime = 0.2f;
    void PredictiveJumpTime()
    {
        //跳跃发生后 该函数等0.2秒再每帧持续检测
        
        //在滞空时间时，若玩家按跳跃键，允许跳跃键持续一段时间后再变为不跳跃
        if (v > 0.1f && isground == false)
        {
            lastPressKey = Time.time;
        }
        if (lastGroundtime - lastPressKey <= JumpPredictiveTime && isground == false)
        {
            KeyDownJump = true;

        }

    }
    [SerializeField] private float UpSpeed=0.01f;
    public void EdgeDetection()
    {
        bool high = Highcheck.IsCollision();
        bool low = Lowcheck.IsCollision();
        Transform temp = Lowcheck.GetCollisionTransform();
        if (high != true && low == true&&isground==false)
        {
           rb.velocity.Set(rb.velocity.x, 0);
           // var dir = transform.position-temp.position;
            this.transform.position += new Vector3(0, UpSpeed, 0);
        }
     }
    ////预输入跳跃 
    //   if (lastGroundtime - lastPressKey <= JumpPredictiveTime && isground==false)
    //      KeyDownJump = true;

    public readonly float moveSpeed = 6f;
    public readonly float jumpSpeed = 10f;


    //-------------------------------------------
    //冲刺CD为1秒 同时冲刺所需要的时间为<=0.4s
    public readonly float sprintCD = 0.8f;
    public readonly float sprintSpeed = 20f;
  [SerializeField]  private float _LastpressSprintKey = 0;
    public bool CDcanSprint = true;
    private bool KeyDownSprint = false;
    public bool isSprint = false;
    private int sprintTimes = 1;
    public float LastPressSprintKey { get { return _LastpressSprintKey; } }
    public void SetLastPressSprintKey(float t)
    {
        _LastpressSprintKey = t;
    }
    
    //冲刺条件判断
    public bool FinallyPrintCheck()
    {
        //CD 按下冲刺 无交互情况下只能冲一次
        if (CDcanSprint && KeyDownSprint && sprintTimes > 0)
            return true;
        else return false;
    }
    public void SetSprintOneTimes()
    {
        sprintTimes = 1;
    }
    public void SetSprintZeroTimes()
    {
        sprintTimes = 0;
    }
    //--------------------------------------
    //受伤相关
    public void SetInvincible(bool Invinvible) //受伤时候会调用 翻滚时候会调用 弹反时候会调用
    {
        isInvincible = Invinvible;
    }

    public void TakeHit(int hp,Transform where)
    {
        
        if (isInvincible == true ||isDead == true) return; //无敌帧情况下被攻击不被击退不会受伤  死亡情况下也不会
        if(isBlock ==false)
        {
            HP = HP - hp;
            UIManager.Instance.ChangeHPUI(HP,MAXHP);
            if (HP > 0)
            {
                SwitchStatus(PlayerStatus.hit);
                _states[PlayerStatus.hit].Other(where);
                //无敌帧显示
                InvincibleRendered(InvincibleFrameTime);
            }
            else
            {
                HP = 0;
                SwitchStatus(PlayerStatus.died);
            }
        }
        else
        {
            SwitchStatus(PlayerStatus.block);
        }
    }
    //用于无敌帧的闪烁显示
    public void InvincibleRendered(float t)
    {
        Debug.Log("白色闪烁");
        
        TimeManager.Instance.FrameTime(t,
            () =>
            {
                float e = Mathf.PingPong(Time.time * 4f, 0.7f);
                Color color = new Color(e, e, e);
                material.color = color;

                
            },
            ()=>
            {
                material.color= Color.black;
            }
            );
        
    }

    [SerializeField]public float InvincibleFrameTime = 1.2f;
    private bool isInvincible = false;
    private bool isBlock = false;
    Vector2 ImpactDir ;



    //--------------------
    //攻击相关

    bool FinallyAttackCheck()
    {
        //之后的检测用状态机去检查 而非bool
        if (isKeyDownAttack == true && isAttacking != true&&isSprint!=true &&currentStatus!=PlayerStatus.hit&&IsGrounded())
        {
            canAttack = true;
            SwitchStatus(PlayerStatus.attack);
        }
        else
        {

            canAttack = false;
        }


        return canAttack;
    }



    

    private bool isKeyDownAttack = false;
    public bool IsKeyDownAttack { get { return isKeyDownAttack; } }
   public bool isAttacking = false;
    bool canAttack = false;

    
    //------------------
    private Rigidbody2D rb;
    protected Vector2 moveInput;
    public readonly float groundFriction =40f;
    public bool KeyDownJump = true;
    public bool canJump = true;
   [SerializeField]  private  bool isground = true;
    protected bool isfacingleft = false;
   public  bool IsFacingLeft {  get { return isfacingleft; } }

    public int MAXHP = 100;
   public int HP = 100;
   bool isDead = false;
   
   

    public  float h;
    public  float v;
    
    


}
