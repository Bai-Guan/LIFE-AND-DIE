using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeaponSpriteData : ComponentData
{
    //[field: SerializeReference]
    //������������ÿ��������������Ĺ�����
  
    public ComponentData Data;
    [SerializeField] public AttackSprite[] AttackData;
}
