using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;




public class PlayerControl : MonoBehaviour
{
    //��д ����ĳ��״̬ ĳ��״̬update �뿪ĳ��״̬  ѡ��ĳ��״̬ ��Щ����
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
        //����
        h = Input.GetAxisRaw("Horizontal");   // -1, 0, 1
        v = Input.GetAxisRaw("Vertical");
        moveInput.x = h; moveInput.y = v;

        //��Ծ���
        if (v>0.1f)
        {
            KeyDownJump = true;
        }
        else if (v<0.1f)
        {
            KeyDownJump = false;
        }
        //��̼��
        if(Input.GetKeyDown(KeyCode.L))
        {
            KeyDownSprint = true;
            
        }
        else if(Input.GetKeyUp(KeyCode.L)) KeyDownSprint = false;

        //�������
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
    //����Ч��д��FixedUpdate
    private void FixedUpdate()
    {
        EdgeDetection();
        playerState?.FixedUpdate();
    }
    public void SwitchStatus(PlayerStatus newStatus)
    {
        if (currentStatus == newStatus) return;
        playerState?.Exit();    // ���뿪��ǰ״̬
        currentStatus = newStatus;
        playerState = _states[newStatus];
        playerState.Enter();    // �ٽ�����״̬
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

        //��⳯��
        if (h > 0.5f)
        {
           FacingRight();
        }
        if (h < -0.5f)
        {
            //Debug.LogWarning("�����ˣ�");
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
        // ����ǽ�ڼ��
        bool isTouchingWall = IsTouchingWall;

        // ֻ�е���ͼ��ǽ�ڷ����ƶ�ʱ������
        if (isTouchingWall && Mathf.Sign(h) == (isfacingleft ? -1 : 1))
        {
        return;
        }
        else
        {
            // �����ƶ�
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
        // ����ǽ�ڼ��
        bool isTouchingWall = IsTouchingWall;

        // ֻ�е���ͼ��ǽ�ڷ����ƶ�ʱ������
        if (isTouchingWall && Mathf.Sign(h) == (isfacingleft ? -1 : 1))
        {
            return;
        }
        else
        {
            // �����ƶ�
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

        // ʹ��BoxCast���һ����������ǵ�����
        Vector2 boxSize = new Vector2(0.1f, 1f); // ��������С
        Vector2 boxOrigin = new Vector2(transform.position.x, transform.position.y+1f);

        RaycastHit2D hit = Physics2D.BoxCast(
            boxOrigin,
            boxSize,
            0f,
            Vector2.right * dir,
            RayLength,
            layer
        );

        // ���ӻ�����
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

    //������Ծ��ز���
    
    [SerializeField] private float lastGroundtime = 0;
    [SerializeField] private float lastPressKey = 0;
    //����������Ϳ�ʱ��
  [SerializeField]  public float coyoteTime = 0.15f;
    //Ԥ����ʱ��
    public float JumpPredictiveTime = 0.5f;
    //������һ��ʱ���ڱ�֤��Ծ���Ա���ǰ����
    //���ڼ���Ƿ������Ծ  �Լ�����ʱ��
    public bool FinallyJumpCheck()
    {


        if (isground)
        {
            lastGroundtime = Time.time;
            canJump = true;
            
        }
        else
        {
            //���ڵ���ʱ�� ����ʱ����������Ծ ����ʱ������ ������Ծ
            if (Time.time - lastGroundtime <= coyoteTime) canJump = true;
            else canJump = false;
        }
     

        return canJump;
    }

 private float waitTime = 0.2f;
    void PredictiveJumpTime()
    {
        //��Ծ������ �ú�����0.2����ÿ֡�������
        
        //���Ϳ�ʱ��ʱ������Ұ���Ծ����������Ծ������һ��ʱ����ٱ�Ϊ����Ծ
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
    ////Ԥ������Ծ 
    //   if (lastGroundtime - lastPressKey <= JumpPredictiveTime && isground==false)
    //      KeyDownJump = true;

    public readonly float moveSpeed = 6f;
    public readonly float jumpSpeed = 10f;


    //-------------------------------------------
    //���CDΪ1�� ͬʱ�������Ҫ��ʱ��Ϊ<=0.4s
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
    
    //��������ж�
    public bool FinallyPrintCheck()
    {
        //CD ���³�� �޽��������ֻ�ܳ�һ��
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
    //�������
    public void SetInvincible(bool Invinvible) //����ʱ������ ����ʱ������ ����ʱ������
    {
        isInvincible = Invinvible;
    }

    public void TakeHit(int hp,Transform where)
    {
        
        if (isInvincible == true ||isDead == true) return; //�޵�֡����±������������˲�������  ���������Ҳ����
        if(isBlock ==false)
        {
            HP = HP - hp;
            UIManager.Instance.ChangeHPUI(HP,MAXHP);
            if (HP > 0)
            {
                SwitchStatus(PlayerStatus.hit);
                _states[PlayerStatus.hit].Other(where);
                //�޵�֡��ʾ
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
    //�����޵�֡����˸��ʾ
    public void InvincibleRendered(float t)
    {
        Debug.Log("��ɫ��˸");
        
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
    //�������

    bool FinallyAttackCheck()
    {
        //֮��ļ����״̬��ȥ��� ����bool
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
