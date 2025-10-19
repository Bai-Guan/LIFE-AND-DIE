using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//, IComponentData<WeaponSpriteData>
public class WeaponSprite : WeaponComponent
{
    private InitWeaponSystem weapon;

   [SerializeField] WeaponSpriteData weaponData;
    private int _currentNum=0;

    private SpriteRenderer BaseSpriteRenderer;
    private SpriteRenderer WeaponSpriteRenderer;

    private int CurrentNum {
        set 
        {
            //_currentNum %= AttackTimes - 1;
            _currentNum = value;
            if (_currentNum >= AttackTimes) _currentNum = 0;
        }
        get=>_currentNum;
    }
    private int AttackTimes;
    private Animation _animation;
    
    public override void InitData(ComponentData data)
    {
        if(data is WeaponSpriteData spriteData)
        {
            weaponData = spriteData;
            Debug.Log("武器图片数据加载成功");
            AttackTimes = weaponData.AttackTimes;
            
        }
    }

    void Start()
    {
        weapon = GetComponent<InitWeaponSystem>();
        BaseSpriteRenderer = weapon.BaseObject.GetComponent<SpriteRenderer>();
        WeaponSpriteRenderer=weapon.WeaponSpriteRenderer;
    }

    
    void Update()
    {
        
    }
}
