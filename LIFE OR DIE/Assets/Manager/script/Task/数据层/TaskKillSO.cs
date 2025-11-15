using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Task/Kill")]
public class TaskKillSO : TaskSO
{
   
    [SerializeField] public string targetName;

    public override ITaskCondition CreateCondition()
    {
        KillCondition temp = new(TaskID, targetName);
     
        return temp;
    }
}
