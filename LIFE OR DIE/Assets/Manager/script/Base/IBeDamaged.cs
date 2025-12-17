using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeDamaged 
{
    public float OnHurt(DamageData damage, GameObject obj);
}
