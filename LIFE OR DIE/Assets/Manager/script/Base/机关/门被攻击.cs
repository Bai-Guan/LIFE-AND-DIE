using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 门被攻击 : MonoBehaviour, IBeDamaged
{
 [SerializeField]  private Explodable 门上;
    [SerializeField] private Explodable 门下;
    private void Start()
    {
        门上 =this.transform.Find("门上").GetComponent<Explodable>();
        门上 = this.transform.Find("门下").GetComponent<Explodable>();

        if (门上 == null || 门下 == null)
            Debug.LogError("未找到子类");
    }
    public float OnHurt(DamageData damage, GameObject obj)
    {
        Vector2 dir = obj.transform.position;
      门上.explode(damage,dir);
        门下.explode(damage, dir);
        AudioManager.Instance.PlaySFX("破门");
        Destroy(this.gameObject);
        return 1;
    }
}
