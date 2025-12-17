using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 弓箭手事件中心 : MonoBehaviour
{
    [SerializeField] private Animator anim;


    private Animator animator;
   
    //  private BehaviorTree bt;


    public string CurrentAttack { get; private set; }

    void Start()
    {
        anim = GetComponent<Animator>();
       
        //bt = GetComponent<BehaviorTree>();
    }

    //要提供可以停止动画 重置动画的方法

    public void OnAnimEventFireAttack()
    {
        //if (attackHitBox != null)
        //{
        //    attackHitBox.OnAnimEvent_FireAttack(attackName);
        //    AudioManager.Instance.PlaySFX("挥击嗖_1");
        //    CurrentAttack = attackName;
        //}
    }
 

    public void Play(string name)
    {
        anim.SetTrigger(name);
    }

    public void TriggerAttack()
    {
        anim.SetTrigger("attack");
    }

    public void TriggerRoll()
    {
        anim.SetTrigger("roll");
    }
    public void TriggerHit()
    {
        anim.SetTrigger("hit");
    }
    public void TriggerIdle()
    {
        anim.SetTrigger("idle");
    }
    public void TriggerReady()
    {
        anim.SetTrigger("ready");
    }
}
