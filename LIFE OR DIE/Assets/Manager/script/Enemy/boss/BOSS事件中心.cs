using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS事件中心 : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private EnemyAttackHitBox hitbox;

    private Animator animator;
    private string lastName;
    //  private BehaviorTree bt;


    public string CurrentAttack { get; private set; }

    void Start()
    {
        anim = GetComponent<Animator>();
        hitbox = GetComponent<EnemyAttackHitBox>();
        //bt = GetComponent<BehaviorTree>();
    }

    //要提供可以停止动画 重置动画的方法

  



    private void CloseAllBool()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("FiveAttack", false);
        anim.SetBool("hit", false);
        anim.SetBool("run", false);
        anim.SetBool("JumpAttack", false);
        anim.SetBool("jump", false);
        anim.SetBool("fall",false);
        anim.SetBool("throw", false);
        anim.SetBool("quick", false);
        anim.SetBool("push",false);
    }
    public void SetBool(string name)
    {
        //CloseAllBool();
        if(lastName!=null)
        {
            anim.SetBool(lastName, false);
        }
        anim.SetBool(name, true);
        lastName = name;
    }


    public void Attack1()
    {
        hitbox.OnAnimEvent_FireAttack("attack1");
        AudioManager.Instance.PlaySFX("挥击嗖_1");
    }
    public void Attack2()
    {
        hitbox.OnAnimEvent_FireAttack("attack2");
        AudioManager.Instance.PlaySFX("挥击嗖_2");
    }
    public void Attack3()
    {
        hitbox.OnAnimEvent_FireAttack("attack3");
        AudioManager.Instance.PlaySFX("挥击嗖_1");
    }
    public void Attack4()
    {
        hitbox.OnAnimEvent_FireAttack("attack4");
        AudioManager.Instance.PlaySFX("挥击嗖_2");
    }
  public void Attack5()
    {
        hitbox.OnAnimEvent_FireAttack("attack5");
        AudioManager.Instance.PlaySFX("挥击嗖_3");
    }
    public void SpecialAttack()
    {
        hitbox.OnAnimEvent_FireAttack("specialAttack");
        AudioManager.Instance.PlaySFX("HeavykillBlood_1");
        CameraManager.Instance.CameraShake(0.5f, 0.8f);
       
    }
}
