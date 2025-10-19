using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

//������񵲵��л��� playerControl������л� ��Ϊ������ʱ���л�����
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance {  get { return _instance; } }

    public Image HP;
    private Dictionary<string, string> pathDict;
    //����Ԥ�Ƽ�
    private Dictionary<string, GameObject> perfabDict;
    //�洢�򿪵�Ԥ�Ƽ�
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
        //��ⴰ���Ƿ��
        if(panelDict.TryGetValue(name,out  panel))
        {
            Debug.Log("�����Ѵ�" + panel.name);
            //�Ѿ��򿪸ô��� ����
            return null;
        }
        string path = "";
        //���·���Ƿ�������
        if(!pathDict.TryGetValue(name,out  path))
        {
            Debug.Log("��·��������" + path);
            return null;
        }
        GameObject obj = null;
        //����Ԥ�Ƽ��Ƿ񱻼��ع�
        if (!perfabDict.TryGetValue(name, out  obj))
        {
            string realPath = path;
          GameObject  panelper = Resources.Load<GameObject>(realPath) as GameObject;

            if (panelper == null)
            {
                Debug.LogError($"����Ԥ����ʧ�ܣ�·��: {realPath}");
                return null; // ���ߴ������
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
            Debug.Log("��ǰpanel��δ����"+name);
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
