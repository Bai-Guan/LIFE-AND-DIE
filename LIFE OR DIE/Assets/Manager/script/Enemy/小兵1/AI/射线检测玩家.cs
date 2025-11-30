using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 射线检测玩家 :Conditional
{
    public SharedBool 是否发现玩家;
    private EnemyRadioGraphic detector;
    private bool hasDetector;
   private BehaviorTree bt;
    public override void OnStart()
    {
        // 在开始时获取组件，避免每次Update都调用GetComponent
        detector = GetComponent<EnemyRadioGraphic>();
       bt= GetComponent<BehaviorTree>();
        hasDetector = detector != null;

        if (!hasDetector)
        {
            Debug.LogError("射线检测玩家: 未找到EnemyRadioGraphic组件", gameObject);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (!hasDetector)
            return TaskStatus.Failure;
        是否发现玩家 = detector.IsPlayerVisible;
        if (是否发现玩家.Value)
        {
            SharedVector2 temp = detector.PlayerPosition;
            bt.SetVariable("玩家位置",temp);
            bt.SetVariable("是否看见了玩家", 是否发现玩家);
        }
        return detector.IsPlayerVisible ? TaskStatus.Success : TaskStatus.Failure;
    }
}
