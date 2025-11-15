using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskSO : ScriptableObject
{
    [SerializeField] public string TaskID;
    [Header("这里特指任务栏中的名称，与TaskID不同")]
    [SerializeField] public string TaskName;
    [SerializeField] public string TaskDescription;
    [SerializeField] public string 任务默认状态;
    public abstract ITaskCondition CreateCondition();
}

