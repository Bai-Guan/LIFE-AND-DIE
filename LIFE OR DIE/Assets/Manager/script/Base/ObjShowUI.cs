using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIManager;

[RequireComponent(typeof(Collider2D))]
public class ObjShowUI : MonoBehaviour
{
    [SerializeField] public string KeyValue;
    [SerializeField] public string Discription;

    private PopUpBox current;
    //只负责UI显示 不负责逻辑
    void showUI()
    {

        PopUpBox box =  UIManager.Instance.OpenPopBox(UIConst.PopUpBox) as PopUpBox ;
        //if (box == null) return;
        if(box is PopUpBox pop)
        {
            //先唤醒
            pop.WakeUp();
            //再入栈通知工作
            StackInteraction.Instance.Push(this.transform.gameObject);

            current = pop;
           
            
        }
        else
        {
            Debug.LogWarning("检测失败");
        }
    }

    void ExitUI()
    {
        if (current == null) return;
        //将物品弹出 那个框会自己执行关闭流程
        StackInteraction.Instance.PopSomeOne(this.gameObject);

        //int count = current.Exit(this);
        //if (count <= 0)
        //{
        //    current = null;
        //    UIManager.Instance.ClosePanel(UIConst.PopUpBox);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            showUI();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            ExitUI();
    }
}
