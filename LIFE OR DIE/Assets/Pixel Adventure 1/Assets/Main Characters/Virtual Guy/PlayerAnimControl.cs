using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    private const string IDLE_PARAM = "Idle";
    private const string RUN_PARAM = "Run";
    private const string JUMP_PARAM = "Jump";
    private const string DOUBLE_PARAM = "DoubleJump";
    private const string FALL_PARAM = "Fall";
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
    //jump这些用trigger 跑步这些用bool
    public void TriggerJump()
    {
        _anim.SetBool(JUMP_PARAM, true);

    }
    public void TriggerDoublEJump() => _anim.SetTriger(DOUBLE_PARAM);
    public void TriggerFall() => _anim.SetTriger(FALL_PARAM);
    public void TriggerIdle() => _anim.SetTrigger(IDLE_PARAM);
    public void TriggerRUN() => _anim.SetTrigger(RUN_PARAM);

}
