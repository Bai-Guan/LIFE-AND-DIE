using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 弓箭手状态机 : MonoBehaviour,IEnemyReset
{
    private AIFSM fSM;
    private IEnemyState IcurrentState;
    private AITypeState currentType;

    public GameObject MainPlayer;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public EnemyRadioGraphic 射线检测;
    [SerializeField] public EnemyRigidbar 僵直条;
    [SerializeField] public 弓箭手事件中心 动画事件中心;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public InitEnemySystem body;
    [SerializeField] private DamagedComponent 受伤模块;
    [SerializeField] private AITypeState 当前状态;

    public float 闪避概率 { get { return 0.3f; } private set { } }
  [SerializeField]  private bool 闪避锁 = false;//射手只能闪避一次
    public bool clock { get { return 闪避锁; } }
    public float 闪避移动速度 = 7f;
    public bool 是否为初见玩家 = true;
    public bool 玩家是否死亡过一次 = false;

    public GameObject 飞箭本体;
    public float 飞箭速度 = 12f;


    public float 僵直时间 = 0.4f;
    [Tooltip("决策间隔")]
    [SerializeField] private float decisionInterval = 0.3f; // 每0.3秒才判一次
    public float 决策间隔 { get { return decisionInterval; } }
    [Tooltip("距离玩家的长度")]
    [SerializeField] private float _distanceXPlayer = 0;
    public float 水平距离玩家距离
    {
        get
        {
            return _distanceXPlayer;
        }
    }
    public int 朝向 = 1;



    private void Awake()
    {
        fSM = new AIFSM();
        fSM.curState = AITypeState.ldle;
        当前状态 = fSM.curState;
        fSM.AddState(AITypeState.ldle, new 射手待机(this));
        fSM.AddState(AITypeState.hit, new 射手受伤(this));
        fSM.AddState(AITypeState.attack, new 射手射箭(this));
        fSM.AddState(AITypeState.thrust,new 射手闪避(this));
        fSM.AddState(AITypeState.preblock, new 射手拉弦(this));

        fSM.ICurrentState = fSM._dicTypeState[fSM.curState];
    }
    void Start()
    {
        if (MainPlayer == null)
            MainPlayer = GameObject.Find("MainPlayer");
    }


    void Update()
    {
        fSM.Update();
    }
    private void FixedUpdate()
    {
        _distanceXPlayer = Mathf.Abs(transform.position.x - MainPlayer.transform.position.x);
        fSM.FixedUpdate();
    }

    public void SwitchState(AITypeState state)
    {
        当前状态 = state;
        fSM.SwitchStatus(state);
    }
    //为ai状态机提供的方法
    public void SetFilp(float dir)
    {
        body.SetFilp(dir);
    }

    public void CheckRb()
    {
        if (僵直条.检测是否僵直())
        {
            SwitchState(AITypeState.hit);
            僵直条.清空僵直条();
        }
    }
    public void SetInvincible(bool tf)
    {
        受伤模块.SetInvincible(tf);
    }
    public void 设置受伤倍率(int mul)
    {
        受伤模块.设置伤害倍率(mul);
    }
    public void SetFacing(float dir) => SetFilp(dir);   // dir=-1 左，1 右

    public  void 放箭(Vector2 dir,float angle)
    {

        // 3. 生成箭
        GameObject arr = Instantiate(飞箭本体, transform.position,
                                     Quaternion.Euler(0, 0, angle));

        // 4. 给速度（不受重力）
        Rigidbody2D rb = arr.GetComponent<Rigidbody2D>();
        rb.velocity = dir * 飞箭速度;
        rb.gravityScale = 0;
        //5设置不攻击谁
        弓箭碰撞逻辑 temp = arr.GetComponent<弓箭碰撞逻辑>();
        temp.设置发射者(this.gameObject);
    }

    public void 设置闪避锁(bool tf)
    {
        闪避锁=tf;
    }

    public void EnemyReset()
    {
        SwitchState(AITypeState.ldle);
        是否为初见玩家 = true;
        body.ResetHP();
        僵直条.清空僵直条();
       
    }
}
