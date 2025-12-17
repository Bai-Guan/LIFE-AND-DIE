using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 一次性瓦砖 : MonoBehaviour
{
    [SerializeField] private List<GameObject> list;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            foreach (GameObject go in list)
            {
                if (go != null)
                {
                    Destroy(go);
                }
            }
            AudioManager.Instance.PlaySFX("碎石1");
        }
    }
}
