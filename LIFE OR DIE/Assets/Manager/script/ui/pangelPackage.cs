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
    //������ʾ��������
    private void SwitchTab(itemType type)
    {
        
        //�����ʾ�б��ж��� ����б���Ʒ���
        if(UIPackageContent.childCount > 0)
        {
            int a = 0;
            // ���ռ������������ Transform
            List<Transform> children = new List<Transform>();//��Ϊc++Ӧ����������
            foreach (Transform child in UIPackageContent)
            {
                children.Add(child);
            }

            // �ٴ�������
            foreach (Transform child in children)
            {
                PackageCell cell = child.GetComponent<PackageCell>();
                if (cell != null)
                {
                    PoolManager.Instance.UIRecycleCell(cell, type);
                    a++;
                }
            }

            Debug.Log("Ӧ���"+ UIPackageContent.childCount + "��" );
            Debug.Log("ʵ�����" +a + "��");

        }
       
        //����¶���
        // ����¶���
        foreach (var item in PackageInventoryService.Instance.GetDicList(type))
        {
            if (item == null)
            {
                Debug.Log("������������б��п�����");
                continue; // ʹ�� continue ������ return�������ж�����ѭ��
            }

            int id = item.id;
            int objNum = item.count;
            string itemName = item.uid; // �޸ı�������������ؼ��ֳ�ͻ
            Sprite image = PackageInventoryService.Instance._itemDataCache[item.id].itemImage;

            // ֱ��ͨ������ش���
            PoolManager.Instance.UISpanItem(id, objNum, itemName, image, UIPackageContent, type);
        }

    }

    


    private void OnClickExit()
    {
        Debug.Log("�رձ���ui");
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
