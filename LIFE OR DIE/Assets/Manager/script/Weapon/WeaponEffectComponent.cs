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
        Debug.Log("��Ч���������ʼ�����");
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
                Debug.LogWarning("������Ч���δ�ҵ�hitbox���");
                this.enabled = false;
            }
        }

        isfacingLeft = weapon.IsFacingLeft;
        Filp = isfacingLeft ? -1 : 1;
    }


    void EffectAction(GameObject[] objs)
    {
        Debug.Log("��Ч�¼�");
        foreach (GameObject obj in objs)
        {

            //TODO ������Ч������
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
