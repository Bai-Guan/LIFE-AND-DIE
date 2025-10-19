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
    private Transform WeaponSprite;

    private GameObject baseGameObject;
    private Animator anim;
    
    private AnimationEventHandler eventHandler;
    public AnimationEventHandler EventHandler {  get { return eventHandler; } }

    //映射表
    private Dictionary<Type, Type> MappingTable = new Dictionary<Type, Type>()
    {
        {typeof(WeaponSpriteData),typeof(WeaponSprite) },
        { typeof(WeaponHitBoxData),typeof(WeaponHitBox)},
    };
        


    private void Awake()
    {
        InitName();
    }

    private void InitName()
    {
       Base = transform.Find("Base");
       OptionalSprite = transform.Find("Base/OptionalSprite");
        WeaponSprite = transform.Find("WeaponSprite");

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
    }

    public void Exit()
    {
        
    }

    public void OnEnable()
    {
        eventHandler.OnFinish += Exit;
    }

    public void OnDisable()
    {
        eventHandler.OnFinish -= Exit;
    }

    //3.实现组件和玩家之间的逻辑
}
