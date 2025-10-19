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

    //ӳ���
    private Dictionary<Type, Type> MappingTable = new Dictionary<Type, Type>()
    {
        {typeof(WeaponSpriteData),typeof(WeaponSprite) },
        { typeof(WeaponHitBoxData),typeof(WeaponHitBox)},
    };

    //����֪ͨ������������˳�����ģʽ
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
        //�ر�player�Ľ�ɫ��ͼ
        PlayerSpriteRenderer.enabled = false;
        // ȷ��Base���󼤻�
        if (!baseGameObject.activeSelf)
            baseGameObject.SetActive(true);
        //��������ͼƬ����
        CheckFildX();

        // ���ö���״̬ȷ���ӿ�ʼ����
        anim.Rebind();
        anim.Update(0f);

        // ���Ź�������
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

    //����baseͼƬ �� ����ͼƬ ��������
    private void CheckFildX()
    {
        bool isLeft= MainPlayer.GetComponent<PlayerControl>().IsFacingLeft;
        Base.GetComponent<SpriteRenderer>().flipX = isLeft;
    }

    //3.ʵ����������֮����߼�
}
