using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 骷髅杂兵状态机 : MonoBehaviour
{
    private AIFSM fSM;
    private IEnemyState IcurrentState;
    private AITypeState currentType;

    public GameObject MainPlayer;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public EnemyRadioGraphic 射线检测;
    [SerializeField] public EnemyRigidbar 僵直条;
    [SerializeField] public 骷髅杂兵动画事件 动画事件中心;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public InitEnemySystem body;
    [SerializeField] private DamagedComponent 受伤模块;
    [SerializeField] private AITypeState 当前状态;

    public float 跑步移动速度 = 4f;
    public bool 是否为初见玩家 = false;
    public bool 玩家是否死亡过一次 = false;

    public bool 是否追击 = false;
    public float 脱战计时 = 0;
    public float 脱战所需要的时 = 10f;

    public float 近身斩击距离 = 2.5f;
    public float 靠近玩家最近距离 = 1.2f;

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
        fSM.AddState(AITypeState.ldle, new 骷髅杂兵待机(this));
        fSM.AddState(AITypeState.run, new 骷髅杂兵走路(this));
        fSM.AddState(AITypeState.hit, new 骷髅杂兵受伤(this));
        fSM.AddState(AITypeState.attack, new 骷髅杂兵攻击(this));

        fSM.ICurrentState = fSM._dicTypeState[fSM.curState];
    }
    void Start()
    {
        if(MainPlayer==null)
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

}
