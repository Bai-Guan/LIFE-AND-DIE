using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEditorInternal.Profiling.Memory.Experimental;
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

    public event Action<itemType> ˢ���¼�;
    public event Action<int> Packagecell����ˢ��;
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
    public PackageLocalItem GetNewItemById(int id)
    {
        PackageLocalItem item = new PackageLocalItem();
        item.id= _itemDataCache[id].id;
        item.uid = _itemDataCache[id].name;
        item.type = _itemDataCache[id].type;
        item.count = 1;
        return item;

    }
    public PackageLocalItem ��ID�õ�������Ʒ������(int id)
    {
        // ��ʵ�������в�����Ʒ�������Ǵ����¶���
        var item = _localData.saveData.localAllItems.FirstOrDefault(x => x.id == id);
        if (item != null)
        {
            return item;
        }
        else
        {
            // ���������û�и���Ʒ������null�򴴽�һ���µģ�ȡ�����������
            Debug.LogWarning("������û���ҵ� id Ϊ " + id + " ����Ʒ");
            return null;
        }
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

        ˢ���¼�?.Invoke(newItem.type);
    }
    //һ�����ȴ��ڲ����Ƴ�
    public void RemoveItem(PackageLocalItem Removeitem) 
    {
        if(Removeitem==null) return;
        var temp = _localData.saveData.localAllItems.FirstOrDefault(x => x.id == Removeitem.id);
        if (temp == null) {Debug.LogWarning("��ͼ�ڱ����г�ȥ�����ڵ���Ʒ"); return; }
            if (Removeitem.type == itemType.Food)
            {
           
                temp.count -= 1;
            Packagecell����ˢ��?.Invoke(temp.count);
           // PackageCell.CurrentCellObjNumDecrementByOne(temp.count);
                if (temp.count <= 0)
                {

                    _localData.saveData.localAllItems.Remove(temp);
                    DicListPackageItem[temp.type].Remove(temp);
                    

                 }
          
            
             }
        else
        {
            _localData.saveData.localAllItems.Remove(temp);
            DicListPackageItem[temp.type].Remove(temp);
        }

        ˢ���¼�?.Invoke(Removeitem.type);

    }


    public Sprite FromIDToSprite(int id)
    {
        return _itemDataCache[id].itemImage;
    }

    private void RemoveStackableItem(PackageLocalItem removeitem)
    {
    
        DicListPackageItem[removeitem.type].Remove(removeitem);
    }

    public List<PackageLocalItem> GetDicList(itemType type)
    {
        return DicListPackageItem[type];
    }

    //������������Ʒ���������� �ɴ���ĵ�Ԫ���������
    public void PackageEquipmentWeapon(PackageCell weaponCell)
    {
        //����id�ҵ������е�����
        var temp = _localData.saveData.localAllItems.FirstOrDefault(x => x.id == weaponCell.ID);
        if (temp == null) { Debug.LogWarning("���ر�������δ�д�װ��");return; }
        //������Ʒ�ӱ����м���ȥ
        _localData.saveData.localAllItems.Remove(temp);
        DicListPackageItem[temp.type].Remove(temp);
        //���װ�����Ƿ���װ������û����װ������Ʒ
        //����������滻����
        if (EquipmentBar.Instance.Weapon==null)
        {
            //Ϊ����ȥ��Weapon�����µ�λ��->װ������
            EquipmentBar.Instance.EquipTheWeapon(temp);
            
        }
        else
        {
            //�ȵõ����滻����Ʒ���ã�������ӽ� ���ر��� ��
            var temp2 = EquipmentBar.Instance.Weapon;
            EquipmentBar.Instance.Weapon = null;
            _localData.saveData.localAllItems.Add(temp2);

            //��Ϊ����ȥ��Weapon�����µ�λ��->װ������
            EquipmentBar.Instance.EquipTheWeapon(temp);
           
        }
        ˢ���¼�?.Invoke(temp.type);

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
