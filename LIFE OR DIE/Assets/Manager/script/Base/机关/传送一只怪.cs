using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 传送一只怪 : MonoBehaviour
{
    [Header("物体出现位置")]
    [SerializeField] private Transform pos;
  [SerializeField]  private GameObject obj;
    private bool clock = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pos == null) return;
        if (obj == null) return;
        if (collision.CompareTag("Player")&&clock==false)
        {
            clock = true;
            obj.SetActive(true);
            obj.transform.position = pos.position;
        }
    }
}
