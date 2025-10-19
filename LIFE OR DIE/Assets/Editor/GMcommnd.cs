using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UIManager;

public class GMcommnd
{
    [MenuItem("CMCmd/读取表格")]
    public static void ReatTable()
    {
        PackageTable packgeTable = Resources.Load<PackageTable>("TableData/packageTable");
        foreach (PackageTableItem tableItem in packgeTable.DataList)
        {
            Debug.Log(string.Format("[id]={0},[name]={1}", tableItem.id, tableItem.name));
        }
    }

    [MenuItem("CMCmd/打开背包")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIManager.UIConst.BackPack);
    }

    [MenuItem("CMCmd/初始化背包数据")]
    public static void DebugInitPackage()
    {
      PackageInventoryService.Instance.InitPackage();
    }

    [MenuItem("CMCmd/保存当前背包物品")]
    public static void SavePackage()
    {
        PackageInventoryService.Instance.Save();
    }
    [MenuItem("CMCmd/打印当前背包物品")]
    public static void PrintPackage()
    {
        PackageInventoryService.Instance.DebugLogItem();
    }
    [MenuItem("CMCmd/打印Food背包物品")]
    public static void PrintFoodPackage()
    {
        PackageInventoryService.Instance.DebugLogFoodItem();
    }
    [MenuItem("CMCmd/打印Weapon背包物品")]
    public static void PrintWeaponPackage()
    {
        PackageInventoryService.Instance.DebugLogWeaponItem();
    }
    [MenuItem("CMCmd/打印Armor背包物品")]
    public static void PrintArmorPackage()
    {
        PackageInventoryService.Instance.DebugLogArmorItem();
    }
    [MenuItem("CMCmd/为背包增添物品")]
    public static void DebugAddSomeItem()
    {
        Debug.Log("增加物品");
        PackageInventoryService.Instance.AddItem(PackageInventoryService.Instance.GetItemById(1));
        PackageInventoryService.Instance.AddItem(PackageInventoryService.Instance.GetItemById(3));
        PackageInventoryService.Instance.AddItem(PackageInventoryService.Instance.GetItemById(5));
    }
    [MenuItem("CMCmd/调用EquipmentBar脚本中方法强制装备Weapon_1数据")]
    public static void DebugEquipmentWeapon()
    {
        PackageLocalItem temp = new PackageLocalItem();
        temp.id = 1;
        temp.uid = "测试武器";
        temp.count = 1;
        temp.type= itemType.Weapon;
        EquipmentBar.Instance.EquipTheWeapon(temp);
    }


    //此方法仅用来测试
    [MenuItem("CMCmd/输入物品id以为背包增添此物品")]
    public static void DebugAddItemToPackage(int id)
    {
        PackageInventoryService.Instance.AddItem(PackageInventoryService.Instance.GetItemById(id));
    }
    
}
