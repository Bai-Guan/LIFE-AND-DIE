using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 拾取钥匙 : MonoBehaviour
{
    private NewPlayerControll NewPlayerControll;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NewPlayerControll = collision.GetComponent<NewPlayerControll>();
            if (NewPlayerControll != null)
                NewPlayerControll.OnInteractPressed += 拾取;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (NewPlayerControll != null)
            NewPlayerControll.OnInteractPressed -= 拾取;
    }
    private void OnDestroy()
    {
        if (NewPlayerControll != null)
            NewPlayerControll.OnInteractPressed -= 拾取;
    }
    private void 拾取()
    {
        if (StackInteraction.Instance.Peek() == this.gameObject)
        {
            玩家的全局变量.钥匙数量 += 1;
            StackInteraction.Instance.PopSomeOne(this.gameObject);
            执念涌现的脚本 temp = UIManager.Instance.OpenPanel(UIManager.UIConst.addTask) as 执念涌现的脚本;
            temp.改字("拾取了一把钥匙");
            TimeManager.Instance.OneTime(3f, () => UIManager.Instance.ClosePanel(UIManager.UIConst.addTask,true));
            Destroy(this.transform.parent.gameObject);
        }
   
    }
}
