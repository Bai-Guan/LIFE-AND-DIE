using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathDrop : MonoBehaviour
{
    [SerializeField]private GameObject 物品;
    private InitEnemySystem body;
    void Start()
    {
        if (body == null)
        {
            body = GetComponent<InitEnemySystem>();
            body.Die += 爆装备;
        }
        
    }
    void 爆装备(GameObject obj)
    {
        GameObject temp = Instantiate(物品);
        temp.transform.position=this.transform.position;
      if( temp.TryGetComponent<Rigidbody2D>( out Rigidbody2D rb))
        {
            rb.velocity = new Vector2(0, 5f);
        }
    }
    private void OnDestroy()
    {
        body.Die -= 爆装备;
    }

}
