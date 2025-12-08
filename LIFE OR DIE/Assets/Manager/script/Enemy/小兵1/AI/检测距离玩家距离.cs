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
    public SharedFloat 判断成功持续时间 = 3f;
    public SharedBool 是否带距离锁 = false;
    private BehaviorTree bt;
    private EnemyRadioGraphic erg;
    private bool islocked = false;
    private Vector2 targetPos;

    private float timer=0;
    private bool istrue = false;
    private bool 当前是否在攻击 = false;
    private bool 是否看见了玩家 = false;
    public override void OnAwake()
    {
        bt = GetComponent<BehaviorTree>();
        erg = GetComponent<EnemyRadioGraphic>();
        
    }
    public override void OnStart()
    {
       

        //if (timer >= 判断成功持续时间.Value)
        //    timer = 0;
      
    }

    public override TaskStatus OnUpdate()
    {
        当前是否在攻击 = (bt.GetVariable("在攻击") as SharedBool).Value;
        targetPos = erg.PlayerPosition;
        是否看见了玩家 = erg.IsPlayerVisible;
        if (当前是否在攻击 ==true)
            return TaskStatus.Failure;

        //if (istrue&&timer<判断成功持续时间.Value)
        //{
        //    timer += Time.deltaTime;
        //    return TaskStatus.Success;
        //}

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
        if (distanceX <= targetDistanceX.Value && 是否带距离锁.Value==false&&是否看见了玩家)
        {
            if (Random.Range(0f, 1f) <= randomFloat.Value)
            {
                istrue = true;
                return TaskStatus.Success;
            }
               
        }

        //带距离锁
        if ( distanceX <= targetDistanceX.Value && 是否带距离锁.Value == true && islocked== false&&是否看见了玩家)
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
