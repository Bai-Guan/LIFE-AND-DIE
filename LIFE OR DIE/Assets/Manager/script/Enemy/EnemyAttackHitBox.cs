using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHitBox : MonoBehaviour
{
    [SerializeField] public EnemyHitBoxSO allAttacks;

    public bool isDebug = true;
    private Dictionary<string, EHBData> map;            // 名字→数据
    private InitEnemySystem body;
    private DamageData damage;
    private void Awake()
    {
        body = GetComponent<InitEnemySystem>();   
        map = new Dictionary<string, EHBData>();
        foreach(var temp in allAttacks.hitBoxes)
        {
            map.Add(temp.attackName, temp);
        }
    damage = new DamageData();
        damage.atk = 9999;
    }

    /* 动画事件只会调这一句话，完全复用 */
    public void OnAnimEvent_FireAttack(string attackName)
    {
        if (map.TryGetValue(attackName, out var clip))
        {
           StartCoroutine(DoHitBoxes(clip));
        }

    }



    private IEnumerator DoHitBoxes(EHBData clip)
    {
        int frame = 0;
        while (frame < clip.duration)
        {
            CheckHits(clip);          // 每帧扫一次
            yield return null;        // 等一帧
            frame++;
        }
    }

    private void CheckHits(EHBData clip)
    {
        damage.type = clip.damageType;
        int filp=body.isFacingLeft ? -1 : 1;
            Vector2 center = transform.position + new Vector3(clip.hitBoxes.x*filp, clip.hitBoxes.y);
            Collider2D[] cols = Physics2D.OverlapBoxAll(center, clip.hitBoxes.size, 0f, LayerMask.GetMask("Player"));
        foreach (var col in cols)
        {
            col.GetComponent<IBeDamaged>().OnHurt(damage,this.gameObject);
        }
        
    }





    /* —— 可视化调试 —— */
    private void OnDrawGizmos()
    {
      
        if (allAttacks == null) return;

        foreach (var clip in allAttacks.hitBoxes)
        {
            if (!clip.isDebug) continue;

            /* 用 TransformPoint 把本地偏移转世界 */
            Vector2 center = transform.TransformPoint(new Vector2(clip.hitBoxes.x, clip.hitBoxes.y));

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, clip.hitBoxes.size);
        }


    }

    private void DrawClipGizmos(Rect clip, Color c)
    {
        Gizmos.color = c;
        
       
            Vector2 center = transform.position + new Vector3(clip.x, clip.y);
            Gizmos.DrawWireCube(center, clip.size);
        
    }
}
