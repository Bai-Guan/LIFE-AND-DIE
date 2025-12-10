using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterComponent : WeaponComponent
{
    InitWeaponSystem weapon;
    WeaponHitBox hitBox;
    WeaponAfterEffectData AfterEffect;

    public override void InitData(ComponentData data)
    {
        if (data is WeaponAfterEffectData after )
        {
            AfterEffect = after;
            Debug.LogWarning("后处理加载成功，此组件高度依赖后处理管理器、hitbox、摄像机管理类");
        }
    
    }

    private void Start()
    {
        weapon = transform.GetComponent<InitWeaponSystem>();
        weapon.ChildrenEnter += OnEnter;
        hitBox = transform.GetComponent<WeaponHitBox>();
        hitBox.AttackColliderEvent += OnAfterEffect;
    }

    private void OnEnter()
    {
        if (hitBox == null)
        {
            hitBox = transform.GetComponent<WeaponHitBox>();
            hitBox.AttackColliderEvent += OnAfterEffect;
            if (hitBox == null)
            {
                Debug.LogWarning("后处理组件未找到hitbox组件");
                this.enabled = false;
            }
        }

    }

    private void OnAfterEffect(GameObject[] objs)
    {
        if (objs.Length > 0 && objs[0].CompareTag("Phyitem")) return;
        

        int times = weapon.CurrentNum;
        AfterEffectType temp = AfterEffect._AfterEffectsdata[times];
        if (temp.是否震动) CameraManager.Instance.CameraShake(temp.shakeTime, temp.shakePow);
        if(temp.是否径向模糊)EffectManager.Instance.VerticalBlur(temp.BlurTime, temp.BlurPow);
        if (temp.是否色差) EffectManager.Instance.ChromaticAberrationSet(temp.ColorTime, 0.9f);
        
        
    }


    private void OnDisable()
    {
        weapon.ChildrenEnter -= OnEnter;
        hitBox.AttackColliderEvent -= OnAfterEffect;

    }
}
