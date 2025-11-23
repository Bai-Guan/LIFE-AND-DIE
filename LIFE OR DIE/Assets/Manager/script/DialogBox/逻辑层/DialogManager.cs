using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UIManager;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("当前对话状态")]
    [SerializeField] private DialogueContainer currentContainer;
    [SerializeField] private DiaglugueNode currentNode;
    public bool isInDialogue = false;
    public bool isWaitingOption;
    private DialogueUI currentDialogUI;
   [SerializeField] private PlayerInput playerInput;

    public event Action OnClick;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(Instance);
            Instance = this;
        }

        playerInput = GameObject.Find("MainPlayer").GetComponent<PlayerInput>();
        // 1. 输入切走 → 角色收不到 Move/Jump
        if (playerInput == null)
        {
            Debug.LogWarning("找不到玩家的PlayerInput");
            this.gameObject.SetActive(false);
            return;
        }
    }

    public void StartDialogue(DialogueContainer needContainer)
    {
        currentContainer = needContainer;
        currentNode = needContainer.GetNode(currentContainer.start);
        isInDialogue = true;

        //显示UI 
     if(UIManager.Instance.OpenPanel(UIConst.DialogBox) is DialogueUI ui)
        {
            currentDialogUI = ui;
            DisPlayCurrentNode();


            playerInput.SwitchCurrentActionMap("Dialog");
            // 2. 订阅 Click（必须在切 Map 之后）
            playerInput.actions["Click"].performed += CompleteTheConversationImmediately;
            // 2. 告诉游戏“我现在处于对话暂停”
            //GameStateManager.PauseCombat(true);    // 敌人停止 AI、禁止伤害
        }
     else
        {
            Debug.LogWarning("这不对啊2.0");
        }
    }

    public void DisPlayCurrentNode()
    {
        if (currentNode == null )
        {
           // print("对话当前未满足条件");
            EndDialog();
            return;
        }
     
        string text = currentNode.text;
        //清空表现层UI选项
        currentDialogUI.ClearOption();
        //将文本传入UI数据层
        bool hasOption = currentNode.options.Count>0?true:false;
        currentDialogUI.SetAndStartText(currentNode.speaker, currentNode.text,hasOption);

        //如果有任务的数据
        if (currentNode.Taskso != null)
        {
            TaskManager.Instance.AddTask(currentNode.Taskso);
            UIManager.Instance.OpenPanel(UIManager.UIConst.addTask);
            TimeManager.Instance.OneTime(4f,
         () =>
         {
             UIManager.Instance.ClosePanel(UIManager.UIConst.addTask, true);
         }
         );
        }

        //检查是否有选项 并创建交互
        if (hasOption)
        { 
       
        for (int i = 0; i < currentNode.options.Count; i++)
        {
                //将节点数据传给UI层
                currentDialogUI.CreateAndSetOption(currentNode.options[i]);
        }
        }
    }
 //用于回调 


    //哪个选项被选择
    //public void OnOptionSelect(int optionIndex)
    //{
    //    if (optionIndex < 0 || optionIndex >= currentNode.options.Count) return;
        
    //        DialugueOption selectOption = currentNode.options[optionIndex];
    //        if (selectOption.nextNode != null)
    //        {
    //            //跳转到新的对话节点
    //            currentNode=selectOption.nextNode;
    //            DisPlayCurrentNode();
    //        }
    //        else
    //        {
    //            EndDialog();
    //        }
        
    //}
    //点击事件
    ////点击事件
    public void CompleteTheConversationImmediately(InputAction.CallbackContext context)
    {
        print("文本点击事件！");
        //CompleteTheConversationImmediately 当前如果还在打字
        if (currentDialogUI.IsTyping)
        {
            currentDialogUI.立刻显示所有内容();
            return;
           
        }
        //如果当前已经打字结束 并且没有选项时候 跳转下一句
        if (currentDialogUI.IsTyping == false && currentNode.options.Count == 0 && currentNode.NextNodeId!=null)
        {
          currentNode=currentContainer.GetNode( currentNode.NextNodeId);
            DisPlayCurrentNode();
            return;
        }
        //如果打字结束 并且 有选项 则执行选项内容
        if (currentDialogUI.IsTyping == false && currentNode.options.Count > 0 && currentNode.NextNodeId == null)
        {
            //这里的方法由OptionClickCallBack的回调调用
            return;
        }
        //如果打字结束 没有选项 没有下一个对话节点
        if(currentDialogUI.IsTyping == false && currentNode.options.Count == 0 && currentNode.NextNodeId == null)
        {
            //再点击则结束对话 
            EndDialog();
        }
    }

    //option选项的点击回调，由表现层调用
    public void OptionClickCallBack(string NODEID)
    {
        currentNode = currentContainer.GetNode(NODEID);
  

        //如果选项没有下一个文本节点 则直接退出
        if(currentNode == null )
        {
            EndDialog();
        }

        //其他逻辑 比如接受任务什么的

        //立刻刷新 播放新对话
        DisPlayCurrentNode();
    }



    public void EndDialog()
    {
        currentContainer=null;
        currentNode=null;
        isInDialogue = false;
        //TODO:切换那个物品的对话数据

        //通知UI关闭
        UIManager.Instance.ClosePanel(UIConst.DialogBox, true);
        //切换操作
        playerInput.actions["Click"].performed -= CompleteTheConversationImmediately;
        playerInput.SwitchCurrentActionMap("GamePlay");
    }


    public void InputSystemOnClick(InputAction.CallbackContext context)
    {
        OnClick?.Invoke();
    }

   
}
