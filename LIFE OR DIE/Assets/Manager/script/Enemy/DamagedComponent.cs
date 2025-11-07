using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DamagedComponent : MonoBehaviour, IBeDamaged
{
    private InitEnemySystem body;
    private bool isMinusHP =true;
    private bool isCanRepel = true;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        body = GetComponent<InitEnemySystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(body==null)
        {
            Debug.LogWarning("当前敌人未装有初始化器,受击模块失效");
            this.enabled = false;
            return;
        }
    }

    private void Start()
    {
        body.beAttacked += ReturnRBandHP;
    }

    void ReturnRBandHP(DamageData data, GameObject attacker)
    {
        OnHurt(data, attacker);
       
    }

    public void OnHurt(DamageData data, GameObject attacker)
    {
        if (data == null)
        {
            body.MinusHP(0);
            body.SetRBvelcoity(new Vector2(0, 0));
            Debug.LogWarning("传入攻击为空");
            return;
        }

        // 1. 伤害
        int damage = data.type switch
        {
            DamageType.physics => Mathf.Max(1, data.atk - body.Defenese),
            DamageType.magic => data.atk,
            _ => data.atk
        };

        // 2. 击退强度
        float resist = Mathf.Clamp01(body.Knocked * 0.01f);
        float scale = 1f - resist;

        // 3. 方向计算
        Vector2 attackerPos = attacker.transform.position;
        Vector2 victimPos = transform.position;

        // X：左右方向
        float dirX = Mathf.Sign(victimPos.x - attackerPos.x);

        // Y：上下方向（带容错）
        float verticalThreshold = 0.5f; // 单位：米
        float dirY = attackerPos.y - victimPos.y > verticalThreshold ? -1f   // 从上方攻击 → 向下击飞
                                                                     : 1f;  // 否则默认向上

        // 得到方向
        Vector2 dir2= new Vector2(dirX, dirY);

        // 4. 最终速度
        float vx = data.RepellingXSpeed * scale * dirX;
        float vy = data.RepellingYSpeed * scale * dirY;

        Vector2 v = new Vector2(vx, vy);

        if (isMinusHP == false) { damage = 0; }
        if (isCanRepel == false) { v.Set(0, 0); }

        //5.改变数据
        Debug.Log("造成" + damage + "伤害");
     
        body.MinusHP(damage);
        body.SetRBvelcoity(v);
        body.SetLastDamageData(data, dir2);
        //6.播放特效 音效
        // Debug.Log(this.name + "被攻击了");
        if(body.CurrentHP>0)
        AudioManager.Instance.PlaySFX(AudioManager._肉受击音效);
        body.BeAttacked();
        //测试 攻击特效由攻击物体决定 此代码写在weapon附件下
        //  EffectManager.Instance.SpeicalEffectKnife(this.transform, 0.8f, 5f);
        // EffectManager.Instance.ChromaticAberrationSet(0.8f, 1f);
        //  EffectManager.Instance.VerticalBlur(0.8f, 0.9f);
        //InvincibleRendered(0.8f);
        //CameraManager.Instance.CameraShake(2f, 1f);
    }
    //被攻击白色闪烁
    public void InvincibleRendered(float t)
    {
        TimeManager.Instance.FrameTime(t,
            () =>
            {
                float e = Mathf.PingPong(Time.time * 4f, 0.7f);
                Color color = new Color(e, e, e);
                spriteRenderer.color = color;


            },
            () =>
            {
                spriteRenderer.color = Color.black;
            }
            );
    }

    private void OnDisable()
    {
        body.beAttacked -= ReturnRBandHP;
    }


}
