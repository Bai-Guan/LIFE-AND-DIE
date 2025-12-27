using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BOSS状态基类 : IBossState
{
    protected BOSSAI控制器 AIFsm;
    protected BOSS状态基类(BOSSAI控制器 aIFsm)
    {
        AIFsm = aIFsm;
    }

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();


    public abstract void FixedUpdate();
}
public interface IBossState
{
    public void Enter();
    public void Update();
    public void FixedUpdate();
    public void Exit();
}
public enum BOSSAITypeState
{
    waitPlayer,
    ldle,
    run,
    jumpMid,
    jumpAttack,
    fiveAttack,
    hit,
    throwStones,
    squeeze,
    phaseTwoStandby,
    other
}

public class BOSSAIFSM : MonoBehaviour
{


    public BOSSAITypeState curState;
    public IBossState ICurrentState;
    public BackGround backGround;
    public Dictionary<BOSSAITypeState, IBossState> _dicTypeState;


    public BOSSAIFSM()
    {
        _dicTypeState = new Dictionary<BOSSAITypeState, IBossState>();
        // this.backGround = backGround;
        curState = BOSSAITypeState.ldle;

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

    public void AddState(BOSSAITypeState newStatus, IBossState IEnemy)
    {
        _dicTypeState.Add(newStatus, IEnemy);
    }

    public void SwitchStatus(BOSSAITypeState newStatus)
    {

        if (!_dicTypeState.ContainsKey(newStatus))
        {
            Debug.LogError($"状态 {newStatus} 未在状态机中注册！");
            return;
        }
        if (curState == newStatus) return;


        IBossState nextState = _dicTypeState[newStatus];
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