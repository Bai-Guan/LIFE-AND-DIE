using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class StairState : IPlayerState
{
    readonly NewPlayerControll _ctx;
    readonly Rigidbody2D rb;
    float climbDir;      // 1 上  -1 下

    float tempG = 1f;
    public StairState(NewPlayerControll player) { _ctx = player; rb = player.rb; }

    public void Enter()
    {
       tempG=rb.gravityScale;
        rb.gravityScale = 0;          // 爬梯时忽略重力
        rb.velocity = Vector2.zero;
    }

    public void Update()
    {
        climbDir = _ctx.InputY;
        // 每帧根据方向给速度
        rb.velocity = new Vector2(0, climbDir * 8f);
       
    }

    public void Exit()
    {
         rb.gravityScale=tempG;
 
    }

   

    public void FixedUpdate()
    {
        MoveX();
    }

    public void Attack()
    {
   
    }

    public void Dodge()
    {
   
    }

    public void Block()
    {

    }

    public void ContractPower()
    {
    
    }
    private void MoveX()
    {
       
        // 正常移动
        float targetVelocityX = _ctx.InputX * _ctx.DataMan.moveSpeed;
    
        _ctx.rb.velocity = new Vector2(targetVelocityX, _ctx.rb.velocity.y);
    }
}
