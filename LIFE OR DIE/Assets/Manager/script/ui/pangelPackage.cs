using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class pangelPackage :BasePanel
{
  [SerializeField] private Transform UIExit;

    [SerializeField] private Transform UISwitchTabWeapon;
    private Transform UISwiWeaponIcon;

    [SerializeField] private Transform UISwitchTabFood;
    private Transform UISwiFoodIcon;

    [SerializeField] private Transform UISwitchTabArmor;
    private Transform UISwiArmorIcon;

    [SerializeField] private Transform UIWeaponSlot;
    [SerializeField] private Transform UIArmorSlot;
    
    

    private Transform UIPackageContent;
    private Transform DetailShowParent;

    private PackageDetail DetailShow;
    private PackageCell _Current;

    private Transform UseAndDelete;

    override protected void Awake()
    {
        base.Awake();
        InitUIName();
        
    }
    private void InitUIName()
    {
        UIExit = transform.Find("Exit");

        UISwitchTabArmor = transform.Find("background/SwitchTab/Armor");
        UISwiArmorIcon = UISwitchTabArmor.Find("icon2");

       UISwitchTabFood = transform.Find("background/SwitchTab/Food");
        UISwiFoodIcon = UISwitchTabFood.Find("icon2");

        UISwitchTabWeapon = transform.Find("background/SwitchTab/Weapon");
        UISwiWeaponIcon = UISwitchTabWeapon.Find("icon2");

        UIArmorSlot = transform.Find("show/ArmorSlot");
        UIWeaponSlot = transform.Find("show/WeaponSlot");

        UIPackageContent = transform.Find("centre/Scroll View/Viewport/Content");
        DetailShow=transform.Find("DetailPanel").GetComponent<PackageDetail>();
        DetailShowParent = transform.Find("DetailPanel");

        UseAndDelete = transform.Find("UseAndDelete");
    }

    private void Start()
    {
        InitUIClick();

        PackageInventoryService.Instance.刷新事件 += 刷新背包栏;

    }

    private void InitUIClick()
    {
       UIExit.GetComponent<Button>().onClick.AddListener(OnClickExit);
        UISwitchTabWeapon.GetComponent<Button>().onClick.AddListener(OnClickSwitchWeapon);
        UISwitchTabFood.GetComponent<Button>().onClick.AddListener(OnClickSwitchFood);
        UISwitchTabArmor.GetComponent<Button>().onClick.AddListener(OnClickSwitchArmor);

       


    }
    private void InitSlot()
    {

    }

    private void InitUIShow()
    {
        DetailShowParent.gameObject.SetActive(false);
        UseAndDelete.gameObject.SetActive(false);
        _Current=PackageCell._current;
        
    }
    private void OnEnable()
    {
        PackageCell.onAnyClicked += OnCellClicked;
        EquipmentBar.Instance.WeaponEquipmentEvent += UpdateUIEquipmentSprite;
    }
    private void OnDisable()
    {
        PackageCell.onAnyClicked -= OnCellClicked;
        EquipmentBar.Instance.WeaponEquipmentEvent -= UpdateUIEquipmentSprite;
    }
    //如果有单元格被点击 调用此事件
    bool isAnim = false;
    public void OnCellClicked(PackageCell cell)
    {
        _Current=cell;
        DetailShowParent.gameObject.SetActive(true);
        DetailShow.InitDetail(cell.ID);

        UseAndDelete.gameObject.SetActive(true) ;
        Vector3 srcScale = Vector3.one * 3f;
        Vector3 dstScale = Vector3.one;
        UseAndDelete.localScale = srcScale;
        float timer=0;
        if (isAnim == false)
        {
            isAnim= true;
            TimeManager.Instance.FrameTime(0.8f,
                () =>
                {
                    timer += Time.deltaTime;
                    float t = Mathf.Clamp01(timer / 0.8f);   // 0→1

                    // 先快后慢：Ease-Out
                    t = 1f - Mathf.Pow(1f - t, 3f);   // 也可以 Mathf.SmoothStep(0,1,t)

                    UseAndDelete.localScale = Vector3.LerpUnclamped(srcScale, dstScale, t);
                },
                () =>
                {
                    timer = 0;
                    isAnim = false;
                }
                );
        }
    }
    //

    private void OnClickSwitchWeapon()
    {
        SetAllIconActiveFalse();
        UISwiWeaponIcon.gameObject.SetActive(true);
        SwitchTab(itemType.Weapon);
    }

    private void OnClickSwitchFood()
    {
        SetAllIconActiveFalse();
        UISwiFoodIcon.gameObject.SetActive(true);
        SwitchTab(itemType.Food);
    }
    private void OnClickSwitchArmor()
    {
        SetAllIconActiveFalse();
        UISwiArmorIcon.gameObject.SetActive(true);
        SwitchTab(itemType.Armor);
    }
    //负责显示背包内容
    private void SwitchTab(itemType type)
    {
        
        //如果显示列表有东西 则该列表物品清空
        if(UIPackageContent.childCount > 0)
        {
           // int a = 0;
            // 先收集所有子物体的 Transform
            List<Transform> children = new List<Transform>();//若为c++应该主动销毁
            foreach (Transform child in UIPackageContent)
            {
                children.Add(child);
            }

            // 再处理它们
            foreach (Transform child in children)
            {
                PackageCell cell = child.GetComponent<PackageCell>();
                if (cell != null)
                {
                    //如果当前物品数量小于0 则销毁
                    if(cell.objNum<=0)
                    { PoolManager.Instance.UIListRomove(cell.gameObject,cell._type); }
                    else
                    {
                        PoolManager.Instance.UIRecycleCell(cell, type);
                    }
                    
                  //  a++;
                }
            }

            //Debug.Log("应清空"+ UIPackageContent.childCount + "个" );
            //Debug.Log("实际清空" +a + "个");

        }
       
       
        // 添加新东西 或 转移已有物品
        foreach (var item in PackageInventoryService.Instance.GetDicList(type))
        {
            if (item == null)
            {
                Debug.Log("背包服务层中列表有空数据");
                continue; // 使用 continue 而不是 return，避免中断整个循环
            }

            int id = item.id;
            int objNum = item.count;
            string itemName = item.uid; // 修改变量名，避免与关键字冲突
            Sprite image = PackageInventoryService.Instance._itemDataCache[item.id].itemImage;

            // 直接通过对象池创建
            PoolManager.Instance.UISpanItem(id, objNum, itemName, image, UIPackageContent, type);
        }

    }

    public void 刷新背包栏(itemType type)
    {
        Debug.Log("因为物体增减而刷新背包栏");
        SwitchTab(type);
    }

    


    private void OnClickExit()
    {
        Debug.Log("关闭背包ui");
        DetailShowParent.gameObject.SetActive(false);
        UIManager.Instance.ClosePanel(UIManager.UIConst.BackPack);
    }

   
    private void SetAllIconActiveFalse()
    {
        UISwiArmorIcon.gameObject.SetActive(false);
        UISwiWeaponIcon.gameObject.SetActive(false);
        UISwiFoodIcon.gameObject.SetActive(false);
    }

    //用于跟新数据
    private void UpdateUIEquipmentSprite(itemType type,Sprite sprite)
    {
        switch (type)
        {
            case itemType.Weapon:
                UIWeaponSlot.Find("Weapon").GetComponent<Image>().sprite = sprite;
                break;

            case itemType.Armor:
                   UIArmorSlot.Find("Armor").GetComponent<Image>().sprite = sprite;
                break;
            default:
                break;
        }
    }

    //物品的使用和删除
    public void Use()
    {
        if (PackageCell._current == null) return;
        switch (PackageCell._current._type)
        {
            case itemType.Food:
                Debug.Log("食物使用");
                //实现角色加血效果
                break;

             case itemType.Weapon:
                Debug.Log("剑使用");
                PackageInventoryService.Instance.PackageEquipmentWeapon(PackageCell._current);
                break;

                case itemType.Armor:
                Debug.Log("护甲使用");
                break;
                default:
                Debug.Log("试图使用未知物品");
                break;
        }
      
    }
    public void Delete()
    {
        if (PackageCell._current == null) return;

        PackageLocalItem temp = PackageInventoryService.Instance.由ID得到背包物品的引用(PackageCell._current.ID);
        // temp.count=objNum;
        if(temp==null) return;
       // Debug.Log("删除了物品" + temp.uid);
        PackageInventoryService.Instance.RemoveItem(temp);

    }




    //清理事件
    private void OnDestroy()
    {
        PackageInventoryService.Instance.刷新事件 -= 刷新背包栏;
       
    }


}
