using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageData 
{
    [SerializeField] public int atk;
    [SerializeField] public DamageType type;
    [SerializeField] public HitType hitType;
  //  [SerializeField] public GameObject attacker;
    [Header("击退值为击退距离")]
    [SerializeField] public float RepellingXDistance;
    [SerializeField] public float RepellingYDistance;
    [Header("击退速度")]
    [SerializeField] public float RepellingXSpeed;
    [SerializeField] public float RepellingYSpeed;

    //[Header("打击音效")]
    //[SerializeField] public string AudioName;
   // [Header("打击特效")]
    //TODO;


}
public enum DamageType
{
    physics,
    magic,
}
public enum HitType
{
    light,
    heavy
}