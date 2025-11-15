using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskSaveData 
{
    public string TaskSOID;
    public string TaskCur;
   public 任务状态 当前状态;
        
}
public enum 任务状态
{
    未激活,
    激活,
    失败,
    成功,
    已完成



}