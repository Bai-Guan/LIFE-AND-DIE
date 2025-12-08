using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 小怪动画事件 : MonoBehaviour
{
   [SerializeField] private Animator anim;

   
    private Animator animator;
    private EnemyAttackHitBox attackHitBox;
  //  private BehaviorTree bt;

    public bool IsAttacking { get; private set; }
    public bool IsAttackComplete { get; private set; }
    public string CurrentAttack { get; private set; }

    void Start()
    {
        anim = GetComponent<Animator>();
        attackHitBox=GetComponent<EnemyAttackHitBox>();
        //bt = GetComponent<BehaviorTree>();
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
    //public void PlayAttackAnimation(string attackName)
    //{
    //    if (anim != null)
    //    {
    //        anim.SetTrigger(attackName);
    //    }
    //}
    //这个方法由动画调用
    public void 动画调用播放特效()
    {
        AudioManager.Instance.PlaySFX("轻弹刀");
        EffectManager.Instance.Play("火花效果",this.transform);

        EffectManager.Instance.全局慢动作(1f);
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
    public void TriggerWillBlock()
    {
        anim.SetTrigger("willblock");
    }

}