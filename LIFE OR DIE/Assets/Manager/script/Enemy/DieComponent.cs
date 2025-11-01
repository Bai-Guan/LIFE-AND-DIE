using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class DieComponent : MonoBehaviour
{
    private InitEnemySystem body;
    private Explodable explodable;

    private bool CanExploable=true;
    private bool FirstTime=true;
    private void Awake()
    {
        body = GetComponent<InitEnemySystem>();
      explodable = GetComponent<Explodable>();
        if (body == null)
        {
            Debug.LogWarning("当前敌人未装有初始化器,死亡模块失效");
            this.enabled = false;
            return;
        }
        if (explodable == null)
        {
            Debug.LogWarning("当前敌人未装有碎片器,死亡碎片效果禁用");
            CanExploable = false;
          
        }
    }
    private void Start()
    {
        body.Die += SpecialDieEffect;
    }
    public void SpecialDieEffect()
    {
        //再次检测当前物体是否死亡 是否已经死亡
        if (!(body.CurrentHP <= 0)) return;
        if(FirstTime==false) return;
        FirstTime = false;
        //正常死亡逻辑处理

        //-----------------下面开启碎片死亡-----
        if (CanExploable)
        {
            print("死亡碎片");
            explodable.explode(body.LastDamage,body.LastDir);
        }
    }

    private void OnDisable()
    {
        body.Die -= SpecialDieEffect;
    }
}
