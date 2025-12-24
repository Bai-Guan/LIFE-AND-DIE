using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAlert
{
    void OnAlerted();
}



public class EnemyAlertNotice : MonoBehaviour
{
   

    [Header("探测尺寸")]
    [SerializeField] private float radius = 3f;               

    [Header("只扫哪些层")]
    [SerializeField] private LayerMask alertLayer;   // 在 Inspector 里把 Enemy 层勾上

    // 外部触发：你检测到“看见玩家”以后直接调这个
    public void OnSpotPlayer()
    {
        Debug.Log("敌人提醒同类发现玩家");
        // 1. 根据形状做 Physics 查询
        Collider2D[] hits;
      
       
            hits = Physics2D.OverlapCircleAll(transform.position, radius, alertLayer);
        

        // 2. 遍历结果，调用接口
        foreach (var col in hits)
        {
            // 跳过自己
            if (col.gameObject == gameObject) continue;

            // 取接口
            if (col.TryGetComponent(out IEnemyAlert enemy))
            {
                enemy.OnAlerted();
            }
        }

        // 3.（可选）调试用，发布前删掉
        DrawDebug();
    }

    // 只在 Scene 窗口画个线，方便调尺寸
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
     
            Gizmos.DrawWireSphere(transform.position, radius);
    }

    // 运行时想看一眼范围就手动调一下
    [ContextMenu("DrawDebug")]
    private void DrawDebug()
    {
            Debug.DrawLine(transform.position, transform.position + Vector3.right * radius, Color.yellow, 1f);
    }
}
