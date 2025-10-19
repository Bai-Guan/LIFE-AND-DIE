using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

//受伤与格挡的切换在 playerControl类进行切换 因为属于随时可切换类型
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance {  get { return _instance; } }

    public Image HP;
    private Dictionary<string, string> pathDict;
    //缓存预制件
    private Dictionary<string, GameObject> perfabDict;
    //存储打开的预制件
    private Dictionary<string, BasePanel> panelDict;
    private Transform _uiRoot;
    public Transform uiRoot
    {
        get
        {
            if(_uiRoot == null)
            {
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }

    UIManager() 
    {
    InitDicts();
    }
    void InitDicts()
    {
        perfabDict = new Dictionary<string,GameObject >();
        panelDict = new Dictionary<string, BasePanel>();

        pathDict = new Dictionary<string, string>()
        {
            {UIConst.BackPack,"uiPrefab/BackPack" },
            {UIConst.TreasureChest,"uiPrefab/TreasureChest" }
           
        };
    }


    private void Awake()
    {
        if(_instance != null&&_instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

   public class UIConst
    {
        public const string BackPack = "BackPack";

        public const string TreasureChest = "TreasureChest";
    }

    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        //检测窗口是否打开
        if(panelDict.TryGetValue(name,out  panel))
        {
            Debug.Log("窗口已打开" + panel.name);
            //已经打开该窗口 返回
            return null;
        }
        string path = "";
        //检测路径是否有配置
        if(!pathDict.TryGetValue(name,out  path))
        {
            Debug.Log("该路径不存在" + path);
            return null;
        }
        GameObject obj = null;
        //检测该预制件是否被加载过
        if (!perfabDict.TryGetValue(name, out  obj))
        {
            string realPath = path;
          GameObject  panelper = Resources.Load<GameObject>(realPath) as GameObject;

            if (panelper == null)
            {
                Debug.LogError($"加载预制体失败，路径: {realPath}");
                return null; // 或者处理错误
            }

            perfabDict.Add(name, panelper);
            obj = panelper;
        }

        GameObject prefebPanel = GameObject.Instantiate(obj,uiRoot,false);
        BasePanel bp = prefebPanel.GetComponent<BasePanel>();

        panelDict.Add(name, bp);
        return bp;
    }
    
    public bool ClosePanel(string name)
    {
        BasePanel basePanel = null;
        if(!panelDict.TryGetValue(name,out basePanel))
        {
            Debug.Log("当前panel并未被打开"+name);
            return false;
        }
        panelDict.Remove(name);
        basePanel.ClosePanel(name);
        return true;
    }
    public void ChangeHPUI(int hp,int MAXhp)
    {
        if(hp<=MAXhp) 
        HP.fillAmount =(float) hp/MAXhp;
        else if(hp>MAXhp)HP.fillAmount = 1;
    }
}
