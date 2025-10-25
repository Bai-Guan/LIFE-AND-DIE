using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeDamaged 
{
    public void OnHurt(DamageData damage, GameObject obj);
}
