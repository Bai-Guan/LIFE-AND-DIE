using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionState :IPlayerState
{
    NewPlayerControll _ctx;
    bool isfalling=false;
    float fallSpeed = 0f;
    float fallingtimer = 0;
    float rememberATK =0f;
    int layerMask = LayerMask.GetMask("Enemy", "Breakable");
    private float startFallY;   // 进入下落状态时的 Y
    // 最小矩形尺寸（固定）
    readonly Vector2 baseSize = new Vector2(1f, 0.3f);
   public CollisionState(NewPlayerControll ctx)
    {
        _ctx = ctx;
    }
    public void Enter()
    {
        Debug.Log("进入冲击状态");
        //减一条命
        _ctx.DataMan.MinusHP();
        //数据初始化
        isfalling=false;
        fallSpeed = 0f;
        fallingtimer=0;
        rememberATK = 0f;
        //设置无敌
        _ctx.DataMan.SetInvencible(true);
        //设置速度
        _ctx.rb.velocity=new Vector2(0,0);
       _ctx. rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _ctx.rb.interpolation = RigidbodyInterpolation2D.Interpolate; // 顺便补帧，更顺滑

        //记录位置
        startFallY = _ctx.transform.position.y;
        //先往上飞一会

        _ctx.rb.velocity = new Vector2(0, _ctx.DataMan.flySpeed);

        TimeManager.Instance.OneTime(1f,
            () =>
            {
                _ctx.rb.velocity = new Vector2(0, -6f);
                _ctx.rb.gravityScale = 6;
                isfalling = true;

            }
            );
            //,
            //() =>
            //{
                
            //}
            //);



    }

   

    

    public void Exit()
    {
       _ctx. rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        _ctx.rb.interpolation = RigidbodyInterpolation2D.None;
        _ctx.DataMan.SetInvencible(false);
    }

    public void FixedUpdate()
    {
        
    }


   public void  Update()
    {
        if (_ctx.DataMan.isGround && isfalling == true)
        {
            Debug.Log("进行攻击判定");
            isfalling = false;
            _ctx.rb.gravityScale = 3;
            //创建攻击框
            SpawnLandingRect(fallSpeed);
            //动画特效


            if (fallingtimer > 0.3)
            {
                //屏幕震动
                SetShake(fallingtimer);
                //切换状态
                //再消耗一条命 如果速度足够快 
                _ctx.DataMan.MinusHP();
                //转换死亡状态 //临时默认状态 
                _ctx.SwitchState(TypeState.died);

            }
            else
            {
                if (_ctx.DataMan.currentHP > 0)
                {
                    _ctx.SwitchState(TypeState.ldle);
                }
                else
                {
                    _ctx.SwitchState(TypeState.died);
                }
            }
        }

     

        if (isfalling)
        {
            float max = _ctx.DataMan.MaxFallSpeed;
            fallingtimer += Time.deltaTime * 2f;
            fallSpeed = Mathf.Min(fallSpeed + Time.deltaTime * 20f, max);
        }
    }

    public void Attack()
    {

    }

    public void ContractPower()
    {

    }

    public void Dodge()
    {

    }

  
      private  void SpawnLandingRect(float speed)
        {
        Debug.Log("飞行时间为" + fallingtimer);

            float factor = _ctx.DataMan.LandingSizeFactor;          // <- 读数据类
            Vector2 size = baseSize + Vector2.one * (speed * factor);
            size.x = Mathf.Max(size.x, baseSize.x);
            size.y = Mathf.Max(size.y, baseSize.y);

            float groundY = _ctx.transform.position.y;
            Vector2 spawnPos = new Vector2(_ctx.transform.position.x,
                                           groundY + size.y * 0.5f);

        //生成伤害
     
        float fallDistance = startFallY - _ctx.transform.position.y;
        int damageValue = Mathf.RoundToInt(_ctx.DataMan.EvaluateFallDistanceDamage(fallDistance));
        rememberATK = damageValue;
        DamageData damage = new DamageData
        {
            atk = damageValue,
            type = DamageType.physics,
          //  attacker=_ctx.gameObject,
            RepellingXSpeed = speed / 4f,
            RepellingXDistance = speed / 10f,
            RepellingYSpeed = speed / 4f,
            RepellingYDistance = speed / 10f
        };

        //创建碰撞
        Collider2D[] hit = Physics2D.OverlapBoxAll(spawnPos, size, 0f, layerMask);
        foreach (Collider2D hit2 in hit)
        {
            if(hit2.CompareTag("EnemyOrPhyitems")&&hit2.TryGetComponent<IBeDamaged>(out IBeDamaged temp ))
            {
                temp.OnHurt(damage, _ctx.gameObject);
            }

        }

            GameObject go = new GameObject("LandingRect");
            go.transform.position = spawnPos;

            // 仅用于可视化，可调试用 SpriteRenderer
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            Texture2D tex = Texture2D.whiteTexture;
            Sprite sp = Sprite.Create(tex, new Rect(0, 0, 1, 1), Vector2.one * 0.5f, 1f);
            sr.sprite = sp;
            sr.color = new Color(1f, 0.3f, 0f, 0.7f);
        sr.sortingLayerName = "Game";
        go.transform.localScale = size;

           

            GameObject.Destroy(go, 10f);   // 1 秒后自动销毁

       
        }
      private void SetShake(float timer)
    {
        float time =Mathf.Clamp01(timer);
        CameraManager.Instance.CameraShake(time,time);
    }

    public void Block()
    {
        throw new System.NotImplementedException();
    }
}
