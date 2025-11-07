using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : BasePanel
{
    TMPro.TextMeshProUGUI textUI;
    TMPro.TextMeshProUGUI speakerUI;
   [Header("选项的预制件")]
  [SerializeField] private GameObject OptionButton;

    private string showText;
    private Transform ButtonPanel;
    private List<GameObject> currentButtonList;
    private Coroutine m_CurrentCoroutine;
    [SerializeField] private float textSpeed = 0.1f;

    private bool isTyping=false;
    private bool isTypingFinish = false;
    protected override void Awake()
    {
        base.Awake();
        InitName();
        currentButtonList = new List<GameObject>();
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
    public void SetAndStartText(string speaker,string text)
    {
        speakerUI.text = speaker;
        //协程实现打字机效果
        //如果当前还在播放打字 则跳过
        if (m_CurrentCoroutine != null)
        {
            StopCoroutine(m_CurrentCoroutine);
        }
        //将开始时候的文本渲染个数全部关闭
        textUI.maxVisibleCharacters = 0;
        textUI.text = text;
        isTyping = true;
        isTypingFinish = false;
        m_CurrentCoroutine = StartCoroutine(TypeText(text));
    }
    public void CompleteTheConversationImmediately()
    {
        if (m_CurrentCoroutine != null)
        {
            StopCoroutine(m_CurrentCoroutine);
        }
        else
        {
            return;
        }
        m_CurrentCoroutine = null;
        isTyping = false;
        isTypingFinish = true;

        //设置字全显示
        textUI.maxVisibleCharacters= textUI.textInfo.characterCount;

    }

    IEnumerator TypeText(string text)
    {
        int totalCharacters=textUI.textInfo.characterCount;
        for(int i=0;i<=totalCharacters;i++)
        {
            textUI.maxVisibleCharacters = i;
            yield return new WaitForSeconds(textSpeed);
        }


        isTyping = false;
        isTypingFinish = true;
        m_CurrentCoroutine = null;
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
      TMPro.TextMeshProUGUI  optionT = Buttonoption.GetComponent<TMPro.TextMeshProUGUI>();
        Image buttonBg=Buttonoption.GetComponent<Image>();
        
        //设置背景图 文本
        if(option.bg!=null) buttonBg.sprite = option.bg;
        optionT.text=option.OptionText;

        //添加到列表
        currentButtonList.Add(Buttonoption);


        //将选项Hide，等待玩家过完对话
        Buttonoption.SetActive(false);

    }

}

