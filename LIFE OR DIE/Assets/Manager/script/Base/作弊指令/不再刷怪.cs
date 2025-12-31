using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class 不再刷怪 : MonoBehaviour
{

    private bool isEnter = false;
    private NewPlayerControll ctx;
    [SerializeField] private SpriteRenderer sr;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            isEnter = true;
        
            ctx = collision.GetComponent<NewPlayerControll>();
            ctx.OnInteractPressed += 不刷怪;
        }
    }
    private void 不刷怪()
    {

        玩家的全局变量.是否刷怪 = false;
        if(sr != null) 
        sr.color=Color.red;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ctx.OnInteractPressed -= 不刷怪;
            isEnter = false;
        }

    }
    private void OnDestroy()
    {
        if (ctx != null)
            ctx.OnInteractPressed -= 不刷怪;
    }
}
