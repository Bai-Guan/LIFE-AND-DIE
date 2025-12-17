using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XResetAndSetPos : MonoBehaviour
{
    [Header("物体出现位置")]
    [SerializeField] private Transform pos;
    private GameObject obj;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(pos==null) return;
        if(collision.CompareTag("Player"))
        {
            obj=collision.gameObject;
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, rb.velocity.y);
            obj.transform.position = pos.position;
        }
    }
}
