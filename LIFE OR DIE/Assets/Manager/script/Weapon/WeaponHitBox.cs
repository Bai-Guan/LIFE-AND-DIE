using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//, IComponentData<WeaponHitBoxData>
public class WeaponHitBox : WeaponComponent
{

    WeaponHitBoxData _weaponHitBoxData;
  

    public override void InitData(ComponentData data)
    {
        if(data is WeaponHitBoxData HD)
        {
            _weaponHitBoxData = HD;
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
