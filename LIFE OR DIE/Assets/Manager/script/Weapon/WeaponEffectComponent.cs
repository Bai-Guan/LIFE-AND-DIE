using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffectComponent : WeaponComponent
{
    WeaponEffectData effectData;
    InitWeaponSystem weapon;
    WeaponHitBox hitBox;
    bool isfacingLeft;
    int Filp;
    public override void InitData(ComponentData data)
    {
       if(data is WeaponEffectData Data)
        {
            effectData = Data;
        }
        Debug.Log("特效武器组件初始化完毕");
    }

    private void Start()
    {
        weapon = transform.GetComponent<InitWeaponSystem>();
        weapon.ChildrenEnter += OnEnter;
        hitBox = transform.GetComponent<WeaponHitBox>();
        hitBox.AttackColliderEvent += EffectAction;
    }

    private void OnEnter()
    {
     
        if (hitBox==null)
        {
            hitBox = transform.GetComponent<WeaponHitBox>();
            hitBox.AttackColliderEvent += EffectAction;
            if (hitBox == null)
            {
                Debug.LogWarning("武器特效组件未找到hitbox组件");
                this.enabled = false;
            }
        }

        isfacingLeft = weapon.IsFacingLeft;
        Filp = isfacingLeft ? -1 : 1;
    }


    void EffectAction(GameObject[] objs)
    {
        Debug.Log("特效事件");
        foreach (GameObject obj in objs)
        {
            if (obj.CompareTag("Phyitem") == true) continue;


            //TODO 其他特效的适配
            WEffectData temp = effectData.ListEffectDatas[weapon.CurrentNum];
            if (temp.isRandom)
            {
                EffectManager.Instance.SpeicalEffectKnife(obj.transform,temp.durTime,temp.size);
            }
            else
            {
                EffectManager.Instance.SpeicalEffectKnife(obj.transform,temp.durTime,temp.size,Filp*temp.range);
            }
        }
    }

    private void OnDisable()
    {
        weapon.ChildrenEnter -= OnEnter;
     
        hitBox.AttackColliderEvent -= EffectAction;
    }
}
