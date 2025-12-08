using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 摩擦力 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
            c.transform.SetParent(transform, true);   // 关键
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
            c.transform.SetParent(null, true);
    }
}
