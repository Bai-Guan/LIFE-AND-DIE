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

        if (hitBoxCallBack == null) { Debug.LogWarning("当前伤害组件未匹配到伤害碰撞箱组件");this.enabled = false;return; }

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
       // Debug.Log($"DamageAttack 被调用，参数长度: {obj?.Length ?? -1}");

        if (obj == null || obj.Length == 0)
        {
           // Debug.Log("攻击挥空 - 数组为null或空");
            return;
        }

        Debug.Log($"开始处理 {obj.Length} 个敌人");

        foreach (GameObject go in obj)
        {
           // Debug.Log($"处理敌人: {go.name}");
            IBeDamaged rec = null;
            Transform cur = go.transform;

            while (cur != null)
            {
                rec = cur.GetComponent<IBeDamaged>();
                if (rec != null)
                {
                  //  Debug.Log($"找到 IBeDamaged 接口在: {cur.name}");
                    break;
                }
                cur = cur.parent;
            }

            if (rec == null)
            {
              //  Debug.LogWarning($"在 {go.name} 及其父对象中未找到 IBeDamaged 接口");
                continue;
            }

            rec.OnHurt(Data.damageDatas[weapon.CurrentNum], this.gameObject);
            Debug.Log($"对 {go.name} 造成伤害");
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
