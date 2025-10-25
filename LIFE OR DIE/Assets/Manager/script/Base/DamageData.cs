using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageData 
{
    [SerializeField] public int atk;
    [SerializeField] public DamageType type;
    [Header("����ֵΪ���˾���")]
    [SerializeField] public float RepellingXDistance;
    [SerializeField] public float RepellingYDistance;
    [Header("�����ٶ�")]
    [SerializeField] public float RepellingXSpeed;
    [SerializeField] public float RepellingYSpeed;


}
public enum DamageType
{
    physics,
    magic,
}
