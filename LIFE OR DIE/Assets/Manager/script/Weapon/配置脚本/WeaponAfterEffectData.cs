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
    [Header("径向模糊设置,模糊强度推荐在0.1-0.9之间")]
    [SerializeField] public bool 是否径向模糊;
    [SerializeField] public float BlurTime;
    [SerializeField] public float BlurPow;
 

    [Header("色差设置 色差默认0.9f强度")]
    [SerializeField] public bool 是否色差;
    [SerializeField] public float ColorTime;

    [Header("抖动设置,抖动强度推荐在1之下")]
    [SerializeField] public bool 是否震动;
    [SerializeField] public float shakeTime;
    [SerializeField] public float shakePow;
}
