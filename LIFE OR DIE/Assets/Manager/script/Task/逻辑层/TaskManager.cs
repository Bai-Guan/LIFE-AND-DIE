using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskManager
{
    private static TaskManager _instance;
    public static TaskManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TaskManager();
            return _instance;
        }
    }

    private TaskUI currentTaskUI;

    // 运行时户口册
    private readonly Dictionary<string, TaskRuntime> _runtimeDict =
        new Dictionary<string, TaskRuntime>();

    private List<TaskSaveData> _saveData;   // 身份证列表
    private bool _running = false;
    //这个方法用于读取任务的进度数据 并且根据这些数据初始化
    public void InitData()
    {
        //    _saveData = TaskSaveSystem.Instance.LoadTaskData();

        //  //  if (_saveData.Count != 0) { Debug.Log("数据加载成功了！！"); }


        //    //根据保存的数据 完善字典
        //    foreach (var task in _saveData)
        //    {
        //        Debug.Log("TaskSO/" + task.TaskSOID);
        //        TaskSO temp = Resources.Load<TaskSO>("TaskSO/" + task.TaskSOID);
        //        if (temp != null)
        //        {
        //            TaskRuntime RUNtime = new TaskRuntime();
        //            RUNtime.taskSO = temp;
        //            RUNtime.saveData = task;
        //            _runtimeDict.Add(task.TaskSOID, RUNtime);

        //            //查看激活状态，添加监听事件
        //            if (task.当前状态 == 任务状态.激活)
        //            {
        //                //添加监听事件
        //                var condition= temp.CreateCondition();

        //                condition.StartListen();
        //            }
        //        }
        //        else
        //        {
        //            Debug.LogWarning("任务字典SO读取失败");
        //        }
        //    }
        //}
        // 清零：不管以前存了什么，直接新建空列表
        _saveData = new List<TaskSaveData>();
        _runtimeDict.Clear();          // 运行时字典也清空
    }


  public  void NormalOpenTaskUI()
    {
        if(_running==true) return;
        _running = true;
        if (UIManager.Instance.OpenPanel(UIManager.UIConst.TaskBox) is TaskUI ui)
        {
            currentTaskUI = ui;
        }
        else 
        {
            Debug.LogWarning("未找到" + UIManager.UIConst.TaskBox);
            QuitTaskUI();
            return;
        }

        //先把右侧的给关了
        currentTaskUI.隐藏右侧物体();
        //创建任务选项
        for (int i = 0; i < _saveData.Count; i++)
        {
          if(  _runtimeDict.TryGetValue( _saveData[i].TaskSOID,out TaskRuntime runtime) )
            {
                currentTaskUI.创建任务实体并添加至列表(runtime.taskSO.TaskName, i);
            }
        }

        currentTaskUI.进入由完全溶解变常态背景变黑();

    }


    //添加任务 并且保存
   public void AddTask(TaskSO SO)
    {
        if (_runtimeDict.ContainsKey(SO.TaskID))
        {
            Debug.LogWarning($"任务 {SO.TaskID} 已存在，跳过添加。");
            return;
        }

        ITaskCondition condition = SO.CreateCondition();
        //创建保存数据
        TaskRuntime runtime = new TaskRuntime();

        TaskSaveData saveData = new TaskSaveData();
        saveData.TaskSOID = SO.TaskID;
        saveData.TaskCur = SO.任务默认状态;
        saveData.当前状态 = 任务状态.激活;

        runtime.saveData = saveData;
        runtime.taskSO = SO;

        _runtimeDict.Add(SO.TaskID, runtime);
        _saveData.Add(saveData);
        //TODO:监听事件
        condition.StartListen();

        //condition.StartListen();
    }
    //暂时不考虑删除任务
    void RemoveTask() { }

   public void KillFinishTask(string FinishTaskId)
    {
        //打开UI
        if (UIManager.Instance.OpenPanel(UIManager.UIConst.TaskBox) is TaskUI ui)
        {
            currentTaskUI = ui;
        }
        else
        {
            Debug.LogWarning("未找到" + UIManager.UIConst.TaskBox);
            QuitTaskUI();
            return;
        }
        currentTaskUI.隐藏右侧物体();
        //创建任务选项
        for (int i = 0; i < _saveData.Count; i++)
        {
            if (_runtimeDict.TryGetValue(_saveData[i].TaskSOID, out TaskRuntime runtime))
            {
                currentTaskUI.创建任务实体并添加至列表(runtime.taskSO.TaskName, i);
            }
            //---------------------
            //寻找哪个任务被完成了
            if (_saveData[i].TaskSOID == FinishTaskId)
            {
                OnClickCallBack(i);
            }
        }
        Debug.Log("击杀任务完成");
        //播放音效

        //播放动画
        currentTaskUI.完成杀人任务时候调用动画_背景变黑最后展示红字();
    }
 
  
    

    //显示层回调 点击了哪个任务选项 并且显示任务的进度和内容描述
    public void OnClickCallBack(int id)
    {
        if(_runtimeDict.TryGetValue(_saveData[id].TaskSOID, out TaskRuntime runtime))
        {
            //展示详细内容
            currentTaskUI.显示右侧物体();

            currentTaskUI.设置标题和任务描述(runtime.taskSO.TaskName,runtime.taskSO.TaskDescription);

            currentTaskUI.设置当前进度状态(runtime.saveData.TaskCur);
        }
    }

    public void SaveTaskProcess(任务状态 e,string taskState,string TaskID)
    {
        if (_runtimeDict.TryGetValue(TaskID, out TaskRuntime runtime))
        {
            runtime.saveData.当前状态 = e;
            runtime.saveData.TaskCur=taskState;

            foreach(var t in _saveData)
            {
                if(t.TaskSOID==TaskID)
                {
                    t.当前状态 = e;
                    t.TaskCur=taskState ;
                }
            }

            TaskSaveSystem.Instance.SaveTaskData(_saveData);
        }
    }

    public void QuitTaskUI()
    {
       
        currentTaskUI.起点_退出先黑再关字();
    }

    //动画结束后的回调
    public void closePanelCallBack()
    {
        UIManager.Instance.ClosePanel(UIManager.UIConst.TaskBox, false);
        _running = false;
        // playerInput.SwitchCurrentActionMap("GamePlay");
    }
}
