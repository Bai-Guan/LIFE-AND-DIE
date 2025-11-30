using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 小怪动画事件 : MonoBehaviour
{
   [SerializeField] private Animator anim;

   
    private Animator animator;
    private EnemyAttackHitBox attackHitBox;

   
    public bool IsAttacking { get; private set; }
    public bool IsAttackComplete { get; private set; }
    public string CurrentAttack { get; private set; }

    void Start()
    {
        anim = GetComponent<Animator>();
        attackHitBox=GetComponent<EnemyAttackHitBox>();
    }

    //要提供可以停止动画 重置动画的方法

    public void OnAnimEventFireAttack(string attackName)
    {
        if (attackHitBox != null)
        {
            attackHitBox.OnAnimEvent_FireAttack(attackName);
            IsAttacking = true;
            IsAttackComplete = false;
            CurrentAttack = attackName;
        }
    }
    // 这个方法可以由行为树调用
    public void PlayAttackAnimation(string attackName)
    {
        if (anim != null)
        {
            anim.SetTrigger(attackName);
        }
    }
    //这个方法由动画调用
    
    //这个方法由动画调用
    public void OnAttackEnd()
    {
        IsAttacking = false;
        IsAttackComplete = true;
        CurrentAttack = "";
    }

    public void Play(string name)
    {
        anim.SetTrigger(name);
    }

    public void TriggerAttack()
    {
        anim.SetTrigger("attack");
    }
    public void TriggerSpecialAttack()
    {
        anim.SetTrigger("specialAttack");
    }
    public void TriggerRun()
    {
        anim.SetTrigger("run");
    }
    public void TriggerHit()
    {
        anim.SetTrigger("hit");
    }
    public void TriggerIdle()
    {
        anim.SetTrigger("idle");
    }
    public void TriggerBlock()
    {
        anim.SetTrigger("block");
    }

}