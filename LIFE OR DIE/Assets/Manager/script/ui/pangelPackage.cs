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

        PackageInventoryService.Instance.ˢ���¼� += ˢ�±�����;

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
    //����е�Ԫ�񱻵�� ���ô��¼�
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
                    float t = Mathf.Clamp01(timer / 0.8f);   // 0��1

                    // �ȿ������Ease-Out
                    t = 1f - Mathf.Pow(1f - t, 3f);   // Ҳ���� Mathf.SmoothStep(0,1,t)

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
    //������ʾ��������
    private void SwitchTab(itemType type)
    {
        
        //�����ʾ�б��ж��� ����б���Ʒ���
        if(UIPackageContent.childCount > 0)
        {
           // int a = 0;
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
                    //�����ǰ��Ʒ����С��0 ������
                    if(cell.objNum<=0)
                    { PoolManager.Instance.UIListRomove(cell.gameObject,cell._type); }
                    else
                    {
                        PoolManager.Instance.UIRecycleCell(cell, type);
                    }
                    
                  //  a++;
                }
            }

            //Debug.Log("Ӧ���"+ UIPackageContent.childCount + "��" );
            //Debug.Log("ʵ�����" +a + "��");

        }
       
       
        // ����¶��� �� ת��������Ʒ
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

    public void ˢ�±�����(itemType type)
    {
        Debug.Log("��Ϊ����������ˢ�±�����");
        SwitchTab(type);
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

    //���ڸ�������
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

    //��Ʒ��ʹ�ú�ɾ��
    public void Use()
    {
        if (PackageCell._current == null) return;
        switch (PackageCell._current._type)
        {
            case itemType.Food:
                Debug.Log("ʳ��ʹ��");
                //ʵ�ֽ�ɫ��ѪЧ��
                break;

             case itemType.Weapon:
                Debug.Log("��ʹ��");
                PackageInventoryService.Instance.PackageEquipmentWeapon(PackageCell._current);
                break;

                case itemType.Armor:
                Debug.Log("����ʹ��");
                break;
                default:
                Debug.Log("��ͼʹ��δ֪��Ʒ");
                break;
        }
      
    }
    public void Delete()
    {
        if (PackageCell._current == null) return;

        PackageLocalItem temp = PackageInventoryService.Instance.��ID�õ�������Ʒ������(PackageCell._current.ID);
        // temp.count=objNum;
        if(temp==null) return;
       // Debug.Log("ɾ������Ʒ" + temp.uid);
        PackageInventoryService.Instance.RemoveItem(temp);

    }




    //�����¼�
    private void OnDestroy()
    {
        PackageInventoryService.Instance.ˢ���¼� -= ˢ�±�����;
       
    }


}
