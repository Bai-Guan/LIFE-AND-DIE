using System.Collections;
using System.Collections.Generic;
using UnityEngine;




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
        other
    }
    private PlayerStatus currentStatus;
    private PlayerState playerState;
    private Dictionary<PlayerStatus, PlayerState> _states = new Dictionary<PlayerStatus, PlayerState>();
    private Player_checkGround player_CheckGround;
    private  SpriteRenderer spriteRenderer;
    private void Awake()
    {
        _states[PlayerStatus.ldle] = new IdleState(this);
        _states[PlayerStatus.run] = new RunState(this);
        _states[PlayerStatus.jump] = new JumpState(this);
        _states[PlayerStatus.fall] = new FallState(this);
        _states[PlayerStatus.other] = new OtherState(this);
        rb = GetComponent<Rigidbody2D>();
        player_CheckGround = GetComponentInChildren<Player_checkGround>();
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentStatus = PlayerStatus.other;
        SwitchStatus(PlayerStatus.ldle);

    }


    void Update()
    {
        //更新
        h = Input.GetAxisRaw("Horizontal");   // -1, 0, 1
        v = Input.GetAxisRaw("Vertical");
        moveInput.x = h; moveInput.y = v;


        playerState?.Update();

    }
    //物理效果写在FixedUpdate
    private void FixedUpdate()
    {
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
    public bool IsGrounded()
    {
       return player_CheckGround.CheckGround();
    }

    public void SetGrounded(bool what)
    {
        player_CheckGround.SetIsGround(what);
    }

    public void FacingLeft()
    {
        spriteRenderer.flipX = true;
        isfacingleft = true;
    }
    public void FacingRight()
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
    public bool GetDoubleJump()
    {
        return canDoubleJump;
    }

    public void SetDoubleJump(bool yesORno)
    {
        canDoubleJump = yesORno;
    }

    public readonly float moveSpeed = 6f;
    public readonly float jumpSpeed = 12f;

    private Rigidbody2D rb;
    protected Vector2 moveInput;
    public readonly float groundFriction =40f;
  
    protected bool isfacingleft = false;
    //是否可以跳跃由碰撞检测进行判断
    private bool canJump = true;
    private bool canDoubleJump = true;

    public  float h;
    public  float v;
    
    


}
