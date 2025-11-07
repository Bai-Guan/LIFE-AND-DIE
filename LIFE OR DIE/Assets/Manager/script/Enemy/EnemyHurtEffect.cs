using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtEffect : MonoBehaviour
{
    private InitEnemySystem body;
   [SerializeField] float playTime = 0.5f;
   [SerializeField] ParticleSystem 受击特效粒子发射器;

    private bool isEmitting=false;   
    private void Awake()
    {
        body = GetComponent<InitEnemySystem>();
        受击特效粒子发射器 = this.transform.Find("Effect").GetComponent<ParticleSystem>();

        if (body == null)
        {
            Debug.LogWarning("当前敌人未装有初始化器,受击特效模块失效");
            this.enabled = false;
            return;
        }
        if(受击特效粒子发射器==null)
        {
            Debug.LogWarning("当前敌人未装有粒子发射器,受击特效模块失效");
            this.enabled = false;
            return;
        }
    
        
    }
    // Start is called before the first frame update
    void Start()
    {
        body.BeAttack += OnPlay;
        受击特效粒子发射器.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        受击特效粒子发射器.gameObject.SetActive(false);
    }

 
    private void OnPlay()
    {
        if(isEmitting) {return;}
        if (受击特效粒子发射器 == null) return;
        isEmitting = true;
        受击特效粒子发射器.gameObject.SetActive(true);
        受击特效粒子发射器.Play();
        TimeManager.Instance.OneTime(playTime,
          () =>
          {
              StopAndReset();
          }
          );
    }

    public void StopAndReset()
    {
        if (受击特效粒子发射器 == null) return;
        受击特效粒子发射器.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            isEmitting = false;
        受击特效粒子发射器.gameObject.SetActive(false);

    }

    private void Reset()
    {
        
    }
    private void OnDisable()
    {
        body.BeAttack -= OnPlay;
    }
}
