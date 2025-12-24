using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class 一次性伤害 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            DamageData damageData = new DamageData();
            damageData.atk = 1;
            collision.GetComponent<IBeDamaged>().OnHurt(damageData,this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
