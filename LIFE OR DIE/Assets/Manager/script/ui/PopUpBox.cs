using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UIManager;

public class PopUpBox : BasePanel
{


    private float Width;
    private float Height;
    private float WorldHeight;
   [SerializeField] private RectTransform rectTransform;
    TMPro.TextMeshProUGUI KeyVaule;
    TMPro.TextMeshProUGUI Description;

    public const string 交互 = "Use";
    public const string 拾取 = "Pick Up";
    public const string 对话 = "Dialogue";

    private ObjShowUI _inside;
    enum 移动模式
    {
        等待,
        平移,
        出现,
    }

    private 移动模式 当前模式=移动模式.等待;

    private Vector2 firstPosition;
    private Vector2 targetPosition; 
    float timer = 0;
    const float needTime = 0.2f;
    const float needMoveY = 150f;
    //场上只能有一个谈话框 
    //如果场上没有谈话框 就冒出一个 
    //如果场上没有谈话框 就把谈话框平移到他那里
    //离开时候关闭谈话框

    //谁调用移动函数？

    protected override void Awake()
    {
        base.Awake();
        InitName();
        InitData();
        StackInteraction.Instance.ActionPush += Push;
        StackInteraction.Instance.ActionPop += Pop;
    }
    private void InitName()
    {
        KeyVaule = this.transform.Find("Key/KeyValue").GetComponent<TextMeshProUGUI>();
        Description=this.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        this.rectTransform=this.transform.GetComponent<RectTransform>();
    }
    private void InitData()
    {
        Width = this.transform.GetComponent<RectTransform>().rect.width;
        Height= this.transform.GetComponent<RectTransform>().rect.height;
     
    }

    public void WakeUp()
    {
        //StackInteraction.Instance.ActionPush += Push;
        //StackInteraction.Instance.ActionPop += Pop;
    }


    public void Push(GameObject objShow)
    {
     _inside= objShow.GetComponent<ObjShowUI>();
        if (_inside == null) { Debug.LogWarning(objShow.name+"缺少UI展示组件"); return; }

        // 更新文本
        KeyVaule.text = _inside.KeyValue;
        Description.text = _inside.Discription;

        // 直接计算屏幕位置
        Vector3 worldPos = _inside.transform.position;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // 添加偏移
        float pixelOffset = -needMoveY; // 像素偏移量，根据需要调整
        screenPos.y += pixelOffset;

        // 转换为UI本地坐标
        Canvas canvas = UIManager.Instance.uiRoot?.GetComponent<Canvas>();
        if (canvas == null) return;

        RectTransform canvasRect = canvas.transform as RectTransform;
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            uiCamera,
            out Vector2 localPoint
        ))
        {
            targetPosition = localPoint;
            ////TODO:动画效果
            //如果栈的个数只有一个，则渐入 如果不只一个，则平移
            if (StackInteraction.Instance.CurrentNum == 1)
            {
                GraduallyEnter();
            }
            else
            {
                Translation();
            }
        }
    }

    private void GraduallyEnter()
    {
        timer = 0;
        当前模式 = 移动模式.出现;
    }
    private void Translation()
    {
     

        timer = 0;
        rectTransform.localScale = new Vector3(1, 1, 1);
        firstPosition = rectTransform.localPosition;
        当前模式 = 移动模式.平移;
    }
    public void Pop(int currentNum,GameObject currentTop,bool isPopTop)
    {
        if(currentTop == null||currentNum<=0)
        {
            UIManager.Instance.ClosePanel(UIConst.PopUpBox);
            return;
        }
        _inside = currentTop.GetComponent<ObjShowUI>();
        if (_inside == null) { Debug.LogWarning(currentTop.name + "缺少UI展示组件"); return; }

        if (isPopTop) {
            //出栈是栈顶 需要移动 
            if (StackInteraction.Instance.CurrentNum > 0)
            {
             

                // 更新文本
                KeyVaule.text = _inside.KeyValue;
                Description.text = _inside.Discription;

                // 直接计算屏幕位置
                Vector3 worldPos = _inside.transform.position;
                Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

                // 添加偏移
                float pixelOffset = -needMoveY; // 像素偏移量，根据需要调整
                screenPos.y += pixelOffset;

                // 转换为UI本地坐标
                Canvas canvas = UIManager.Instance.uiRoot?.GetComponent<Canvas>();
                if (canvas == null) return ;

                RectTransform canvasRect = canvas.transform as RectTransform;
                Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPos,
                    uiCamera,
                    out Vector2 localPoint
                ))
                {
                    targetPosition = localPoint;
                    Translation();
                }

            }
        }
        else
        {

        }      
  }

    private void Update()
    {
        // 面板已被销毁，立即退出
        if (rectTransform == null) return;

        switch (当前模式)
        {
            case 移动模式.等待:

                break;
            case 移动模式.平移:
                timer += Time.deltaTime;
                float how = timer / needTime;
                Vector2 temp;
                temp.x = Mathf.Lerp(firstPosition.x, targetPosition.x, how);
                temp.y = Mathf.Lerp(firstPosition.y, targetPosition.y, how);
                rectTransform.localPosition = temp;
                if(timer>needTime)
                {
                    timer = 0;
                    当前模式 = 移动模式.等待;
                }
                break;

            case 移动模式.出现:
                //先从小到大 再从大到小 缩放由0.01到3到1
                timer += Time.deltaTime;
                
                rectTransform.localPosition = targetPosition;

                if(timer<needTime)
                {
                    float how2 = timer / needTime;
                      float temp2 = Mathf.Lerp(0.01f, 1f, how2);
                      Vector3 scale =new Vector3( temp2, temp2, temp2);
                      rectTransform.localScale=scale;
                }
                else
                {
                    timer = 0;
                    当前模式 = 移动模式.等待;
                }
                break;

        }
    }
    private void OnEnable()
    {
    
    }
    private void OnDestroy()
    {
        StackInteraction.Instance.ActionPush -= Push;
        StackInteraction.Instance.ActionPop -= Pop;
    }
    private void OnDisable()
    {
        StackInteraction.Instance.ActionPush -= Push;
        StackInteraction.Instance.ActionPop -= Pop;
    }
    //设置坐标
}
