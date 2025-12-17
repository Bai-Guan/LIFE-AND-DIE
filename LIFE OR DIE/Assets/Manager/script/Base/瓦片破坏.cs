using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 瓦片破坏 : MonoBehaviour, IBeDamaged
{

    [SerializeField] private string 受击音效;
    [SerializeField] private string 被破坏音效;
  [SerializeField]  private int HP = 50;
    public float OnHurt(DamageData damage, GameObject obj)
    {
      if(damage.hitType==HitType.heavy)
        {
            HP-=damage.atk;
            if(受击音效!=null&&HP>0)
            {
                AudioManager.Instance.PlaySFX(受击音效);
                CameraManager.Instance.CameraShake(0.2f, 0.3f);
            }
     
        }

      if(HP<=0)
            {
            if (被破坏音效 != null)
            {
                AudioManager.Instance.PlaySFX(被破坏音效);
                CameraManager.Instance.CameraShake(0.2f, 0.3f);
            }
         
            Destroy(this.gameObject);
          
        }
      return HP;
    }
}
