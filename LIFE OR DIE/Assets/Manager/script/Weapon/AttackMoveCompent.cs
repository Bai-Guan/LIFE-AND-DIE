using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AttackMoveCompent : WeaponComponent
{
    private InitWeaponSystem weapon;
    private AttackMoveData attackMoveData;
    private Rigidbody2D rb;
    private bool isfacingLeft;
    private int Filp;

    private float NeedDistanceX;
    private float NeedDistanceY;
    private float Speed;
    private bool dashing = false;
    private float timer=0;
   public  override void InitData(ComponentData data)
    {
        if (data is AttackMoveData moveData)
        {
            attackMoveData = moveData;
        }
     
    }
    private void Start()
    {
        weapon = this.transform.GetComponent<InitWeaponSystem>();
        rb = weapon.PlayerRB;
        isfacingLeft=weapon.IsFacingLeft;
        weapon.EventHandler.OneAttack += OnAttackMove;
        weapon.ChildrenEnter += OnEnter;
    }
    private void OnEnter()
    {
        isfacingLeft = weapon.IsFacingLeft;
        Filp = isfacingLeft ? -1 : 1;

        var cur = attackMoveData.AttackMoveDatas[weapon.CurrentNum];
        NeedDistanceX = cur.MoveX * Filp;   // 水平方向已含朝向
        NeedDistanceY = cur.MoveY;          // Y 方向保持原值
        Speed = cur.SpeedMove;

        timer = 0f;
    }

    private void FixedUpdate()
    {
        if (!dashing) return;

        float totalLen = Mathf.Sqrt(NeedDistanceX * NeedDistanceX + NeedDistanceY * NeedDistanceY);
        if (totalLen == 0f) { StopDash(); return; }

        float timeNeed = totalLen / Speed;
        timer += Time.fixedDeltaTime;

        if (timer >= timeNeed)
        {
            StopDash();
        }
        else
        {
            Vector2 dir = new Vector2(NeedDistanceX, NeedDistanceY).normalized;
            rb.velocity = dir * Speed;
        }
    }
    private void StopDash()
    {
        rb.velocity = Vector2.zero;
        dashing = false;
        NeedDistanceX = 0f;
        NeedDistanceY = 0f;
    }
    private void OnAttackMove()
    {
        dashing = true;
    }
    private void OnExit()
    {
        
    }

 
    private void OnDisable()
    {
        weapon.EventHandler.OneAttack -= OnAttackMove;
        weapon.ChildrenEnter -= OnEnter;
    }
}
