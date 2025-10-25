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
     Collider2D[] hit  = Physics2D.OverlapBoxAll(offset, Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size,
            0f, Data.DetectableLayer);

        if (hit != null) { 
            HashSet<GameObject> Enemylist = new HashSet<GameObject>();
            foreach (var col in hit)
            {
                Debug.LogWarning(col.gameObject.name+ col.gameObject.ToString());
                // ”√≤„≈–∂œ
                int mask = (1 << 9) | (1 << 10);
                if ((mask & (1 << col.gameObject.layer)) != 0)
                    Enemylist.Add(col.attachedRigidbody ? col.attachedRigidbody.gameObject
                                                   : col.gameObject);
            }
            //int mask = (1 << 9) | (1 << 10);   // 9 ∫Õ 10 ¡Ω≤„
            //if (((1 << obj.gameObject.layer) & mask) != 0)
            //{
            //    Enemylist.Add(obj.gameObject);
            //}
        
            GameObject[] enemiesHit = new GameObject[Enemylist.Count];
            Enemylist.CopyTo(enemiesHit);

            AttackColliderEvent?.Invoke(enemiesHit);
        }
        else 
        { Debug.Log("OverlapBoxAllŒ¥ºÏ≤‚µΩ"); }
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
