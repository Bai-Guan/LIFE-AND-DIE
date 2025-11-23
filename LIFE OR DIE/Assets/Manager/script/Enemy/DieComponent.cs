using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class DieComponent : MonoBehaviour
{
    private InitEnemySystem body;
    private Explodable explodable;
    [SerializeField] public string DieAudioName;
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
    public void SpecialDieEffect(GameObject murderer)
    {
        //再次检测当前物体是否死亡 是否已经死亡
        if (!(body.CurrentHP <= 0)) return;
        if(FirstTime==false) return;
        FirstTime = false;




        //正常死亡逻辑处理

        //-----------------下面开启碎片死亡-----
        if (CanExploable)
        {
           if(DieAudioName != null) AudioManager.Instance.PlaySFX(DieAudioName);
            
            explodable.explode(body.LastDamage,body.LastDir);
        }

        //传递事件
        传入杀戮委托的数据 temp = new 传入杀戮委托的数据();
        temp.被杀生物的ID=body.BiologicalName;
        temp.凶手 = murderer;
        EventBus.Publish<传入杀戮委托的数据>(temp);
    }

    private void OnDisable()
    {
        body.Die -= SpecialDieEffect;
    }
}
