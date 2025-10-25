using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//, IComponentData<WeaponSpriteData>
public class WeaponSprite : WeaponComponent
{
    private InitWeaponSystem weapon;

   [SerializeField] WeaponSpriteData weaponData;
   

    private SpriteRenderer BaseSpriteRenderer;
    private SpriteRenderer WeaponSpriteRenderer;

    private int AttackTimes ;
    private Animation _animation;

    private int count;
  [SerializeField]  private int pointer=0;
    public override void InitData(ComponentData data)
    {
        if(data is WeaponSpriteData spriteData)
        {
            weaponData = spriteData;
            Debug.Log("武器图片数据加载成功");

           // weaponData.AttackData[0].Sprites
        }
    }

    void Start()
    {
        weapon = GetComponent<InitWeaponSystem>();
        BaseSpriteRenderer = weapon.BaseObject.GetComponent<SpriteRenderer>();
        WeaponSpriteRenderer=weapon.WeaponSpriteRenderer;
        AttackTimes = weapon.CurrentNum;
        //注册攻击结束事件
        weapon.EventHandler.OnFinish += OnExit;
        weapon.ChildrenEnter += OnEnter;

        BaseSpriteRenderer.RegisterSpriteChangeCallback(OnSpriteChange);
    }

    
    void Update()
    {
        
    }

    private void OnEnter()
    {
        
        pointer = 0;
        WeaponSpriteRenderer.enabled = true;
    }

    void OnSpriteChange(SpriteRenderer renderer)
    {
       count=weapon.CurrentNum;
        int len = weaponData.AttackData[count].Sprites.Length;
        WeaponSpriteRenderer.sprite = weaponData.AttackData[count].Sprites[pointer];
        pointer = (pointer + 1) % len;
    }
    //一次攻击结束 指针归0
    private void OnExit()
    {
        WeaponSpriteRenderer.enabled = false;
       
        
    }

    private void OnDisable()
    {
        weapon.EventHandler.OnFinish -= OnExit;
        weapon.ChildrenEnter -= OnEnter;
    }
    // BaseSpriteRenderer.RegisterSpriteChangeCallback()
    //根据当前攻击段数 播放那个段数动画
}
