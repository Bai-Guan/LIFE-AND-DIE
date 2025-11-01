using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


//public interface IComponentData<T> where T:ComponentData
//{
//   void InitData(T data);
//}



public class InitWeaponSystem : MonoBehaviour
{
    public WeaponData WeaponDataOS {
        private set { }
        get { return _weaponDataOS; }
    }
    [SerializeField] private WeaponData _weaponDataOS;

    private Transform Base;

    private Transform OptionalSprite;
    public SpriteRenderer optionalSpriteRenderer { get { return OptionalSprite.GetComponent<SpriteRenderer>(); } private set { } }

    private Transform WeaponSprite;
    public SpriteRenderer WeaponSpriteRenderer { get { return WeaponSprite.GetComponent<SpriteRenderer>(); } private set { } }

    


    private Transform MainPlayer;
    public Rigidbody2D PlayerRB { get { return MainPlayer.GetComponent<Rigidbody2D>(); } private set { } }
    public SpriteRenderer PlayerSpriteRenderer { get { return MainPlayer.GetComponent<SpriteRenderer>(); } private set { } }

    private GameObject baseGameObject;
    public GameObject BaseObject {  get { return baseGameObject; } private set { } }

    private Animator anim;
    private AnimationEventHandler eventHandler;
    public AnimationEventHandler EventHandler {  get { return eventHandler; } }

    private List<WeaponComponent> HaveListWeaponComponents = new List<WeaponComponent>();

    //处理攻击重置的时间类
    [SerializeField] private float attackCounterResetCooldown=2f;
    EZTimer CounterResetTimer;
    private void ResetAttackCountZero() => _currentNum = 0;
  

    //映射表
    private Dictionary<Type, Type> MappingTable = new Dictionary<Type, Type>()
    {
        {typeof(WeaponSpriteData),typeof(WeaponSprite) },
        { typeof(WeaponHitBoxData),typeof(WeaponHitBox)},
        {typeof(AttackMoveData),typeof(AttackMoveCompent) },
        {typeof(WeaponDamageData),typeof(WeaponAttackDamage) },
        {typeof(WeaponEffectData),typeof(WeaponEffectComponent) },
        {typeof(WeaponAfterEffectData),typeof(AfterComponent) },
        {typeof(WeaponAudioData),typeof(WeaponAudioComponent)},
    };

    //用于通知其他组件正在退出攻击模式
    public event Action ChildrenExit;
    public event Action ChildrenEnter;

    private int AttackTimes;
    private int _currentNum = 0;
    //---------可被组件共享的数据
    public int CurrentNum //当前攻击段数
    {
        set
        {
            //_currentNum %= AttackTimes - 1;
            _currentNum = value;
            if (_currentNum >= AttackTimes) _currentNum = 0;
        }
        get => _currentNum;
    }
    public bool IsFacingLeft {  get { return MainPlayer.GetComponent<PlayerControl>().IsFacingLeft; } }

    private void Awake()
    {
        InitName();
        InitData();
    }

    private void InitName()
    {
        Base = transform.Find("Base");
        OptionalSprite = transform.Find("Base/OptionalSprite");
        WeaponSprite = transform.Find("WeaponSprite");
        MainPlayer = GameObject.Find("MainPlayer").transform;
        baseGameObject = Base.gameObject;
        anim = Base.GetComponent<Animator>();
        eventHandler = Base.GetComponent<AnimationEventHandler>();
    }
    private void InitData()
    {
        CounterResetTimer = new EZTimer(attackCounterResetCooldown);
    }
    public void UpdateWeaponData(WeaponData weaponData)
    {
        ClearTheComponent();
        if (weaponData == null) { Debug.Log("无效武器数据"); return; }
        _currentNum = 0;
        _weaponDataOS = weaponData;
        AttackTimes = _weaponDataOS.NumberOfAttacks;
        //1.由WeaponData数据先添加对应的Component 
        //2.再为相应的Compent赋予对应的Data
        foreach (var data in _weaponDataOS.componentDatas)
        {
            if (MappingTable.TryGetValue(data.GetType(),out Type component))
            {
              WeaponComponent com =  this.gameObject.AddComponent(component) as WeaponComponent;
                HaveListWeaponComponents.Add(com);
               Debug.Log("增添的脚本名字:"+component.Name);
                com.InitData(data);
               
            }
        }
    }
    private void ClearTheComponent()
    {
        if (HaveListWeaponComponents.Count > 0)
        {
            foreach(var data in HaveListWeaponComponents)
            {
                Destroy(data);
            }
            HaveListWeaponComponents.Clear();
        }
    }

    public void Enter()
    {
        //先检测有无武器
        if (_weaponDataOS == null) { Debug.LogWarning("当前没有武器装备！"); MainPlayer.GetComponent<PlayerControl>().SwitchStatus(PlayerControl.PlayerStatus.ldle); return; }


        print($"{this.name} Enter");
        //关闭player的角色贴图
        PlayerSpriteRenderer.enabled = false;
        // 确保Base对象激活
        if (!baseGameObject.activeSelf)
            baseGameObject.SetActive(true);
        //装备武器的动画器
        anim.runtimeAnimatorController = _weaponDataOS.BaseAnimator;
      
        //调整动画图片朝向
        CheckFildX();
        //停止计时器
        CounterResetTimer.StopTime();

        //通知子组件开始攻击
        ChildrenEnter?.Invoke();

        // 重置动画状态确保从开始播放
        anim.Rebind();
        anim.Update(0f);

        // 播放攻击动画
        anim.SetInteger("count",_currentNum);
        anim.SetBool("active", true);
    }

    public void Update()
    {
        CounterResetTimer.Tick();
    }


    public void Exit()
    {
        ChildrenExit?.Invoke();
        CounterResetTimer.StartTimer();
        CurrentNum = _currentNum + 1; Debug.Log("当前攻击段数(起始值为0)："+CurrentNum);
        anim.SetBool("active", false);
        PlayerSpriteRenderer.enabled = true;
    }

    public void OnEnable()
    {
        eventHandler.OnFinish += Exit;
        CounterResetTimer.OnTimerDone += ResetAttackCountZero;
    }

    public void OnDisable()
    {
        eventHandler.OnFinish -= Exit;
        CounterResetTimer.OnTimerDone -= ResetAttackCountZero;
    }

    //更改base图片 和 武器图片 动画朝向
    private void CheckFildX()
    {
        
        Base.GetComponent<SpriteRenderer>().flipX = IsFacingLeft;
        WeaponSpriteRenderer.flipX = IsFacingLeft;
    }

    //3.实现组件和玩家之间的逻辑
}
