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
        doubleJump,
        fall,
        hit,
        other
    }
    private PlayerStatus currentStatus;
    private PlayerState playerState;
    private Dictionary<PlayerStatus,PlayerState> _states = new Dictionary<PlayerStatus, PlayerState>();

    private void Awake()
    {
        _states[PlayerStatus.ldle] = new IdleState(this);
        _states[PlayerStatus.run] = new RunState(this);
        _states[PlayerStatus.jump] = new JumpState(this);
        _states[PlayerStatus.fall] = new FallState(this);
        _states[PlayerStatus.doubleJump]=new DoubleJumpState(this);
        _states[PlayerStatus.other]= new OtherState(this);
    }

    void Start()
    {
        SwitchStatus(PlayerStatus.ldle);
        rb=GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
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

    private float moveSpeed = 8f;
    private float jumpSpeed = 12f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

  
    protected bool isfacingleft = false;





}
