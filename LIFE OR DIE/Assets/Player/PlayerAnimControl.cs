using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAnimControl : MonoBehaviour
{
    private const string SPRINT_PARAM = "Sprint";
    private const string BLOCK_PARAM = "Block";
    private const string JUMP_PARAM = "Jump";
    private const string HURT_PARAM = "Hurt";
    private const string FALL_PARAM = "Fall";
    private const string DIE_PARAM = "Death";
    private const string ATTACK1_PARAM = "Attack1";
    private const string ATTACK2_PARAM = "Attack2";
    private const string ATTACK3_PARAM = "Attack3";
    float speedY;

    bool Grounded =false;
    bool noBlood = false;

    bool isRun = false;


    [Header("References")]
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
       _anim = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }
    private void Start()
    {
        
    }
    //jump这些用trigger 跑步这些用bool
    private void Update()
    {
        UpdateDate();
    
     }

    private void UpdateDate()
    {
        Grounded = playerControl.IsGrounded();
        _anim.SetBool("Grounded", Grounded);

        speedY = playerControl.GetRigidbody2D().velocity.y;
        _anim.SetFloat("AirSpeedY", speedY);

        if(playerControl.h!=0)isRun = true;
        else isRun = false;
        _anim.SetBool("IsRun",isRun);
    }

    public void TriggerJump()
    {
     _anim.SetTrigger(JUMP_PARAM);

    }

 public void TriggerDie()
    {
        _anim.SetTrigger(DIE_PARAM);
    }

    public  void TriggerHurt()
    {
        _anim.SetTrigger(HURT_PARAM);
    }

    public void TriggerBlock()
    {
        _anim.SetTrigger(BLOCK_PARAM);

    }
    public void TriggerSprint()
    {
        _anim.SetTrigger(SPRINT_PARAM);

    }
    public void TriggerAttack1()
    {
        _anim.SetTrigger(ATTACK1_PARAM);

    }
    public void TriggerAttack2()
    {
        _anim.SetTrigger(ATTACK2_PARAM);

    }
    public void TriggerAttack3()
    {
        _anim.SetTrigger(ATTACK3_PARAM);

    }

}
