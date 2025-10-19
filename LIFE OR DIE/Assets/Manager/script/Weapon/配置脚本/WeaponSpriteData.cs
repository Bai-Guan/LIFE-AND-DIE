using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeaponSpriteData : ComponentData
{
    //[field: SerializeReference]
    //用于整个武器每个攻击动作所需的攻击组
  
    public ComponentData Data;
    [SerializeField] public AttackSprite[] AttackData;
}
