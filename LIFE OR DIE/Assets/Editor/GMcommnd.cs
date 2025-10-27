using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UIManager;

public class GMcommnd
{
    [MenuItem("CMCmd/��ȡ���")]
    public static void ReatTable()
    {
        PackageTable packgeTable = Resources.Load<PackageTable>("TableData/packageTable");
        foreach (PackageTableItem tableItem in packgeTable.DataList)
        {
            Debug.Log(string.Format("[id]={0},[name]={1}", tableItem.id, tableItem.name));
        }
    }

    [MenuItem("CMCmd/�򿪱���")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIManager.UIConst.BackPack);
    }

    [MenuItem("CMCmd/��ʼ����������")]
    public static void DebugInitPackage()
    {
      PackageInventoryService.Instance.InitPackage();
    }

    [MenuItem("CMCmd/���浱ǰ������Ʒ")]
    public static void SavePackage()
    {
        PackageInventoryService.Instance.Save();
    }
    [MenuItem("CMCmd/��ӡ��ǰ������Ʒ")]
    public static void PrintPackage()
    {
        PackageInventoryService.Instance.DebugLogItem();
    }
    [MenuItem("CMCmd/��ӡFood������Ʒ")]
    public static void PrintFoodPackage()
    {
        PackageInventoryService.Instance.DebugLogFoodItem();
    }
    [MenuItem("CMCmd/��ӡWeapon������Ʒ")]
    public static void PrintWeaponPackage()
    {
        PackageInventoryService.Instance.DebugLogWeaponItem();
    }
    [MenuItem("CMCmd/��ӡArmor������Ʒ")]
    public static void PrintArmorPackage()
    {
        PackageInventoryService.Instance.DebugLogArmorItem();
    }
    [MenuItem("CMCmd/Ϊ����������Ʒ")]
    public static void DebugAddSomeItem()
    {
        Debug.Log("������Ʒ");
        PackageInventoryService.Instance.AddItem(PackageInventoryService.Instance.GetNewItemById(1));
        PackageInventoryService.Instance.AddItem(PackageInventoryService.Instance.GetNewItemById(3));
        PackageInventoryService.Instance.AddItem(PackageInventoryService.Instance.GetNewItemById(5));
    }
    [MenuItem("CMCmd/Debug��ȥIDΪ1����Ʒ")]
    public static void Debug��ȥIDΪ1����Ʒ()
    {
        PackageLocalItem temp = PackageInventoryService.Instance.��ID�õ�������Ʒ������(1);
        //temp.count = 1;
        Debug.Log("ɾ������Ʒ" + temp.uid);
        PackageInventoryService.Instance.RemoveItem(temp);
        //����ˢ��
    }

    [MenuItem("CMCmd/����EquipmentBar�ű��з���ǿ��װ��Weapon_1����")]
    public static void DebugEquipmentWeapon()
    {
        PackageLocalItem temp = new PackageLocalItem();
        temp.id = 1;
        temp.uid = "��������";
        temp.count = 1;
        temp.type= itemType.Weapon;
        EquipmentBar.Instance.EquipTheWeapon(temp);
    }
    [MenuItem("CMCmd/����EquipmentBar�ű��з���ǿ��װ��Weapon_2����")]
    public static void DebugEquipmentWeapon2()
    {
        PackageLocalItem temp = new PackageLocalItem();
        temp.id = 2;
        temp.uid = "��������2";
        temp.count = 1;
        temp.type = itemType.Weapon;
        EquipmentBar.Instance.EquipTheWeapon(temp);
    }


    ////�˷�������������
    //[MenuItem("CMCmd/������Ʒid��Ϊ�����������Ʒ")]
    //public static void DebugAddItemToPackage(int id)
    //{
    //    PackageInventoryService.Instance.AddItem(PackageInventoryService.Instance.GetItemById(id));
    //}
    
}
