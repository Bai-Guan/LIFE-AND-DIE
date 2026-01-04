using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class 玩家死亡时候切换关卡 : MonoBehaviour
{
    private PlayerInput pi;
    private PlayerDataManager pdm;
    bool isEnter=false;
    //[SerializeField] private int 关卡编号 = 1;
    private bool clock = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            pi=collision.GetComponent<PlayerInput>();
            pdm=collision.GetComponent<PlayerDataManager>();
            isEnter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
             isEnter=false;
        }
    }

    private void Update()
    {
        //优雅一点就用事件
        if(玩家的全局变量.玩家是否死亡==true&&isEnter==true&&clock==false)
        {
           
            弹出UI();
        }
    }
    private void 弹出UI()
    {
        if (pi == null) return;
        clock = true;
        pdm.currentHP = 1;
        pi.SwitchCurrentActionMap("Inventory");
        TimeManager.Instance.OneTime(1.5f, () => UIManager.Instance.OpenPanel(UIManager.UIConst.FallAmbush));
     
    }

}
