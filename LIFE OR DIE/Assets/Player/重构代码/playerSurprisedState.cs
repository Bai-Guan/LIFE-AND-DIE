using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSurprisedState : IPlayerState
{
    //实现卡肉  攻击到时候卡一下 收刀时候再卡一下
    private NewPlayerControll _ctx;
    public playerSurprisedState(NewPlayerControll playerControll)
    {
        _ctx = playerControll;
    }
    bool ischose = false;
   float timer = 0;
    public void Enter()
    {
        //不负责减少生命
        Debug.Log("进入奇点状态");
        ischose = false;
       timer = 0;
        _ctx.Anim.TriggerHurt();
        AudioManager.Instance.PlaySFX("处决");
        EffectManager.Instance.VerticalBlur(1.5f, 0.6f);
        EffectManager.Instance.ChromaticAberrationSet(1.5f, 0.6f);
    }

    public void Attack()
    {
        if(ischose)return;
        //播放攻击动画 
        _ctx.Anim.TriggerAttack2();
        ischose = true;
      
    }

    public void ContractPower()
    {
        if (ischose) return;
    }

    public void Dodge()
    {
        if (ischose) return;
    }

  

   
    public void FixedUpdate()
    {
        if (ischose) return;
        timer += Time.fixedDeltaTime;
    }

  
   public void Update()
    {
        if (ischose) return;
        if(timer > 0.6f) _ctx.SwitchState(TypeState.ldle);
        //如果按了跳 则跳的很高
    }

    public void Exit()
    {

    }

    public void Block()
    {
      
    }
}
