using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueUI : BasePanel
{
    TMPro.TextMeshProUGUI textUI;
    TMPro.TextMeshProUGUI speakerUI;
   [Header("选项的预制件")]
  [SerializeField] private GameObject OptionButton;



  // 不公开，代码里抓  5


    private string showText;
    private Transform ButtonPanel;
    private List<GameObject> currentButtonList;
    private Coroutine m_CurrentCoroutine;
    [SerializeField] private float textSpeed = 0.1f;

    private bool isTyping=false;
    public bool IsTyping {  get { return isTyping; } }
    private bool hasOption;
    protected override void Awake()
    {
        base.Awake();
        InitName();
        currentButtonList = new List<GameObject>();
    }

    

    void OnEnable()          // 面板显示
    {
      
    }

    void OnDisable()         // 面板关闭
    {
   
    }

    private void Start()
    {
        if (textUI == null) this.gameObject.SetActive(false);
        
    }
    void InitName()
    {
        textUI = this.transform.Find("bg/text").GetComponent<TMPro.TextMeshProUGUI>();
        speakerUI = this.transform.Find("bg/speaker").GetComponent<TMPro.TextMeshProUGUI>();
        ButtonPanel = transform.Find("ButtonList");
    }
    public void SetAndStartText(string speaker,string text,bool option)
    {
       // Debug.Log("说话人"+speaker+" 文本:"+ text);

        speakerUI.text = speaker;
        //协程实现打字机效果
        //如果当前还在播放打字 则跳过
        if (m_CurrentCoroutine != null)
        {
            StopCoroutine(m_CurrentCoroutine);
        }
        //将开始时候的文本渲染个数全部关闭
     
        textUI.text = text;
        textUI.ForceMeshUpdate();
        textUI.maxVisibleCharacters = 0;
        isTyping = true;
        hasOption = option;
        m_CurrentCoroutine = StartCoroutine(TypeText(text));
    }


   
    IEnumerator TypeText(string text)
    {
        isTyping = true;
       

        int totalCharacters=textUI.textInfo.characterCount;
        print("应该显示的字数" + totalCharacters);
        for(int i=0;i<=totalCharacters;i++)
        {
            textUI.maxVisibleCharacters = i;
            yield return new WaitForSeconds(textSpeed);
        }


        isTyping = false;
        if (hasOption) ShowAllOption();
        m_CurrentCoroutine = null;
        //设置展示选项
    }
    //展示所以选项
    private void ShowAllOption()
    {
        foreach (GameObject obj in currentButtonList)
        {
            obj.SetActive(true);
        }
    }
    public void ClearOption()
    {
        foreach(GameObject obj in currentButtonList)
        {
            Destroy(obj);
        }
        currentButtonList.Clear();
    }

    public void CreateAndSetOption(DialugueOption option)
    {
     GameObject Buttonoption=   Instantiate(OptionButton);
        Buttonoption.transform.SetParent(ButtonPanel, false);
      TMPro.TextMeshProUGUI  optionT = Buttonoption.transform.Find("optionText").GetComponent<TMPro.TextMeshProUGUI>();
        Image buttonBg=Buttonoption.transform.Find("bg").GetComponent<Image>();
        
        //设置背景图 文本
        if(option.bg!=null) buttonBg.sprite = option.bg;
        optionT.text=option.OptionText;

        //添加到列表
        currentButtonList.Add(Buttonoption);
        //为button设置点击事件 里面传入的方法是调用ID
        Buttonoption.transform.GetComponent<Button>().onClick.AddListener(
            
         () =>  ButtonOnClickCallBackToLogic(option.NextNodeID)
            
            );

        //将选项Hide，等待玩家过完对话
        Buttonoption.SetActive(false);

    }
    public void ButtonOnClickCallBackToLogic(string NextID)
    {
        DialogManager.Instance.OptionClickCallBack(NextID);
    }


    public void 立刻显示所有内容()
    {
        if (m_CurrentCoroutine != null)
        {
            StopCoroutine(m_CurrentCoroutine);
        }

        m_CurrentCoroutine = null;
        isTyping = false;


        //设置字全显示
        textUI.maxVisibleCharacters = textUI.textInfo.characterCount;
        if(hasOption)
            ShowAllOption();

    }

}

