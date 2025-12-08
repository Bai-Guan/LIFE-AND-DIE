using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFSM : MonoBehaviour
{
    
    
        public AITypeState curState;
        public IEnemyState ICurrentState;
        public BackGround backGround;
        public Dictionary<AITypeState, IEnemyState> _dicTypeState;

  
        public AIFSM()
        {
            _dicTypeState = new Dictionary<AITypeState, IEnemyState>();
            // this.backGround = backGround;
            curState = AITypeState.ldle;

        }

        public void Attack()
        {
            ICurrentState.Attack();
        }


        public void Block()
        {
            ICurrentState.Block();
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

        public void AddState(AITypeState newStatus, IEnemyState IEnemy)
        {
            _dicTypeState.Add(newStatus, IEnemy);
        }

        public void SwitchStatus(AITypeState newStatus)
        {

            if (!_dicTypeState.ContainsKey(newStatus))
            {
                Debug.LogError($"状态 {newStatus} 未在状态机中注册！");
                return;
            }
            if (curState == newStatus) return;


        IEnemyState nextState = _dicTypeState[newStatus];
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
public enum AITypeState
{
    ldle,
    run,
    jump,
    fall,
    preblock,
    block,
    attack,
    died,
    hit,
    relax,
    alert,
    thrust,
    other
}
public interface IEnemyState
{
    public void Enter();
    public void Update();
    public void FixedUpdate();
    public void Attack();
    public void Block();
    public void Exit();
}