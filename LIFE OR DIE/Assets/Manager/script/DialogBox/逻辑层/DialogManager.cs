using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIManager;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("当前对话状态")]
    [SerializeField] private DialogueContainer currentContainer;
    [SerializeField] private DiaglugueNode currentNode;
    public bool isInDialogue = false;

    private DialogueUI currentDialogUI;



    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    public void StartDialogue(DialogueContainer needContainer)
    {
        currentContainer = needContainer;
        currentNode=needContainer.strat;
        isInDialogue = true;

        //显示UI 
     if(UIManager.Instance.OpenPanel(UIConst.DialogBox) is DialogueUI ui)
        {
            currentDialogUI = ui;
        }
    }

    public void DisPlayCurrentNode()
    {
        if (currentDialogUI == null) return;
     
        string text = currentNode.text;
        //清空表现层UI选项
        currentDialogUI.ClearOption();
        //将文本传入UI数据层
        currentDialogUI.SetAndStartText(currentNode.speaker, currentNode.text);

        //检查是否有选项 并创建交互
        if (currentNode.options.Count>0)
        { 
       
        for (int i = 0; i < currentNode.options.Count; i++)
        {
                //将节点数据传给UI层
                currentDialogUI.CreateAndSetOption(currentNode.options[i]);
        }
        }
    }


    //哪个选项被选择
    public void OnOptionSelect(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= currentNode.options.Count) return;
        
            DialugueOption selectOption = currentNode.options[optionIndex];
            if (selectOption.nextNode != null)
            {
                //跳转到新的对话节点
                currentNode=selectOption.nextNode;
                DisPlayCurrentNode();
            }
            else
            {
                EndDialog();
            }
        
    }

  
   
    public void EndDialog()
    {
        currentContainer=null;
        currentNode=null;
        isInDialogue = false;
        //通知UI关闭
        UIManager.Instance.ClosePanel(UIConst.DialogBox);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
