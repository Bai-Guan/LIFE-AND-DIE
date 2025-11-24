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
    public void Enter()
    {
        Debug.Log("进入奇点状态");
        ischose = false;
        //时间会放慢一下
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
        
    }

  
   public void Update()
    {
        if (ischose) return;
        //如果按了跳 则跳的很高
    }

    public void Exit()
    {

    }

}
