using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerControl : MonoBehaviour
{
    //��д ����ĳ��״̬ ĳ��״̬update �뿪ĳ��״̬  ѡ��ĳ��״̬ ��Щ����
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
    //����Ч��д��FixedUpdate
    private void FixedUpdate()
    {
        playerState?.FixedUpdate();   
    }


    private float moveSpeed = 8f;
    private float jumpSpeed = 12f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    public void SwitchStatus(PlayerStatus playerStatus)
    {
        if (currentStatus == playerStatus) return;
        playerState?.Exit();    // ���뿪��ǰ״̬
        currentStatus = playerStatus;
            playerState.Enter();    // �ٽ�����״̬
    }
    





}
