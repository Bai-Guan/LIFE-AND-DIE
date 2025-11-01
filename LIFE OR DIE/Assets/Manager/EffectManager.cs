using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    private static EffectManager effectManager;
    public static EffectManager Instance { get { return effectManager; } }

    [SerializeField] GameObject PreferKnifeLight;

   private GameObject cameraRadialBlur;
    [SerializeField] private GameObject fuckobj;
    [SerializeField] private Camera cameraRadial;
    private SpriteRenderer rmRenderer;
     private Material RadialMater;
    private bool isRadialBlur = false;

    private Volume volume;
    private ChromaticAberration ca;
    private bool isChromaticAb = false;

    public const float SCALE_FLOAT = 0.9f;

    public const float 刀光除数 = 5f;



    private const float InTime = 0.08f;   // 淡入占 8%
    private const float OutTime = 0.25f;   // 淡出占 25%
    private void Awake()
    {
        if(effectManager != null &&effectManager !=this)
        {
            Destroy(gameObject);
        }
        else
        {
            effectManager = this;
        }

        InitName();
    }

    private void Start()
    {
        if (cameraRadial != null && RadialMater != null)
        {
        cameraRadial.enabled = false;
        RadialMater.SetFloat("_Scale", 0);
            fuckobj.SetActive(false);
            isRadialBlur = false;
         }
        else
        {
            Debug.LogWarning("未找到径向模糊渲染器");
        }
    }


    void InitName()
    {
        volume = Camera.main.GetComponent<Volume>();
        if (volume == null) { Debug.LogWarning("特效管理器未能找到后处理"); }
        if (PreferKnifeLight == null) { Debug.LogWarning("特效管理器未能找到刀光预制体"); }
        if(!volume.profile.TryGet<ChromaticAberration>(out ca))
        {
            ca = volume.profile.Add<ChromaticAberration>();
        }
        ca.intensity.value = 0;             // 初始关闭
        ca.active = true;                   // 确保生效
       cameraRadial= GameObject.Find("cameraRadialBlur").GetComponent<Camera>();
        fuckobj = cameraRadial.transform.Find("fuckShader").gameObject;
       rmRenderer=fuckobj.GetComponent<SpriteRenderer>();
        RadialMater = rmRenderer.material;


    }

    public void SpeicalEffectKnife(Transform where,float durTime,float size)
    {
        if (PreferKnifeLight == null) return;
      float range= UnityEngine.Random.Range(-180, 180);
        GameObject light = Instantiate(PreferKnifeLight);
      light.GetComponent<KnifeLight>().Set(where,durTime,size,range);
    }
    public void SpeicalEffectKnife(Transform where, float durTime, float size,float range)
    {
        if (PreferKnifeLight == null) return;
        float du = UnityEngine.Random.Range(-10, 10);
        GameObject light = Instantiate(PreferKnifeLight);
        light.GetComponent<KnifeLight>().Set(where, durTime, size, range+du);
    }
    //径向模糊
    public void VerticalBlur(float durTime,float power)
    {
        if (isRadialBlur == true) return;
        if (cameraRadial == null||RadialMater == null)return;
        print("径向模糊启动");
        cameraRadial.enabled = true;
        fuckobj.SetActive(true);

        float timer = 0;
       
        TimeManager.Instance.FrameTime(durTime,
           () =>
           {
               timer += Time.deltaTime;
               float progress = timer / durTime;
               float strength;

               // 使用百分比控制的平滑渐进渐出
               if (progress <= InTime)
               {
                   // 淡入阶段：从0到power
                   strength = Mathf.Lerp(0f, power, progress / InTime);
               }
               else if (progress >= 1f - OutTime)
               {
                   // 淡出阶段：从power到0
                   strength = Mathf.Lerp(power, 0f, (progress - (1f - OutTime)) / OutTime);
               }
               else
               {
                   // 保持阶段：保持power
                   strength = power;
               }

               RadialMater.SetFloat("_Scale", strength);
           }
           ,
           () =>
           {
               isRadialBlur = false;
               cameraRadial.enabled=false;
               fuckobj.SetActive(false);
               RadialMater.SetFloat("_Scale", 0);
           }

           );
   
    }

    //色差特效
    public void ChromaticAberrationSet(float durTime, float power)
    {
        if(isChromaticAb == true) return;
        ca.intensity.value =Mathf.Clamp01( power);
        isChromaticAb = true;
        TimeManager.Instance.OneTime(durTime,
            () =>
            {
                ca.intensity.value = 0;
                isChromaticAb = false;
            }
            );
    }


}
