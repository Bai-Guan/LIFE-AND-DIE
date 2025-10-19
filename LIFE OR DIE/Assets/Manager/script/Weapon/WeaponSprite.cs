using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//, IComponentData<WeaponSpriteData>
public class WeaponSprite : WeaponComponent
{
   [SerializeField] WeaponSpriteData weaponData;


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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
