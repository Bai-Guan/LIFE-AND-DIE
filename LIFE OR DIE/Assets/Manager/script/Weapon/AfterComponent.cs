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
            Debug.LogWarning("������سɹ���������߶����������������hitbox�������������");
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
                Debug.LogWarning("�������δ�ҵ�hitbox���");
                this.enabled = false;
            }
        }

    }

    private void OnAfterEffect(GameObject[] objs)
    {
        int times = weapon.CurrentNum;
        AfterEffectType temp = AfterEffect._AfterEffectsdata[times];
        if (temp.�Ƿ���) CameraManager.Instance.CameraShake(temp.shakeTime, temp.shakePow);
        if(temp.�Ƿ���ģ��)EffectManager.Instance.VerticalBlur(temp.BlurTime, temp.BlurPow);
        if (temp.�Ƿ�ɫ��) EffectManager.Instance.ChromaticAberrationSet(temp.ColorTime, 0.9f);
        
        
    }


    private void OnDisable()
    {
        weapon.ChildrenEnter -= OnEnter;
        hitBox.AttackColliderEvent -= OnAfterEffect;

    }
}
