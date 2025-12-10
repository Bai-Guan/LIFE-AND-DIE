using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 骷髅杂兵动画事件 : MonoBehaviour
{
    [SerializeField] private Animator anim;


    private Animator animator;
    private EnemyAttackHitBox attackHitBox;
    //  private BehaviorTree bt;


    public string CurrentAttack { get; private set; }

    void Start()
    {
        anim = GetComponent<Animator>();
        attackHitBox = GetComponent<EnemyAttackHitBox>();
        //bt = GetComponent<BehaviorTree>();
    }

    //要提供可以停止动画 重置动画的方法

    public void OnAnimEventFireAttack(string attackName)
    {
        if (attackHitBox != null)
        {
            attackHitBox.OnAnimEvent_FireAttack(attackName);
            AudioManager.Instance.PlaySFX("挥击嗖_2");
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


    public void Play(string name)
    {
        anim.SetTrigger(name);
    }

    public void TriggerAttack()
    {
        anim.SetTrigger("attack");
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


}
