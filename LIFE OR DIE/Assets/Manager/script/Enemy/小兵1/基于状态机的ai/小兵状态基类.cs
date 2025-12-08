using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class 小兵状态基类 : IEnemyState
{
    protected 小怪状态机AI AIFsm;
    protected 小兵状态基类(小怪状态机AI aIFsm)
    {
        AIFsm = aIFsm;
    }

    public abstract void Attack();


    public abstract void Block();


    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();


    public abstract void FixedUpdate();
 

   

    
  

  
}
