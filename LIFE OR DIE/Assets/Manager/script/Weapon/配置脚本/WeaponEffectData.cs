using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffectData : ComponentData
{
  [SerializeField] public  WEffectData[] ListEffectDatas;
}
[System.Serializable]
public class WEffectData
{
    [SerializeField] public GameObject preEffect;
    [SerializeField] public float durTime;
    [SerializeField] public float size;
    [SerializeField] public float range;
    [SerializeField] public bool isRandom;
}