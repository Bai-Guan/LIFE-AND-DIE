using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class 小怪状态机AI : MonoBehaviour,IEnemyReset
{
  private AIFSM fSM;
    private IEnemyState IcurrentState;
    private AITypeState currentType;

    public GameObject MainPlayer;
    [SerializeField]public SpriteRenderer spriteRenderer;
    [SerializeField] public EnemyRadioGraphic 射线检测;
    [SerializeField] public EnemyRigidbar 僵直条;
    [SerializeField] public 小怪动画事件 动画事件中心;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public InitEnemySystem body;
    [SerializeField] private DamagedComponent 受伤模块;
  [SerializeField]  private AITypeState 当前状态;

    public float 跑步移动速度 = 4f;
    public bool 是否为初见玩家 =true;
    public bool 玩家是否死亡过一次=false;
   
    public bool 是否追击 = false;
    public float 脱战计时 = 0;
    public float 脱战所需要的时=10f;

    public float 突刺距离 = 7f;
    public float 突刺概率 = 0.3f;
    public float 突刺瞬时速度 = 12f;
    public float 突刺CD { get { return Random.Range(15f, 20f); } }
    public bool 突刺是否在CD = false;

    public float 近身斩击距离 = 2.5f;
    public float 靠近玩家最近距离 = 1.2f;

    public float 格挡成功概率 = 0.3f;
    public float 僵直时间 = 0.4f;
    [Tooltip("决策间隔")]
 [SerializeField]   private float decisionInterval = 0.3f; // 每0.3秒才判一次
    public float 决策间隔 { get {  return decisionInterval; } }
    [Tooltip("距离玩家的长度")]
    [SerializeField]   private float _distanceXPlayer = 0;
  public float 水平距离玩家距离 { get 
        {
            return _distanceXPlayer;
        } 
    }
    public int 朝向 =1;



    private void Awake()
    {
        fSM = new AIFSM();
        fSM.curState = AITypeState.ldle;
        当前状态=fSM.curState;
        fSM.AddState(AITypeState.ldle,new 小兵待机状态(this));
        fSM.AddState(AITypeState.run, new 小兵跑步状态(this));
        fSM.AddState(AITypeState.thrust, new 小兵突刺状态(this));
        fSM.AddState(AITypeState.preblock, new 小兵预备格挡状态(this));
        fSM.AddState(AITypeState.block,new 小兵格挡状态(this));
        fSM.AddState(AITypeState.hit, new 小兵硬直状态(this));
        fSM.AddState(AITypeState.relax, new 小兵松懈状态(this));
        fSM.AddState(AITypeState.alert, new 小兵警惕状态(this));
        fSM.AddState(AITypeState.attack, new 小兵斩击状态(this));
        
        fSM.ICurrentState = fSM._dicTypeState[fSM.curState];
    }
    void Start()
    {
        MainPlayer= GameObject.Find("MainPlayer");
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
        当前状态=state;
        fSM.SwitchStatus(state);
    }
    //为ai状态机提供的方法
    public void SetFilp(float dir)
    {
        body.SetFilp(dir);
    }

    public void CheckRb()
    {
        if(僵直条.检测是否僵直())
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

    public void EnemyReset()
    {
        SwitchState(AITypeState.ldle);
        body.ResetHP();
        僵直条.清空僵直条();
        是否追击 = false;
        脱战计时 = 0;
        是否为初见玩家 = true;
        突刺是否在CD = false;
    }
}
