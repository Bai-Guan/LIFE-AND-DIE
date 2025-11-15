using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 任务系统测试 : MonoBehaviour
{
    [SerializeField] public TaskSO task;
    public void Debug按钮添加任务小女孩()
    {
        TaskManager.Instance.AddTask(task);
    }
    public void Debug按钮完成小女孩任务()
    {
       传入杀戮委托的数据 e = new 传入杀戮委托的数据();
        e.被杀生物的ID ="小女孩";
        EventBus.Publish<传入杀戮委托的数据>(e);
    }
    public void Debug打开UI栏()
    {
        TaskManager.Instance.NormalOpenTaskUI();
    }

    public void Debug特殊打开()
    {
        TaskManager.Instance.KillFinishTask(task.TaskID);
    }

    public void Debug关闭UI()
    {
        TaskManager.Instance.QuitTaskUI();
    }
}
