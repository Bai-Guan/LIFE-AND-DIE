using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 上锁铁门 : MonoBehaviour
{
    private bool block = false;
    private NewPlayerControll NewPlayerControll;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NewPlayerControll = collision.GetComponent<NewPlayerControll>();
            if (NewPlayerControll != null)
                NewPlayerControll.OnInteractPressed += 打开铁门;
        }
    }
    private void 打开铁门()
    {
        if (block == true) return;
        if (StackInteraction.Instance.Peek() == this.gameObject && 玩家的全局变量.钥匙数量 > 0)
        {
            玩家的全局变量.钥匙数量 -= 1;
      
            StackInteraction.Instance.PopSomeOne(this.gameObject);
            Destroy(this.gameObject);
        }
        if (StackInteraction.Instance.Peek() == this.gameObject && 玩家的全局变量.钥匙数量 <= 0)
        {
            block = true;
         执念涌现的脚本 temp = UIManager.Instance.OpenPanel(UIManager.UIConst.addTask) as 执念涌现的脚本;
            StackInteraction.Instance.PopSomeOne(this.gameObject);
            temp.改字("这扇门上锁了");
            TimeManager.Instance.OneTime(2f, () => UIManager.Instance.ClosePanel(UIManager.UIConst.addTask, true));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (NewPlayerControll != null)
            NewPlayerControll.OnInteractPressed -= 打开铁门;
        block=false;
    }

    private void OnDestroy()
    {
        if (NewPlayerControll != null)
            NewPlayerControll.OnInteractPressed -= 打开铁门;
    }
}
