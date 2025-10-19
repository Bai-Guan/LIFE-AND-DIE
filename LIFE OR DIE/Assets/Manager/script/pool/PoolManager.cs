using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
   private static PoolManager _instance;
    public static PoolManager Instance
    {
        get {
          
            return _instance; 
        }
    }
    public GameObject packageCellPrefab;

    static Transform UIPoolsFood;
     static Transform UIPoolsWeapon;
     static Transform UIPoolsArmor;

    public static List<GameObject> UIPackagePools_Food = new List<GameObject>();
    public static List<GameObject> UIPackagePools_Weapon = new List<GameObject>();
    public static List<GameObject> UIPackagePools_Armor = new List<GameObject>();
    Dictionary<itemType, List<GameObject>> _DicUIPackagePool = new Dictionary<itemType, List<GameObject>>()
    {
        { itemType.Food,UIPackagePools_Food},
        {itemType.Weapon,UIPackagePools_Weapon},
        {itemType.Armor,UIPackagePools_Armor},
    };
    Dictionary<itemType, Transform> _DicUIRoot = new Dictionary<itemType, Transform>() { 
        {itemType.Food ,UIPoolsFood},
        {itemType.Armor,UIPoolsArmor },
        { itemType.Weapon,UIPoolsWeapon}
    };

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        // 初始化对象池根节点
        InitializePoolRoots();
    }

    private void InitializePoolRoots()
    {
        // 创建对象池的根节点
        UIPoolsFood = new GameObject("FoodPool").transform;
        UIPoolsFood.SetParent(this.transform);
        UIPoolsWeapon = new GameObject("WeaponPool").transform;
        UIPoolsWeapon.SetParent(this.transform);
        UIPoolsArmor = new GameObject("ArmorPool").transform;
        UIPoolsArmor.SetParent(this.transform);

        // 更新字典引用
        _DicUIRoot[itemType.Food] = UIPoolsFood;
        _DicUIRoot[itemType.Weapon] = UIPoolsWeapon;
        _DicUIRoot[itemType.Armor] = UIPoolsArmor;
    }
    //只移一个物品
    public void UISpan(PackageCell obj, Transform father,itemType type)
    {
      GameObject temp =  _DicUIPackagePool[type].Find(x=>x.gameObject.name== obj.name);
        if(temp==null)
        {
            GameObject e = Instantiate(packageCellPrefab);
            _DicUIPackagePool[type].Add(e);
            e.transform.SetParent(father, false);

            PackageCell temp2 = e.GetComponent<PackageCell>();
            temp2.Set(obj);

            e.SetActive(true);
            return;
        }
        temp.transform.SetParent(father, false);
        temp.SetActive(true);
     
    }

        public void UIRecycleCell(PackageCell obj,  itemType type)
        {
        obj.transform.SetParent(_DicUIRoot[type],false);
        obj.gameObject.SetActive(false);
        }

    //用于ui销毁
    public void UIListRomove(GameObject obj, itemType type)
    {
        GameObject temp = _DicUIPackagePool[type].Find(x => x.gameObject.name == obj.name);
        if (temp == null) { Debug.LogWarning("要移除的物品在对象池的list列表中不存在！");return; }
        _DicUIPackagePool[type].Remove(temp);
    }
    public void UISpanItem(int id, int num, string name, Sprite sprite, Transform father, itemType type)
    {
        GameObject temp = _DicUIPackagePool[type].Find(x => x != null && !x.activeInHierarchy);


        if (temp == null)
        {
            temp = Instantiate(packageCellPrefab);
            _DicUIPackagePool[type].Add(temp);
            temp.transform.SetParent(_DicUIRoot[type], false);
        }

        temp.transform.SetParent(father, false);

        PackageCell cell = temp.GetComponent<PackageCell>();
        if (cell != null && sprite != null)
        {
            cell.Set(id, num, name, sprite);
        }
        else
        {
            Debug.LogWarning($"创建 PackageCell 失败: cell={cell}, sprite={sprite}");
        }

        temp.SetActive(true);
    }
}
