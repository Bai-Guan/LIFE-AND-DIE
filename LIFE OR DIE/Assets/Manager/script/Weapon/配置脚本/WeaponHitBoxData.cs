using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponHitBoxData : ComponentData
{
    [field: SerializeField]public LayerMask DetectableLayer {  get; private set; }
    [field: SerializeField]public HitBoxSizeAndOffset[] HitBoxSizeAndOffsets{  get; private set; }
}

[System.Serializable]
public class HitBoxSizeAndOffset
{
 [SerializeField]  public bool isDebug;
    [field: SerializeField] public Rect HitBox { get; private set; }
}
