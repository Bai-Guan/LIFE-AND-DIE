using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 物体碰撞伤害 : MonoBehaviour
{
   [SerializeField] private int Atk = 100;
    private DamageData damageData = new DamageData()
    {
        RepellingXSpeed = 0,
        RepellingYSpeed = 0,
        type=DamageType.magic,
    };
    private void Awake()
    {
        damageData.atk = Atk;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.TryGetComponent<IBeDamaged>(out IBeDamaged temp))
        {
            temp.OnHurt(damageData,this.gameObject);
        }
    }
    
}
