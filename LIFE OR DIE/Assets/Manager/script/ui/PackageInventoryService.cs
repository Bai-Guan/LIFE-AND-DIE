using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using static UnityEditor.Progress;

public class PackageInventoryService 
{
    private static PackageInventoryService _instance;
    public static PackageInventoryService Instance
    {
        get
        {
            if (_instance == null) _instance = new PackageInventoryService();
            return _instance;
        }
    }

    private PackageLocalData _localData;

    private Dictionary<itemType, List<PackageLocalItem>> DicListPackageItem;

    private PackageTable packageTable;
    private string path = "TableData/packageTable";
    public Dictionary<int, PackageTableItem> _itemDataCache;


    //��ʼ������ ������
    public void InitPackage()
    {
        //���ؾ�̬����
        packageTable =Resources.Load<PackageTable>(path) as PackageTable;
        if (packageTable == null) { Debug.LogError("��̬��Ʒ���ݼ���ʧ�ܣ�"); return; }
        //��ʼ��id�ֵ�
        _itemDataCache = new Dictionary<int, PackageTableItem>();
        foreach (PackageTableItem item in packageTable.DataList)
        {
            _itemDataCache[item.id] = item;
        }

        //��ʼ���ض�̬���ݣ��������ݣ�
        _localData = new PackageLocalData();
        Load();

        //��ʼ����������
        DicListPackageItem=new Dictionary<itemType, List<PackageLocalItem>>();
        foreach (itemType type in Enum.GetValues(typeof(itemType)))
        {
            
            DicListPackageItem[type] = new List<PackageLocalItem>();
        }
        if (_localData.saveData.localAllItems.Count <= 0)
        {
            Debug.Log("�ֿ�û����Ʒ��");
            Debug.Log("������ʼ�����");
            return;
        }
       BuildItemDictionaryFromLocalData();
        Debug.Log("������ʼ�����");
    }
    public PackageLocalItem GetItemById(int id)
    {
        PackageLocalItem item = new PackageLocalItem();
        item.id= _itemDataCache[id].id;
        item.uid = _itemDataCache[id].name;
        item.type = _itemDataCache[id].type;
        item.count = 1;
        return item;

    }


    //���Ϊˮ�� ��������1  ���Ϊ����������������������ui��ʾʱ���ռһ��
    //����10��ˮ��������ͬһ��ui������ ������װ�����ɵ���
    private void BuildItemDictionaryFromLocalData()
    {
        // ��������ֵ�����
        foreach (var list in DicListPackageItem.Values)
        {
            list.Clear();
        }

        // �����ͷ��鴦��
        var groupedByType = _localData.saveData.localAllItems.GroupBy(item => item.type);

        foreach (var group in groupedByType)
        {
            itemType type = group.Key;

            if (type == itemType.Food)
            {
                // �ɵ�����Ʒ����ID�ϲ�����
                var foodGroups = group.GroupBy(item => item.id);
                foreach (var foodGroup in foodGroups)
                {
                    int totalCount = foodGroup.Sum(item => item.count);
                    PackageLocalItem mergedItem = new PackageLocalItem
                    {
                        id = foodGroup.Key,
                        uid = foodGroup.First().uid,
                        type = type,
                        count = totalCount
                    };
                    DicListPackageItem[type].Add(mergedItem);
                }
            }
            else
            {
                // ���ɵ�����Ʒ��ֱ�����
                foreach (var item in group)
                {
                    DicListPackageItem[type].Add(item);
                }
            }
        }
    }

    // �޸�AddItem������ֻ������Ϸ����ʱ�������Ʒ
    public void AddItem(PackageLocalItem newItem)
    {
        // ��ӵ��������б�
        if (newItem.type == itemType.Food)
        {
            var existingItem = _localData.saveData.localAllItems.FirstOrDefault(x => x.id == newItem.id);
            if (existingItem != null)
            {
                existingItem.count += newItem.count;
            }
            else
            {
                _localData.saveData.localAllItems.Add(newItem);
            }
        }
        else
        {
            _localData.saveData.localAllItems.Add(newItem);
        }

        // ���·����ֵ�
        if (newItem.IsStackable)
        {
            DicListPackageItem[newItem.type].FirstOrDefault(x => x.id == newItem.id).count += 1;
        }
        else
        {
            DicListPackageItem[newItem.type].Add(newItem);
        }
    }
    //һ�����ȴ��ڲ����Ƴ�
    public void RemoveItem(PackageLocalItem Removeitem) 
    {
        if(Removeitem==null) return;
        if(Removeitem.type == itemType.Food)
        {
            var temp = _localData.saveData.localAllItems.FirstOrDefault(x => x.id == Removeitem.id);
            if (temp != null)
            {
                temp.count -= 1;
                if (temp.count <= 0)
                {

                    _localData.saveData.localAllItems.Remove(Removeitem);
                }
            }
        }
        else
        {
            _localData.saveData.localAllItems.Remove(Removeitem);
        }
       

        if (Removeitem.type == itemType.Food)
        {
           RemoveStackableItem(Removeitem);
        }
        else
        {
            RemoveNonStackableItem(Removeitem);
        }

    }

    private void RemoveNonStackableItem(PackageLocalItem removeitem)
    {
      var temp =  DicListPackageItem[removeitem.type].FirstOrDefault(x=>x.id == removeitem.id);
        if (temp != null)
        {
            temp.count -= 1;
            if (temp.count <= 0)
            {
                
                DicListPackageItem[removeitem.type].Remove(removeitem);
            }
        }
        else { Debug.LogWarning("��ͼ�Ƴ�һ�������壡"); }
      
    }

    private void RemoveStackableItem(PackageLocalItem removeitem)
    {
    
        DicListPackageItem[removeitem.type].Remove(removeitem);
    }

    public List<PackageLocalItem> GetDicList(itemType type)
    {
        return DicListPackageItem[type];
    }

    public void Save()
    {
        _localData.savePackage();
    }
    public void Load()
    {
        _localData.LoadPackage();
    }
    //�˷������ɶ���ҵ�ǰ�������ݵ�������ɾ��
    //����Ŀǰ����Ϊ�������ݴ浵

    //-------------------------------Debug--------------------------------
    public void DebugLogItem()
    {
        foreach (var item in _localData.saveData.localAllItems)
            Debug.Log(item.ToString());
    }
    public void DebugLogFoodItem()
    {
        foreach (var item in DicListPackageItem[itemType.Food])
            Debug.Log(item.ToString());
    }
    public void DebugLogWeaponItem()
    {
        foreach (var item in DicListPackageItem[itemType.Weapon])
            Debug.Log(item.ToString());
    }
    public void DebugLogArmorItem()
    {
        foreach (var item in DicListPackageItem[itemType.Armor])
            Debug.Log(item.ToString());
    }
}
