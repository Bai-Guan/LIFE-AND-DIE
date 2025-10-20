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
    
    public override void InitData(ComponentData data)
    {
        if(data is WeaponSpriteData spriteData)
        {
            weaponData = spriteData;
           
            Debug.Log("武器图片数据加载成功");
          
            
        }
    }

    void Start()
    {
        weapon = GetComponent<InitWeaponSystem>();
        BaseSpriteRenderer = weapon.BaseObject.GetComponent<SpriteRenderer>();
        WeaponSpriteRenderer=weapon.WeaponSpriteRenderer;
        AttackTimes = weapon.CurrentNum;
    }

    
    void Update()
    {
        
    }
}
