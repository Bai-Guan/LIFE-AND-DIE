using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using ClipperLib;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class 检测距离玩家距离 : Conditional
{
    
    public SharedFloat targetDistanceX = 7f;
    public SharedFloat targetDistanceY = 0f;
    public SharedFloat randomFloat = 1f;
    public SharedFloat hysteresis = 0.5f;
    public SharedFloat lockedCD = 6f;
    public SharedBool 是否带距离锁 = false;
    private BehaviorTree bt;
    private EnemyRadioGraphic erg;
    private bool islocked = false;
    private Vector2 targetPos;
    

    public override void OnAwake()
    {
        bt = GetComponent<BehaviorTree>();
        erg = GetComponent<EnemyRadioGraphic>();
    }
    public override void OnStart()
    {
        targetPos = erg.PlayerPosition;
        SharedVector2 temp = targetPos;
        bt.SetVariable("玩家位置", temp);


    }

    public override TaskStatus OnUpdate()
    {

        // 1. 取玩家位置
        targetPos = erg.PlayerPosition;
        if (targetPos == null)
        {
            Debug.Log("获取位置失败");
            return TaskStatus.Failure;
        }

        // 2. 只算横向距离（y 不限）
        float distanceX = Mathf.Abs(transform.position.x - targetPos.x);
       // Debug.Log(distanceX);
       
        //不带距离锁的情况
        if (distanceX <= targetDistanceX.Value && 是否带距离锁.Value==false)
        {
            if (Random.Range(0f, 1f) <= randomFloat.Value)
                return TaskStatus.Success;
        }

        //带距离锁
        if ( distanceX <= targetDistanceX.Value && 是否带距离锁.Value == true && islocked==false)
        {
           
            // 概率触发
            if (Random.Range(0f, 1f) <= randomFloat.Value)
            {

                islocked = true;
                TimeManager.Instance.OneTime(lockedCD.Value, () =>
                {

                    islocked = false;
                });
               
                return TaskStatus.Success;
            }
        }

        //// 4. 离开判定（滞回只给 x）
        //if ( distanceX > targetDistanceX.Value + hysteresis.Value)
        //{
           
        //    islocked = false;
        //}

        return TaskStatus.Failure;

    }

}
