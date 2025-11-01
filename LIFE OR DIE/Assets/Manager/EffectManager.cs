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

    public const float ������� = 5f;



    private const float InTime = 0.08f;   // ����ռ 8%
    private const float OutTime = 0.25f;   // ����ռ 25%
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
            Debug.LogWarning("δ�ҵ�����ģ����Ⱦ��");
        }
    }


    void InitName()
    {
        volume = Camera.main.GetComponent<Volume>();
        if (volume == null) { Debug.LogWarning("��Ч������δ���ҵ�����"); }
        if (PreferKnifeLight == null) { Debug.LogWarning("��Ч������δ���ҵ�����Ԥ����"); }
        if(!volume.profile.TryGet<ChromaticAberration>(out ca))
        {
            ca = volume.profile.Add<ChromaticAberration>();
        }
        ca.intensity.value = 0;             // ��ʼ�ر�
        ca.active = true;                   // ȷ����Ч
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
    //����ģ��
    public void VerticalBlur(float durTime,float power)
    {
        if (isRadialBlur == true) return;
        if (cameraRadial == null||RadialMater == null)return;
        print("����ģ������");
        cameraRadial.enabled = true;
        fuckobj.SetActive(true);

        float timer = 0;
       
        TimeManager.Instance.FrameTime(durTime,
           () =>
           {
               timer += Time.deltaTime;
               float progress = timer / durTime;
               float strength;

               // ʹ�ðٷֱȿ��Ƶ�ƽ����������
               if (progress <= InTime)
               {
                   // ����׶Σ���0��power
                   strength = Mathf.Lerp(0f, power, progress / InTime);
               }
               else if (progress >= 1f - OutTime)
               {
                   // �����׶Σ���power��0
                   strength = Mathf.Lerp(power, 0f, (progress - (1f - OutTime)) / OutTime);
               }
               else
               {
                   // ���ֽ׶Σ�����power
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

    //ɫ����Ч
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
