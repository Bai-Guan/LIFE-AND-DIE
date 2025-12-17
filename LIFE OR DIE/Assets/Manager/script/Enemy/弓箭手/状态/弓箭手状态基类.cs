using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class 弓箭手状态基类 : IEnemyState
{
    protected 弓箭手状态机 AIFsm;
    protected 弓箭手状态基类(弓箭手状态机 aIFsm)
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
