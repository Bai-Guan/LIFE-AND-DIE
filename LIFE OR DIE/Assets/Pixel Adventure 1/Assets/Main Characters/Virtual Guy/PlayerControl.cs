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
        //����
        h = Input.GetAxisRaw("Horizontal");   // -1, 0, 1
        v = Input.GetAxisRaw("Vertical");
        moveInput.x = h; moveInput.y=v;


        playerState?.Update();
        
    }
    //����Ч��д��FixedUpdate
    private void FixedUpdate()
    {
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


    public readonly float moveSpeed = 8f;
    public readonly float jumpSpeed = 12f;

    private Rigidbody2D rb;
    protected Vector2 moveInput;

  
    protected bool isfacingleft = false;
    public  float h;
    public  float v;




}
