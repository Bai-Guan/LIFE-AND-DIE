using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PackageCell : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
   [SerializeField] Transform select1;
    [SerializeField] Transform select2;
    [SerializeField] Transform obj;
    [SerializeField] Transform numBG;
    int _id;
    public int ID {  get { return _id; } }
   public int objNum;
    string _name;
    public string Name {  get { return _name; } }
   public itemType _type;

  //  public PackageLocalItem myData;
    [SerializeField] TextMeshProUGUI _Textname;
    [SerializeField] TextMeshProUGUI _Textnum;

    Transform TranClick;
    Transform TranPass;

    Animator AnimClick;
    Animator AnimPass;


    Transform NowMenu;
     float MoveDistance ;
    const float MoveTime = 0.8f;
    Vector2 FirstMenuPos; // 改为存储初始位置
    RectTransform NowMenuRect; // 添加RectTransform引用

    public static Action<PackageCell> onAnyClicked;
    public static PackageCell _current = null;
    private void Awake()
    {
       
        InitName();

        // 订阅点击事件，当其他单元格被点击时关闭菜单
        onAnyClicked += OnAnyCellClicked;
        PackageInventoryService.Instance.Packagecell数字刷新 += CurrentCellObjNumDecrementByOne;

    }
    private void Start()
    {
        MoveDistance=transform.Find("menu/bg").GetComponent<RectTransform>().rect.width;
    }
    private void showData()
    {
        _Textname.text= _name;
        _Textnum.text= objNum.ToString();
        if(objNum>1)
        {
            numBG.gameObject.SetActive(true);
        }
    }

    public  void CurrentCellObjNumDecrementByOne(int num)
    {
        if (_current != this) return;
        objNum = num;
        Debug.Log("当前操作的对象是：" + name + "，objNum = " + objNum);
     
      _Textnum.text= objNum.ToString();

        //if (objNum <= 0)
        //{
        //    Destroy(gameObject);
        //    _current = null;
        //}
    }
    private void InitName()
    {
        select1 = transform.Find("select1");
        select2 = transform.Find("select2");
        obj = transform.Find("object");
        _Textname = transform.Find("name").GetComponent<TextMeshProUGUI>();
        _Textnum = transform.Find("numBG/num").GetComponent<TextMeshProUGUI>();
        numBG = transform.Find("numBG");

        TranClick = transform.Find("Anim/click");
        TranPass =  transform.Find("Anim/show");

        AnimClick = transform.Find("Anim/click").GetComponent<Animator>();
        AnimPass = transform.Find("Anim/show").GetComponent<Animator>();

        NowMenu = transform.Find("menu");
        NowMenuRect = NowMenu.GetComponent<RectTransform>(); // 获取RectTransform
        FirstMenuPos = NowMenuRect.anchoredPosition; // 存储初始anchoredPosition


    }
    private void OnAnyCellClicked(PackageCell clickedCell)
    {
        // 如果点击的不是当前单元格，则关闭菜单
        if (clickedCell != this)
        {
            CloseMenu();
        }
    }
    private void InitSet()
    {
        TranClick.gameObject.SetActive(false);
        TranPass.gameObject.SetActive(false);
        select1.gameObject.SetActive(false);
        select2.gameObject.SetActive(false);

        // 确保菜单回到初始位置
        NowMenuRect.anchoredPosition = FirstMenuPos;
    }

    public void Set(int id,int num,string name,Sprite sprite,itemType type)//PackageLocalItem pli)
    {
       // myData = pli;
        _id = id;
        objNum = num;
        _name = name;
        _type = type;
        if (obj == null) { Debug.LogError("OBJ为空！");return;}
        obj.GetComponent<Image>().sprite= sprite;
        _Textnum.text = objNum.ToString();
        showData();
    }

    public void Set(PackageCell cell)
    {
        _id = cell._id;
        objNum =cell.objNum;
        _name = cell._name;
        obj.GetComponent<Image>().sprite = cell.obj.GetComponent<Image>().sprite;
        showData();
    }

 


    // 添加关闭菜单的方法
    public void CloseMenu()
    {
        if (NowMenu.gameObject.activeSelf)
        {
            float timer = 0;
            Vector2 from = NowMenuRect.anchoredPosition;
            Vector2 to = FirstMenuPos;

            TimeManager.Instance.FrameTime(MoveTime,
                () =>
                {
                    timer += Time.deltaTime;
                    float t = Mathf.Clamp01(timer / MoveTime);
                    float s = 1f - (1f - t) * (1f - t);
                    NowMenuRect.anchoredPosition = Vector2.Lerp(from, to, s);
                },
                () =>
                {
                    NowMenu.gameObject.SetActive(false);
                }
            );
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _current = this;
        onAnyClicked?.Invoke(this);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("OnPointClick" + eventData.ToString());
            AnimClick.SetTrigger("Click");
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 如果菜单已经打开，则关闭它
            if (NowMenu.gameObject.activeSelf)
            {
                CloseMenu();
            }
            else
            {
                // 否则打开菜单
                float timer = 0;
                Vector2 from = NowMenuRect.anchoredPosition;
                bool toRight = (_current == this);
                Vector2 to = FirstMenuPos + (toRight ? Vector2.right * MoveDistance : Vector2.left * MoveDistance);

                NowMenu.gameObject.SetActive(true);

                TimeManager.Instance.FrameTime(MoveTime,
                    () =>
                    {
                        timer += Time.deltaTime;
                        float t = Mathf.Clamp01(timer / MoveTime);
                        float s = 1f - (1f - t) * (1f - t);
                        NowMenuRect.anchoredPosition = Vector2.Lerp(from, to, s);
                    }
                );
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointEnter" + eventData.ToString());

        TranClick.gameObject.SetActive(true);
        TranPass.gameObject.SetActive(true);

        AnimPass.SetTrigger("In");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointExit" + eventData.ToString());

        
        TimeManager.Instance.OneTime(0.2f,
            () =>
            {
                TranClick.gameObject.SetActive(false);
                TranPass.gameObject.SetActive(false);
            }
            );
        

        AnimPass.SetTrigger("Out");
    }

    private void OnDestroy()
    {
        // 取消订阅，防止内存泄漏
        onAnyClicked -= OnAnyCellClicked;
        PackageInventoryService.Instance.Packagecell数字刷新 -= CurrentCellObjNumDecrementByOne;
    }
}
