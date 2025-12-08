using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 追击玩家:  Action
{
    public SharedFloat moveSpeed = 4f;
    public SharedBool isFacingleft;
    public SharedFloat 追击时间 = 5f;
    public SharedFloat 离目标最近距离 = 2.5f;
    private InitEnemySystem body;
    private Rigidbody2D rb;
    private Transform player;
    private BehaviorTree bt;
    private EnemyRadioGraphic rg;
    private float timer=0;
    private bool 是否发现玩家=false;

    public override void OnAwake()
    {
        body = GetComponent<InitEnemySystem>();
        isFacingleft = body.isFacingLeft;
        bt = GetComponent<BehaviorTree>();
        rb = GetComponent<Rigidbody2D>();
        rg = GetComponent<EnemyRadioGraphic>();
        player = GameObject.Find("MainPlayer").transform;
    }
    public override void OnStart()
    {
        timer = 0;



    }

    public override TaskStatus OnUpdate()
    {
        if (rg.IsPlayerVisible == false)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        if (timer >= 追击时间.Value)
        {
            Debug.Log("爷我不追了");
            return TaskStatus.Failure;
        }
        float temp = Mathf.Abs(this.transform.position.x - player.position.x);
        if (temp <= 离目标最近距离.Value)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return TaskStatus.Success;
         }
        bool filp = this.transform.position.x - player.position.x > 0 ? true : false;
        int filpi = filp ? -1 : 1;
        isFacingleft = filp;
        body.SetFilp(filpi);
        rb.velocity = new Vector2(moveSpeed.Value*filpi, rb.velocity.y);
       
        是否发现玩家 = (bt.GetVariable("是否看见了玩家") as SharedBool).Value;



            //如果丢失视野并且玩家与敌人Y距离大于一定距离 也不追
        
            
        return TaskStatus.Running;
    }
    public override void OnEnd()
    {
        //rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
