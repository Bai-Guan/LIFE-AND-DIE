using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskCondition
{
    //public string TaskID;
    void StartListen();

    void StopListen();

 //   void SetData();

}

public struct 传入杀戮委托的数据
{
    public string 被杀生物的ID;

}

public class KillCondition : ITaskCondition
{
   private string targetName;
    public string TaskID;
 
  public  KillCondition(string taskID, string  targetName)
    {
        this.targetName = targetName;
        TaskID = taskID;
    }


        //public void SetData(string targetID, int targetCount, string TaskID, int currentkillCount)
    //{
    //    this.currentkillCount = currentkillCount;
    //    this.targetID = targetID;
    //    this.targetCount = targetCount;
    //    this.TaskID = TaskID;
    //}

    public void StartListen()
    {
       EventBus.Add<传入杀戮委托的数据>(OnKill);

      //  TaskManager.Instance.SaveTaskProcess(任务状态.激活, temp, TaskID);
    }

    public void StopListen()
    {
        EventBus.Remove<传入杀戮委托的数据>(OnKill);
    }

   public void OnKill(传入杀戮委托的数据 e)
    {
        Debug.Log($"收到ID='{e.被杀生物的ID}'  本地targetID='{targetName}'  相等? {e.被杀生物的ID == targetName}");
        if (e.被杀生物的ID == targetName)
        {
            Debug.Log("杀1！！！！");
            string temp = "已完成<color=#BE0000>杀戮</color>";
                StopListen();
               TaskManager.Instance.SaveTaskProcess(任务状态.成功, temp, TaskID);
            TaskManager.Instance.KillFinishTask(TaskID);
        }
    }
}

public class TalkCondition : ITaskCondition
{
    public bool IsDone()
    {
        throw new System.NotImplementedException();
    }

    public void StartListen()
    {
        throw new System.NotImplementedException();
    }

    public void StopListen()
    {
        throw new System.NotImplementedException();
    }
}
