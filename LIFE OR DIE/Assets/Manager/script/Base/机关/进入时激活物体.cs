using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 进入时激活物体 : MonoBehaviour
{
   [SerializeField] GameObject obj;
    private void Awake()
    {
        if(obj != null ) 
            obj.SetActive( false );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            obj.SetActive( true );
        }
    }
}
