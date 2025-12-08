using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
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

        //Debug.DrawLine(offset - new Vector2(Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size.x * 0.5f,
        //    Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size.y * 0.5f),
        //        offset + new Vector2(Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size.x * 0.5f,
        //        Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size.y * 0.5f),
        //        Color.red, 2f);

        Collider2D[] hit = Physics2D.OverlapBoxAll(offset, Data.HitBoxSizeAndOffsets[weapon.CurrentNum].HitBox.size,
            0f, Data.DetectableLayer);

        Debug.Log($"检测到的碰撞体数量: {hit?.Length ?? 0}"); // 添加这行

        if (hit != null && hit.Length > 0)
        {
            HashSet<GameObject> Enemylist = new HashSet<GameObject>();
            int layerMaskCount = 0;

            foreach (var col in hit)
            {
                //Debug.Log($"碰撞体: {col.gameObject.Name}, 层级: {col.gameObject.layer}");

                int mask = (1 << 9) | (1 << 10);
                if ((mask & (1 << col.gameObject.layer)) != 0)
                {
                    layerMaskCount++;
                    GameObject target = col.attachedRigidbody ? col.attachedRigidbody.gameObject : col.gameObject;
                    Enemylist.Add(target);
                  //  Debug.Log($"添加到敌人列表: {target.Name}");
                }

                if(col.CompareTag("Phyitem"))
                {
                   
                        Enemylist.Add(col.gameObject);
                    
                }
             
            }

         //   Debug.Log($"通过层级过滤的敌人数量: {layerMaskCount}");
          //  Debug.Log($"去重后的敌人数量: {Enemylist.Count}");

            GameObject[] enemiesHit = new GameObject[Enemylist.Count];
            Enemylist.CopyTo(enemiesHit);

            // 检查事件是否有订阅者
            if (AttackColliderEvent != null)
            {
               // Debug.Log($"触发事件，传递敌人数量: {enemiesHit.Length}");
                AttackColliderEvent(enemiesHit);
            }
            else
            {
               // Debug.LogError("AttackColliderEvent 没有订阅者！");
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
