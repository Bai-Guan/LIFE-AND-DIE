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

        if (hitBoxCallBack == null) { Debug.LogWarning("��ǰ�˺����δƥ�䵽�˺���ײ�����");this.enabled = false;return; }

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
       // Debug.Log($"DamageAttack �����ã���������: {obj?.Length ?? -1}");

        if (obj == null || obj.Length == 0)
        {
           // Debug.Log("�����ӿ� - ����Ϊnull���");
            return;
        }

        Debug.Log($"��ʼ���� {obj.Length} ������");

        foreach (GameObject go in obj)
        {
           // Debug.Log($"�������: {go.name}");
            IBeDamaged rec = null;
            Transform cur = go.transform;

            while (cur != null)
            {
                rec = cur.GetComponent<IBeDamaged>();
                if (rec != null)
                {
                  //  Debug.Log($"�ҵ� IBeDamaged �ӿ���: {cur.name}");
                    break;
                }
                cur = cur.parent;
            }

            if (rec == null)
            {
              //  Debug.LogWarning($"�� {go.name} ���丸������δ�ҵ� IBeDamaged �ӿ�");
                continue;
            }

            rec.OnHurt(Data.damageDatas[weapon.CurrentNum], this.gameObject);
            Debug.Log($"�� {go.name} ����˺�");
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
