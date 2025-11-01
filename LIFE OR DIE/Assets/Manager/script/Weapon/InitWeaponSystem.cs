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

    //���������õ�ʱ����
    [SerializeField] private float attackCounterResetCooldown=2f;
    EZTimer CounterResetTimer;
    private void ResetAttackCountZero() => _currentNum = 0;
  

    //ӳ���
    private Dictionary<Type, Type> MappingTable = new Dictionary<Type, Type>()
    {
        {typeof(WeaponSpriteData),typeof(WeaponSprite) },
        { typeof(WeaponHitBoxData),typeof(WeaponHitBox)},
        {typeof(AttackMoveData),typeof(AttackMoveCompent) },
        {typeof(WeaponDamageData),typeof(WeaponAttackDamage) },
        {typeof(WeaponEffectData),typeof(WeaponEffectComponent) },
        {typeof(WeaponAfterEffectData),typeof(AfterComponent) }
    };

    //����֪ͨ������������˳�����ģʽ
    public event Action ChildrenExit;
    public event Action ChildrenEnter;

    private int AttackTimes;
    private int _currentNum = 0;
    //---------�ɱ�������������
    public int CurrentNum //��ǰ��������
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
        if (weaponData == null) { Debug.Log("��Ч��������"); return; }
        _currentNum = 0;
        _weaponDataOS = weaponData;
        AttackTimes = _weaponDataOS.NumberOfAttacks;
        //1.��WeaponData��������Ӷ�Ӧ��Component 
        //2.��Ϊ��Ӧ��Compent�����Ӧ��Data
        foreach (var data in _weaponDataOS.componentDatas)
        {
            if (MappingTable.TryGetValue(data.GetType(),out Type component))
            {
              WeaponComponent com =  this.gameObject.AddComponent(component) as WeaponComponent;
                HaveListWeaponComponents.Add(com);
               Debug.Log("����Ľű�����:"+component.Name);
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
        //�ȼ����������
        if (_weaponDataOS == null) { Debug.LogWarning("��ǰû������װ����"); MainPlayer.GetComponent<PlayerControl>().SwitchStatus(PlayerControl.PlayerStatus.ldle); return; }


        print($"{this.name} Enter");
        //�ر�player�Ľ�ɫ��ͼ
        PlayerSpriteRenderer.enabled = false;
        // ȷ��Base���󼤻�
        if (!baseGameObject.activeSelf)
            baseGameObject.SetActive(true);
        //װ�������Ķ�����
        anim.runtimeAnimatorController = _weaponDataOS.BaseAnimator;
      
        //��������ͼƬ����
        CheckFildX();
        //ֹͣ��ʱ��
        CounterResetTimer.StopTime();

        //֪ͨ�������ʼ����
        ChildrenEnter?.Invoke();

        // ���ö���״̬ȷ���ӿ�ʼ����
        anim.Rebind();
        anim.Update(0f);

        // ���Ź�������
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
        CurrentNum = _currentNum + 1; Debug.Log("��ǰ��������(��ʼֵΪ0)��"+CurrentNum);
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

    //����baseͼƬ �� ����ͼƬ ��������
    private void CheckFildX()
    {
        
        Base.GetComponent<SpriteRenderer>().flipX = IsFacingLeft;
        WeaponSpriteRenderer.flipX = IsFacingLeft;
    }

    //3.ʵ����������֮����߼�
}
