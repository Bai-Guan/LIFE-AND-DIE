using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : BasePanel
{
    [SerializeField] UnityEngine.UI.Image 最内层背景着色器;
    [SerializeField] UnityEngine.UI.Image 公共背景着色器;
    [SerializeField] UnityEngine.UI.Image 分割线;
    [SerializeField] TMPro.TextMeshProUGUI 待办的执念;
    [SerializeField] TMPro.TextMeshProUGUI 任务标题;
    [SerializeField] TMPro.TextMeshProUGUI 任务描述;
    [SerializeField] TMPro.TextMeshProUGUI 定死的任务状态文本;
    [SerializeField] TMPro.TextMeshProUGUI 当前状态;

    [SerializeField] GameObject 任务选项预制体;

    [SerializeField] GameObject 右侧标题物体;
    [SerializeField] GameObject 右侧内容物体;
    [SerializeField] GameObject 任务大背景框;
    [SerializeField] GameObject 分界线物体;
    [SerializeField] GameObject 执念物体;


    private List<GameObject> 任务选项列表;
    private Transform 滚动列表;

   [SerializeField] private Material 背景材质;
    [SerializeField] private Material 框材质;

   private bool 是否在脚本动画;
    public bool isAnim { get { return 是否在脚本动画; } }
    private const float 显示时长 = 1f;


    private Gradient gradient;
    private const float 消失变黑时长 = 1.5f;
    private const float 消失溶解时长 = 1f;

    private const float 杀人变黑时长 = 1.2f;
    protected override void Awake()
    {
        base.Awake();
        背景材质 = 最内层背景着色器.material;
        框材质 = 公共背景着色器.material;
        任务选项列表 = new List<GameObject>();
 
        GradientColorKey[] colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(Color.white, 0.0f),      // 白
            new GradientColorKey(new Color(1, 0.8f, 0.8f), 0.2f), // 浅红
            new GradientColorKey(new Color(0.8f, 0.1f, 0.1f), 0.5f), // 血红
            new GradientColorKey(new Color(0.3f, 0, 0), 0.8f),      // 深红
            new GradientColorKey(Color.black, 1.0f)                   // 黑
        };

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
        {
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        };
      
        gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);

        滚动列表 = this.transform.Find("TaskList/滚动视图/Viewport/Content");
    }
    public void 隐藏右侧物体()
    {
        右侧标题物体.SetActive(false);
        右侧内容物体.SetActive(false);
    }
    public void 显示右侧物体()
    {
        右侧标题物体.SetActive(true);
        右侧内容物体.SetActive(true);
    }

    public void 关闭所有物件()
    {
        右侧标题物体.SetActive(false);
        右侧内容物体.SetActive(false);
        任务大背景框.SetActive(false );
        分界线物体.SetActive(false);
        执念物体.SetActive(false); ;

        foreach(var obj in 任务选项列表)
        {
            obj.SetActive(false);   
        }

    }
    public void 启动所有物件()
    {
        右侧标题物体.SetActive(true);
        右侧内容物体.SetActive(true);
        任务大背景框.SetActive(true);
        分界线物体.SetActive(true);
        执念物体.SetActive(true); ;

        foreach (var obj in 任务选项列表)
        {
            obj.SetActive(true);
        }

    }

    public void 设置所有字体全开()
    {
        待办的执念.enabled = true;
        任务标题.enabled=true;
        任务描述.enabled=true;
        定死的任务状态文本.enabled = true;
        当前状态.enabled = true;
        分割线.enabled = true;

    }
    public void 设置所有字体全关()
    {
        待办的执念.enabled = false;
        任务标题.enabled = false;
        任务描述.enabled = false;
        定死的任务状态文本.enabled = false;
        当前状态.enabled = false;
        分割线.enabled = false;
    }


    public void 设置标题和任务描述(string 标题,string 描述)
    {
        任务标题.text = 标题;
        任务描述.text = 描述;
    }

    public void 设置当前进度状态(string state)
    {
        当前状态.text=state;
    }

    //---------------回调逻辑---------------

    //创建任务  TODO:动态数据要保存 逻辑区
    public void 创建任务实体并添加至列表(string name,int id)
    {
      GameObject temp=  Instantiate(任务选项预制体);
        temp.transform.SetParent(滚动列表);

    TMPro.TextMeshProUGUI text = temp.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.text = name;

        while (任务选项列表.Count <= id) 任务选项列表.Add(null);
        任务选项列表[id]=temp;

        Button button = temp.transform.GetComponent<Button>();

        button.onClick.AddListener(
            () =>
            {
                OnTaskButtonClickedCallBack(id);
            }
            );

    }

    //点击时候 
    void OnTaskButtonClickedCallBack(int id)
    {
        TaskManager.Instance.OnClickCallBack(id);
    }



    public void ShowDetail(string name,string content,string state)
    {
        任务标题.text = name;
        任务描述.text = content;
         当前状态.text = state;
    }









    //---------------动画设置-------------------------------
    public void 进入由完全溶解变常态背景变黑()
    {
        if(是否在脚本动画==true)return;

        //TODO:看看逻辑层要不要切换输入状态

        //描边颜色 0.0817 0 0   背景透明度到0.97
        设置所有字体全关();
        是否在脚本动画 = true;
        //ui出现后 先设置溶解度为完全溶解 再设置最后背景为完全透明
        Color t = new Color(0.0817f,0,0,0);
      背景材质.SetColor("Outline Color", t);


        框材质.SetFloat("_AlphaClip", 1);
        //描边颜色
        框材质.SetColor("_EmissionColor",new Color(0.11f,0,0));
        //整体颜色
        框材质.SetColor("_Color", new Color(1,1,1));
        //描边宽度
        框材质.SetFloat("_LightWeight",0.02f);

        float timer = 0;
        //计时器
        float scale;
        TimeManager.Instance.FrameTime(显示时长,
            () =>
            {
                timer += Time.deltaTime;
                scale = timer / 显示时长;

               float 透明度= Mathf.Lerp(0,0.97f,scale);
                Color temp = new Color(0.0817f, 0, 0, 透明度);
                float 溶解度 = Mathf.Lerp(1, 0.02f,scale);

                背景材质.SetColor("_OutlineColor", temp);
                框材质.SetFloat("_AlphaClip", 溶解度);

                if (scale > 0.5) 设置所有字体全开();
            },
            //回调 设置
            ()=>
            {
                是否在脚本动画 = false;
             }
            );
        
    }

    public void 起点_退出先黑再关字()
    {
        if (是否在脚本动画 == true) return;

    
        背景材质.SetColor("_OutlineColor", new Color(0.0817f, 0, 0, 0.97f));
        框材质.SetFloat("_AlphaClip", 0.02f);

       
        //描边颜色
        框材质.SetColor("_EmissionColor", new Color(0.11f, 0, 0));
        //整体颜色
        框材质.SetColor("_Color", new Color(1, 1, 1));
        //描边宽度
        框材质.SetFloat("_LightWeight", 0.02f);



        是否在脚本动画 = true;
        float timer = 0;
        //计时器
        float scale;
        TimeManager.Instance.FrameTime(消失变黑时长+0.5f,
            () =>
            {
                timer += Time.deltaTime;
                scale = timer / 消失变黑时长;

             Color c=   gradient.Evaluate(scale);

              
                框材质.SetColor("_Color", c);

                
            },
            //回调 设置
            () =>
            {
               
                终点_最后溶解背景恢复常亮();
            }
            );
    }
    private void 终点_最后溶解背景恢复常亮()
    {
        //描边颜色
        框材质.SetColor("_EmissionColor", new Color(0.39f, 0.006f, 0.006f));
        //描边宽度
        框材质.SetFloat("_LightWeight", 0.02f);

     
        //计时器
        float scale;
        float timer = 0;
        TimeManager.Instance.FrameTime(消失溶解时长,
        () =>
        {
            timer += Time.deltaTime;
            scale = timer / 消失溶解时长;

            float 透明度 = Mathf.Lerp(0.97f, 0f, scale);
            Color temp = new Color(0.0817f, 0, 0, 透明度);
            float 溶解度 = Mathf.Lerp(0.02f, 1f, scale);

            背景材质.SetColor("_OutlineColor", temp);
            框材质.SetFloat("_AlphaClip", 溶解度);

            if(scale>0.5f) 设置所有字体全关();




        },
        //回调 设置
        () =>
        {
            是否在脚本动画 = false;
            TaskManager.Instance.closePanelCallBack();
        }
        );

    }

   public void 完成杀人任务时候调用动画_背景变黑最后展示红字()
    {
        if (是否在脚本动画 == true) return;
        是否在脚本动画 = true;

        Color t = new Color(0.0817f, 0, 0, 0);
        背景材质.SetColor("Outline Color", t);

        //描边颜色
        框材质.SetColor("_EmissionColor", new Color(1f, 0, 0));
        //整体颜色
        框材质.SetColor("_Color", new Color(0, 0, 0));
        //描边宽度
        框材质.SetFloat("_LightWeight", 0f);
        //调溶解度
        框材质.SetFloat("_AlphaClip", 0.02f);

        //关闭
        关闭所有物件();
        设置所有字体全关();
        float timer = 0;
        //计时器
        float scale;
        TimeManager.Instance.FrameTime(杀人变黑时长 + 0.3f,

            () =>
            {
                timer += Time.deltaTime;
                scale = timer / 杀人变黑时长;
                float 透明度 = Mathf.Lerp(0, 1f, scale);
                Color temp = new Color(0.0817f, 0, 0, 透明度);

                背景材质.SetColor("_OutlineColor", temp);
            },
            () =>
            {
                //显示红字
                启动所有物件();
                设置所有字体全开();
                //播放音效

                //等待0.5f
                TimeManager.Instance.OneTime(1.3f,
                    () =>
                    {
                        完成杀人任务时候调用动画_屏幕切红();
                    }
                    );
            }
            );
    }
    private void 完成杀人任务时候调用动画_屏幕切红()
    {
        float timer = 0;
        //计时器
        float scale;
        TimeManager.Instance.FrameTime(1.5f,

            () =>
            {
                timer += Time.deltaTime;
                scale = timer / 1.5f;
                float weight = Mathf.Lerp(0, 1f, scale);

                //描边宽度
                框材质.SetFloat("_LightWeight", weight);

            },
            () =>
            {
                完成杀人任务时候调用动画_恢复正常();
            }

            );

    }
    
    private void 完成杀人任务时候调用动画_恢复正常()
    {
        float timer = 0;
        //计时器
        float scale;
        TimeManager.Instance.FrameTime(1.5f,

            () =>
            {
                timer += Time.deltaTime;
                scale = timer / 1.5f;
                float weight = Mathf.Lerp(1, 0f, scale);
                float color = 1 - weight;
                float 透明度=Mathf.Lerp(1, 0.97f, scale);
                //描边宽度
                框材质.SetFloat("_LightWeight", weight);
                //整体颜色
                框材质.SetColor("_Color", new Color(color, color, color));

                背景材质.SetColor("_OutlineColor", new Color(0.0817f, 0, 0, 透明度));
            },
            () =>
            {
                是否在脚本动画 = false;
            }
            );
    }



}
