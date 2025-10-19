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

    //ӳ���
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
        if (weaponData == null) { Debug.Log("��Ч��������"); return; }
        _weaponDataOS = weaponData;
        //1.��WeaponData��������Ӷ�Ӧ��Component 
        //2.��Ϊ��Ӧ��Compent�����Ӧ��Data
        foreach (var data in _weaponDataOS.componentDatas)
        {
            if (MappingTable.TryGetValue(data.GetType(),out Type component))
            {
              WeaponComponent com =  this.gameObject.AddComponent(component) as WeaponComponent;
               Debug.Log("����Ľű�����:"+component.Name);
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

    //3.ʵ����������֮����߼�
}
