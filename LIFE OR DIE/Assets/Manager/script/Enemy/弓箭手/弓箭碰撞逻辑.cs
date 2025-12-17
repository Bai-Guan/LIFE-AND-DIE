using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 弓箭碰撞逻辑 : MonoBehaviour
{
    [SerializeField] private float TheLongestSaveTime = 15f;
    private float timer = 0;

    // 定义受伤接口
    private GameObject 发射者;

    private bool isAttached = false; // 是否已经附着在目标上
    private Transform attachedTarget; // 附着的目标
    private Rigidbody2D rb;

    DamageData damagedata = new DamageData()
    {
        atk = 100,
        type = DamageType.physics,
        hitType = HitType.light,
        RepellingXSpeed = 2,
        RepellingXDistance = 0.2f,
    };

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }
    public void 设置发射者(GameObject obj)
    {
        发射者=obj;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        // 如果已经附着到目标上，跟随目标移动
        if (isAttached && attachedTarget != null)
        {
            transform.position = attachedTarget.position;
            if(attachedTarget == null) 
                Destroy(this.gameObject);
        }

        // 基础销毁时间
        if (timer > TheLongestSaveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果已经附着或碰撞过，不再处理
        if (isAttached) return;


        if (collision.gameObject==发射者)
        {
            return;
        }
        // 检查是否实现了受伤接口
        IBeDamaged hurtable = collision.GetComponent<IBeDamaged>();

        if (hurtable != null)
        {
            // 对目标造成伤害
           float hp= hurtable.OnHurt(damagedata,this.gameObject);
            if(hp==-1)
            EffectManager.Instance.Play("飞血", this.transform);
            if(hp==1)
                EffectManager.Instance.Play("火花效果", this.transform);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            // 碰到墙壁或地板，停止运动
            StopMovement();
        }
    }

    private void AttachToTarget(Transform target)
    {
        isAttached = true;
        attachedTarget = target;

        // 停止物理运动
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // 禁用碰撞检测，避免多次触发
        GetComponent<Collider2D>().enabled = false;

        // 设置10秒后销毁
        timer = 0;
        TheLongestSaveTime = 10f;
    }

    private void StopMovement()
    {
        // 停止物理运动
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // 禁用碰撞检测
        GetComponent<Collider2D>().enabled = false;

        // 设置10秒后销毁
        timer = 0;
        TheLongestSaveTime = 10f;
    }


}
