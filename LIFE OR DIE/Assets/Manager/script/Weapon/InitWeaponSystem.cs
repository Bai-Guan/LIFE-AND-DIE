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
    public SpriteRenderer PlayerSpriteRenderer { get { return MainPlayer.GetComponent<SpriteRenderer>(); } private set { } }

    private GameObject baseGameObject;
    public GameObject BaseObject {  get { return baseGameObject; } private set { } }

    private Animator anim;
    private AnimationEventHandler eventHandler;
    public AnimationEventHandler EventHandler {  get { return eventHandler; } }

    //映射表
    private Dictionary<Type, Type> MappingTable = new Dictionary<Type, Type>()
    {
        {typeof(WeaponSpriteData),typeof(WeaponSprite) },
        { typeof(WeaponHitBoxData),typeof(WeaponHitBox)},
    };

    //用于通知其他组件正在退出攻击模式
    public event Action ChildrenExit;

    private void Awake()
    {
        InitName();
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

    public void UpdateWeaponData(WeaponData weaponData)
    {
        if (weaponData == null) { Debug.Log("无效武器数据"); return; }
        _weaponDataOS = weaponData;
        //1.由WeaponData数据先添加对应的Component 
        //2.再为相应的Compent赋予对应的Data
        foreach (var data in _weaponDataOS.componentDatas)
        {
            if (MappingTable.TryGetValue(data.GetType(),out Type component))
            {
              WeaponComponent com =  this.gameObject.AddComponent(component) as WeaponComponent;
               Debug.Log("增添的脚本名字:"+component.Name);
                com.InitData(data);
               
            }
        }
    }

    public void Enter()
    {
        print($"{this.name} Enter");
        //关闭player的角色贴图
        PlayerSpriteRenderer.enabled = false;
        // 确保Base对象激活
        if (!baseGameObject.activeSelf)
            baseGameObject.SetActive(true);
        //调整动画图片朝向
        CheckFildX();

        // 重置动画状态确保从开始播放
        anim.Rebind();
        anim.Update(0f);

        // 播放攻击动画
        anim.SetBool("active", true);
    }

    public void Exit()
    {
        ChildrenExit?.Invoke();
        anim.SetBool("active", false);
        PlayerSpriteRenderer.enabled = true;
    }

    public void OnEnable()
    {
        eventHandler.OnFinish += Exit;
    }

    public void OnDisable()
    {
        eventHandler.OnFinish -= Exit;
    }

    //更改base图片 和 武器图片 动画朝向
    private void CheckFildX()
    {
        bool isLeft= MainPlayer.GetComponent<PlayerControl>().IsFacingLeft;
        Base.GetComponent<SpriteRenderer>().flipX = isLeft;
    }

    //3.实现组件和玩家之间的逻辑
}
