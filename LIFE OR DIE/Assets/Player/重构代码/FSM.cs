using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Playables;


public enum TypeState
{
    ldle,
    run,
    jump,
    fall,
    Unexpected,
    sprint,
    block,
    attack,
    died,
    hit,
    collision,
    other
}
public interface IPlayerState
{
    public void Enter();
    public void Update();
    public void FixedUpdate();
    public void Attack();
    public void Dodge();

    public void ContractPower();
    public void Exit();
}
[SerializeField]
public class BackGround
{

}
public class FSM 
{
    public TypeState curState;
    public IPlayerState ICurrentState;
    public BackGround backGround;
   public Dictionary<TypeState, IPlayerState> _dicTypeState;
   public FSM()//(BackGround backGround)
    {
        _dicTypeState = new Dictionary<TypeState, IPlayerState>();
        // this.backGround = backGround;
        curState = TypeState.ldle;
        
    }

    public void Attack()
    {
       ICurrentState.Attack();
    }

    public void ContractPower()
    {
        ICurrentState.ContractPower();
    }

    public void Dodge()
    {
        ICurrentState.Dodge();
    }

    public void Enter()
    {
        ICurrentState.Enter();
    }

    public void Exit()
    {
        ICurrentState.Exit();
    }

    public void FixedUpdate()
    {
      //  if (ICurrentState == null) { Debug.LogWarning("这tm为空");return; }
        ICurrentState.FixedUpdate();
    }

 

    public void Update()
    {
        //if (ICurrentState == null) { Debug.LogWarning("这tm为空"); return; }
        ICurrentState.Update();
    }

    public void AddState(TypeState newStatus,IPlayerState IPlayer)
    {
        _dicTypeState.Add(newStatus, IPlayer);
    }

    public void SwitchStatus(TypeState newStatus)
    {
        
        if (!_dicTypeState.ContainsKey(newStatus))
        {
            Debug.LogError($"状态 {newStatus} 未在状态机中注册！");
            return;
        }
        if (curState == newStatus) return;


        IPlayerState nextState = _dicTypeState[newStatus];
        if (nextState == null)
        {
            Debug.LogError($"状态 {nextState} 的实例为 null！");
            return;
        }

        ICurrentState?.Exit();    // 先离开当前状态
        curState = newStatus;
        ICurrentState = _dicTypeState[newStatus];
        ICurrentState.Enter();    // 再进入新状态
    }
}
