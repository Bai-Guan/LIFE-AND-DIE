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

    public event Action<itemType> 刷新事件;
    public event Action<int> Packagecell数字刷新;
    //初始化背包 并缓存
    public void InitPackage()
    {
        //加载静态数据
        packageTable =Resources.Load<PackageTable>(path) as PackageTable;
        if (packageTable == null) { Debug.LogError("静态物品数据加载失败！"); return; }
        //初始化id字典
        _itemDataCache = new Dictionary<int, PackageTableItem>();
        foreach (PackageTableItem item in packageTable.DataList)
        {
            _itemDataCache[item.id] = item;
        }

        //开始加载动态数据（本地数据）
        _localData = new PackageLocalData();
        Load();

        //初始化本地数据
        DicListPackageItem=new Dictionary<itemType, List<PackageLocalItem>>();
        foreach (itemType type in Enum.GetValues(typeof(itemType)))
        {
            
            DicListPackageItem[type] = new List<PackageLocalItem>();
        }
        if (_localData.saveData.localAllItems.Count <= 0)
        {
            Debug.Log("仓库没有物品！");
            Debug.Log("背包初始化完成");
            return;
        }
       BuildItemDictionaryFromLocalData();
        Debug.Log("背包初始化完成");
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
    public PackageLocalItem 由ID得到背包物品的引用(int id)
    {
        // 从实际数据中查找物品，而不是创建新对象
        var item = _localData.saveData.localAllItems.FirstOrDefault(x => x.id == id);
        if (item != null)
        {
            return item;
        }
        else
        {
            // 如果背包中没有该物品，返回null或创建一个新的（取决于你的需求）
            Debug.LogWarning("背包中没有找到 id 为 " + id + " 的物品");
            return null;
        }
    }


    //如果为水果 则数量加1  如果为护甲武器，则不增加数量在ui显示时候独占一格
    //比如10个水果可以在同一个ui方格内 但武器装备不可叠加
    private void BuildItemDictionaryFromLocalData()
    {
        // 清空现有字典数据
        foreach (var list in DicListPackageItem.Values)
        {
            list.Clear();
        }

        // 按类型分组处理
        var groupedByType = _localData.saveData.localAllItems.GroupBy(item => item.type);

        foreach (var group in groupedByType)
        {
            itemType type = group.Key;

            if (type == itemType.Food)
            {
                // 可叠加物品：按ID合并数量
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
                // 不可叠加物品：直接添加
                foreach (var item in group)
                {
                    DicListPackageItem[type].Add(item);
                }
            }
        }
    }

    // 修改AddItem方法，只用于游戏运行时添加新物品
    public void AddItem(PackageLocalItem newItem)
    {
        // 添加到主数据列表
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

        // 更新分类字典
        if (newItem.IsStackable)
        {
            DicListPackageItem[newItem.type].FirstOrDefault(x => x.id == newItem.id).count += 1;
        }
        else
        {
            DicListPackageItem[newItem.type].Add(newItem);
        }

        刷新事件?.Invoke(newItem.type);
    }
    //一定是先存在才能移除
    public void RemoveItem(PackageLocalItem Removeitem) 
    {
        if(Removeitem==null) return;
        var temp = _localData.saveData.localAllItems.FirstOrDefault(x => x.id == Removeitem.id);
        if (temp == null) {Debug.LogWarning("试图在背包中除去不存在的物品"); return; }
            if (Removeitem.type == itemType.Food)
            {
           
                temp.count -= 1;
            Packagecell数字刷新?.Invoke(temp.count);
           // PackageCell.CurrentCellObjNumDecrementByOne(_isfacingleft.count);
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

        刷新事件?.Invoke(Removeitem.type);

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

    //背包武器与物品栏武器交互 由传入的单元格进行索引
    public void PackageEquipmentWeapon(PackageCell weaponCell)
    {
        //根据id找到背包中的索引
        var temp = _localData.saveData.localAllItems.FirstOrDefault(x => x.id == weaponCell.ID);
        if (temp == null) { Debug.LogWarning("本地背包数据未有此装备");return; }
        //将此物品从背包中挤出去
        _localData.saveData.localAllItems.Remove(temp);
        DicListPackageItem[temp.type].Remove(temp);
        //检测装备栏是否有装备，若没有则装备此物品
        //若有则进行替换操作
        if (EquipmentBar.Instance.Weapon==null)
        {
            //为挤出去的Weapon赋予新的位置->装备栏中
            EquipmentBar.Instance.EquipTheWeapon(temp);
            
        }
        else
        {
            //先得到被替换的物品引用，将其添加进 本地背包 中
            var temp2 = EquipmentBar.Instance.Weapon;
            EquipmentBar.Instance.Weapon = null;
            _localData.saveData.localAllItems.Add(temp2);

            //再为挤出去的Weapon赋予新的位置->装备栏中
            EquipmentBar.Instance.EquipTheWeapon(temp);
           
        }
        刷新事件?.Invoke(temp.type);

    }



    public void Save()
    {
        _localData.savePackage();
    }
    public void Load()
    {
        _localData.LoadPackage();
    }
    //此服务层完成对玩家当前背包数据的增加与删减
    //并且目前运行为背包数据存档

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
