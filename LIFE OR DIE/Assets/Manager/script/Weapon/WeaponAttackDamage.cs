using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackDamage : WeaponComponent
{
    InitWeaponSystem weapon;
    WeaponDamageData Data;
    private WeaponHitBox hitBoxCallBack;

 

    public override void InitData(ComponentData data)
    {
        if (data is WeaponDamageData HD)
        {
            Data = HD;
        }
    }

    private void OnEnter()
    {
       
    }


    void Start()
    {
        weapon = GetComponent<InitWeaponSystem>();
        hitBoxCallBack = GetComponent<WeaponHitBox>();

        if (hitBoxCallBack == null) { Debug.LogWarning("µ±Ç°ÉËº¦×é¼þÎ´Æ¥Åäµ½ÉËº¦Åö×²Ïä×é¼þ");this.enabled = false;return; }

        weapon.ChildrenEnter += OnEnter;
      
        hitBoxCallBack.AttackColliderEvent += DamageAttack;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnExit()
    {

    }
    private void DamageAttack(GameObject[] obj)
    {
        if(obj.Length == 0) { Debug.Log("¹¥»÷»Ó¿Õ");return; }
        foreach (GameObject go in obj)
        {
            IBeDamaged rec = null;
            Transform cur = go.transform;
            while (cur != null)
            {
                rec=cur.GetComponent<IBeDamaged>();
                if (rec != null) break;
               cur=cur.parent;
            }
            if (rec == null) continue;

            rec.OnHurt(Data.damageDatas[weapon.CurrentNum], this.gameObject);

        }
        


            //InitEnemySystem temp = go.GetComponent<InitEnemySystem>();
            //if (temp == null) { continue; }
            //temp.BeAttacked(Data.damageDatas[weapon.CurrentNum],this.gameObject);
        
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        hitBoxCallBack.AttackColliderEvent -= DamageAttack;
        weapon.ChildrenEnter -= OnEnter;
    }

  
}
