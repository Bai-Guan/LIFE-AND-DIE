using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 冲刺 : Action
{
    public SharedFloat 冲刺瞬时速度 = 10f;
    private Rigidbody2D rb;
    private InitEnemySystem body;
    private int isfacingleft;
    // Start is called before the first frame update
    public override void OnAwake()
    {
        rb= GetComponent<Rigidbody2D>();
        body = GetComponent<InitEnemySystem>();
    }
    public override void OnStart()
    {
       isfacingleft=body.isFacingLeft?-1:1;
     //   Debug.Log("怪物冲刺");
    }
    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        rb.velocity = new Vector2(冲刺瞬时速度.Value * isfacingleft,rb.velocity.y);
       return TaskStatus.Success;
    }
}
