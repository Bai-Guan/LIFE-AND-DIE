using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSAI控制器 : MonoBehaviour
{
    private BOSSAIFSM fSM;
    private IBossState IcurrentState;
    private BOSSAITypeState currentType;

    public GameObject MainPlayer;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public EnemyRadioGraphic 射线检测;
    [SerializeField] public EnemyRigidbar 僵直条;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public InitEnemySystem body;
    [SerializeField] private DamagedComponent 受伤模块;
    [SerializeField] private BOSSAITypeState 当前状态;
    [SerializeField] private EnemyAlertNotice notice;
    [SerializeField] private EnemyAttackHitBox hitComp;
    [SerializeField] public BOSS事件中心 AnimtorEvent;

    [SerializeField] public GameObject Boss扔出的石头;

    public bool 是否为初见玩家 = true;
    public bool 玩家是否死亡过一次 = false;
    private bool 警惕锁 = false;
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
    public int 朝向
    {
        get
        {
            if (body.isFacingLeft == true) return -1;
            else return 1;

        }
    }

    public BossHPUI BossUI;

    //是否为二阶段
    public bool isTwoPhase = false;

    [Header("移动时参数")]
    [SerializeField] public float 最大移动速度 = 13f;
    [SerializeField] public float 五连击距离 = 3.5f;
    [SerializeField] public float 挤压距离 = 7f;

    [Header("下坠参数")]
    [SerializeField] public float 下坠速度 = 15f;








    private void Awake()
    {
        fSM = new BOSSAIFSM();
        fSM.curState = BOSSAITypeState.waitPlayer;
        当前状态 = fSM.curState;
        fSM.AddState(BOSSAITypeState.waitPlayer, new BOSS等待玩家(this));
        fSM.AddState(BOSSAITypeState.ldle, new BOSS待机(this));
        fSM.AddState(BOSSAITypeState.run, new BOSS移动(this));
        fSM.AddState(BOSSAITypeState.squeeze, new BOSS挤压(this));
        fSM.AddState(BOSSAITypeState.fiveAttack, new BOSS五连击(this));
        fSM.AddState(BOSSAITypeState.jumpAttack, new BOSS大屁股砸地(this));
        fSM.AddState(BOSSAITypeState.hit, new BOSS弱点状态(this));
        fSM.AddState(BOSSAITypeState.phaseTwoStandby, new Boss二阶段待机(this));
        fSM.AddState(BOSSAITypeState.jumpMid, new BOSS跳回中间(this));
        fSM.AddState(BOSSAITypeState.throwStones, new BOSS扔石头(this));
        fSM.AddState(BOSSAITypeState.quickAttack, new Boss冲刺攻击(this));
        fSM.ICurrentState = fSM._dicTypeState[fSM.curState];

        body.BeAttack += 受伤时更新UI;
    }
    void Start()
    {
        if (MainPlayer == null)
            MainPlayer = GameObject.Find("MainPlayer");
        body.SetBackstab(false);
    }


    void Update()
    {
        fSM.Update();
     
        if (警惕锁 == false && 是否为初见玩家 == false)
        {
            警惕锁 = true;
            notice.OnSpotPlayer();
        }
    }
    private void FixedUpdate()
    {
        _distanceXPlayer = Mathf.Abs(transform.position.x - MainPlayer.transform.position.x);
        fSM.FixedUpdate();
        if(BossUI!=null) 
        BossUI.改变僵直条数值(僵直条.currentRigidValue, 僵直条.MaxRigid);
    }

    public void SwitchState(BOSSAITypeState state)
    {
        当前状态 = state;
        fSM.SwitchStatus(state);
    }
    //为ai状态机提供的方法
    public void 显示BossUI()
    {
        if (BossUI == null)
            BossUI = UIManager.Instance.OpenPanel(UIManager.UIConst.Boss_1) as BossHPUI;
    }

    private void 受伤时更新UI()
    {
        if (BossUI != null)
        {
            BossUI.改变血量(body.CurrentHP, body.MaxHp);
       
        }


    }
    public void 改变僵直条颜色(Color color)
    {
        if (BossUI != null)
            BossUI.改变僵直条颜色( color);
    }
    public void 只改变僵直条显示_不影响数值(float a)
    {
        BossUI.改变僵直条数值(a, 僵直条.MaxRigid);
    }
    public float ReturnDirToPlayer()
    {
        return transform.position.x - MainPlayer.transform.position.x >= 0 ? -1 : 1;
    }
    public void CheckRb()
    {
        if (僵直条.检测是否僵直())
        {
            SwitchState(BOSSAITypeState.hit);
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
    public void SetFacing(float dir) => body.SetFilp(dir);   // dir=-1 左，1 右

    public void 面朝玩家() 
        {
        SetFacing(ReturnDirToPlayer());
        }
    public void EnemyReset()
    {
        SwitchState(BOSSAITypeState.waitPlayer);
        是否为初见玩家 = true;
        body.ResetHP();
        警惕锁 = false;
        僵直条.清空僵直条();

    }
    public void OnAlerted()
    {

        是否为初见玩家 = false;
        float dirToPlayer = MainPlayer.transform.position.x - transform.position.x;
        SetFacing(Mathf.Sign(dirToPlayer));   // 正数朝右，负数朝左
    }

    public void CreateHitBox(string name)
    {
        hitComp.OnAnimEvent_FireAttack(name);
    }
    private void OnDestroy()
    {
        body.BeAttack -= 受伤时更新UI;
    }
}
