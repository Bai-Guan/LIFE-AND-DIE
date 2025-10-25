using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//, IComponentData<WeaponHitBoxData>
public class WeaponHitBox : WeaponComponent
{
    InitWeaponSystem weapon;
    WeaponHitBoxData Data;
    private bool isfacingLeft;
    private int Filp;

    private Vector2 offset;

    public event Action<GameObject[]> AttackColliderEvent;


    public override void InitData(ComponentData data)
    {
        if(data is WeaponHitBoxData HD)
        {
            Data = HD;
        }
    }

    private void OnEnter()
    {
        isfacingLeft = weapon.IsFacingLeft;
        Filp = isfacingLeft ? -1 : 1;
    }


    void Start()
    {
        weapon = GetComponent<InitWeaponSystem>();
        weapon.ChildrenEnter += OnEnter;
        isfacingLeft = weapon.IsFacingLeft;
        weapon.EventHandler.OneAttack += OnAttack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnExit()
    {

    }
    private void OnAttack()
    {
        offset.Set(
      transform.position.x + Filp * Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.center.x,
      transform.position.y + Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.center.y
          );

        Debug.DrawLine(offset - new Vector2(Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size.x * 0.5f,
            Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size.y * 0.5f),
                offset + new Vector2(Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size.x * 0.5f,
                Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size.y * 0.5f),
                Color.red, 2f);

        Collider2D[] hit = Physics2D.OverlapBoxAll(offset, Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size,
            0f, Data.DetectableLayer);

        Debug.Log($"��⵽����ײ������: {hit?.Length ?? 0}"); // �������

        if (hit != null && hit.Length > 0)
        {
            HashSet<GameObject> Enemylist = new HashSet<GameObject>();
            int layerMaskCount = 0;

            foreach (var col in hit)
            {
                //Debug.Log($"��ײ��: {col.gameObject.name}, �㼶: {col.gameObject.layer}");

                int mask = (1 << 9) | (1 << 10);
                if ((mask & (1 << col.gameObject.layer)) != 0)
                {
                    layerMaskCount++;
                    GameObject target = col.attachedRigidbody ? col.attachedRigidbody.gameObject : col.gameObject;
                    Enemylist.Add(target);
                  //  Debug.Log($"��ӵ������б�: {target.name}");
                }
            }

         //   Debug.Log($"ͨ���㼶���˵ĵ�������: {layerMaskCount}");
          //  Debug.Log($"ȥ�غ�ĵ�������: {Enemylist.Count}");

            GameObject[] enemiesHit = new GameObject[Enemylist.Count];
            Enemylist.CopyTo(enemiesHit);

            // ����¼��Ƿ��ж�����
            if (AttackColliderEvent != null)
            {
               // Debug.Log($"�����¼������ݵ�������: {enemiesHit.Length}");
                AttackColliderEvent(enemiesHit);
            }
            else
            {
               // Debug.LogError("AttackColliderEvent û�ж����ߣ�");
            }
        }
       
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        weapon.EventHandler.OneAttack -= OnAttack;
        weapon.ChildrenEnter -= OnEnter;
    }

    private void OnDrawGizmos()
    {
        if (Data == null) return;
        foreach(HitBoxSizeAndOffset data in Data.HitBoxSizeAndOffsets)
        {
            if(data.isDebug==false) continue;
            Gizmos.color=Color.white;
            Gizmos.DrawWireCube(transform.position + (Vector3)data.HitBox.center, data.HitBox.size);
        }
    }
}
