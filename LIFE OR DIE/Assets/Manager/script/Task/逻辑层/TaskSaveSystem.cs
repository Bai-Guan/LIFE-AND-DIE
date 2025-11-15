using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TaskSaveSystem
{
    private static TaskSaveSystem _instance;
    public static TaskSaveSystem Instance
    {
        get {
            if(_instance == null)
                _instance = new TaskSaveSystem();
        return _instance;}
    }

    public string FileTestPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), // 桌面
 "Tasksave.json");


    TaskSaveDataWrapper wrapper;
    //todo  这个包装类.


    public void SaveTaskData(List<TaskSaveData> saveData)
    {
        if (wrapper == null) { wrapper = new TaskSaveDataWrapper();}

        wrapper.taskList = saveData;

        string jsonTaskData = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(FileTestPath, jsonTaskData);
    }
    public  List<TaskSaveData> LoadTaskData()
    {


        if (File.Exists(FileTestPath))
        {
            string jsonTaskData = File.ReadAllText(FileTestPath);

            wrapper = JsonUtility.FromJson<TaskSaveDataWrapper>(jsonTaskData);
        }
        else
        {
            Debug.LogWarning("未找到任务的储存数据路径");
        }
        if (wrapper == null || wrapper.taskList == null)
        {
            wrapper = new TaskSaveDataWrapper();
            wrapper.taskList= new List<TaskSaveData>();
        }

  
        return wrapper.taskList;


    }
}

// 创建一个包装类来包含List
[System.Serializable]
public class TaskSaveDataWrapper
{
    public List<TaskSaveData> taskList;
}
