using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 瓦片破坏 : MonoBehaviour, IBeDamaged
{
    private int HP = 50;
    public void OnHurt(DamageData damage, GameObject obj)
    {
      if(damage.hitType==HitType.heavy)
        {
            HP-=damage.atk;
        }

      if(HP<=0)
            {
            Destroy(this.gameObject);
        }
    }
}
