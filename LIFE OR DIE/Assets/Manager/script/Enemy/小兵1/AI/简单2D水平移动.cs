using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 简单2D水平移动 : Action
{
    public SharedFloat moveSpeed =4f;
    private BehaviorTree bt;
    private Rigidbody2D rb;
    private Vector2 目标位置;

    public override void OnAwake()
    {
        bt = GetComponent<BehaviorTree>();

        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnStart()
    {
       
       目标位置=((SharedVector2)bt.GetVariable("玩家位置")).Value;
    }
    public override TaskStatus OnUpdate()
    {
        bool filp=this.transform.position.x- 目标位置.x > 0?true:false;
        int filpi=filp?-1:1;
        rb.velocity = new Vector2(filpi * moveSpeed.Value, 0f);
        float temp = Mathf.Abs(this.transform.position.x - 目标位置.x);
        if(temp<=0.2f)
            return TaskStatus.Success;
       return TaskStatus.Running;
    }

}
