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
    }

    private void Start()
    {
        InitUIClick();
    }

    private void InitUIClick()
    {
       UIExit.GetComponent<Button>().onClick.AddListener(OnClickExit);
        UISwitchTabWeapon.GetComponent<Button>().onClick.AddListener(OnClickSwitchWeapon);
        UISwitchTabFood.GetComponent<Button>().onClick.AddListener(OnClickSwitchFood);
        UISwitchTabArmor.GetComponent<Button>().onClick.AddListener(OnClickSwitchArmor);
        UIArmorSlot.GetComponent<Button>().onClick.AddListener(OnClickArmorSlot);
        UIWeaponSlot.GetComponent<Button>().onClick.AddListener(OnClickWeaponSlot);

    }
    private void InitUIShow()
    {
        DetailShowParent.gameObject.SetActive(false);
        _Current=PackageCell._current;
        
    }
    private void OnEnable()
    {
        PackageCell.onAnyClicked += OnCellClicked;
    }
    private void OnDisable()
    {
        PackageCell.onAnyClicked -= OnCellClicked;
    }
    public void OnCellClicked(PackageCell cell)
    {
        _Current=cell;
        DetailShowParent.gameObject.SetActive(true);
        DetailShow.InitDetail(cell.ID);
    }
    //
    private void OnClickWeaponSlot()
    {
        throw new NotImplementedException();
       
    }
    private void OnClickArmorSlot()
    {
        throw new NotImplementedException();
       
    }
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
            int a = 0;
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
                    PoolManager.Instance.UIRecycleCell(cell, type);
                    a++;
                }
            }

            Debug.Log("应清空"+ UIPackageContent.childCount + "个" );
            Debug.Log("实际清空" +a + "个");

        }
       
        //添加新东西
        // 添加新东西
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

    private void UpdateUIEquipmentSprite(Image sprite,itemType type)
    {
        
    }


}
