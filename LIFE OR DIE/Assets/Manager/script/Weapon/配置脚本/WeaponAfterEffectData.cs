using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WeaponAfterEffectData : ComponentData
{
    [SerializeField] public AfterEffectType[] _AfterEffectsdata;

}
[System.Serializable]
public class AfterEffectType
{
    [Header("����ģ������,ģ��ǿ���Ƽ���0.1-0.9֮��")]
    [SerializeField] public bool �Ƿ���ģ��;
    [SerializeField] public float BlurTime;
    [SerializeField] public float BlurPow;
 

    [Header("ɫ������ ɫ��Ĭ��0.9fǿ��")]
    [SerializeField] public bool �Ƿ�ɫ��;
    [SerializeField] public float ColorTime;

    [Header("��������,����ǿ���Ƽ���1֮��")]
    [SerializeField] public bool �Ƿ���;
    [SerializeField] public float shakeTime;
    [SerializeField] public float shakePow;
}
