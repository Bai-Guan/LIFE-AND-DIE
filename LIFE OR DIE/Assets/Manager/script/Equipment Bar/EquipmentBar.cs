using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBar
{
    
    private static EquipmentBar _instance;
    public static EquipmentBar Instance {
        get {
            if (_instance == null)
            {
                _instance = new EquipmentBar();
            }
            return _instance;
        }

    }
    EquipmentBar()
    {
        InitDic();
    }

    private bool isInitDic = false;
    private PackageLocalItem _weapon;
    public PackageLocalItem Weapon {  set => _weapon = value; get { return _weapon; } }
    private PackageLocalItem _armor;
    public PackageLocalItem Armor {  set { _armor = value; } get { return _armor; } }

    private GameObject Player;

    private Dictionary<int, WeaponData> _DicIDtoWeaponData = new Dictionary<int, WeaponData>();
    private IDToDataSO _dataSO;

    public  event Action<itemType,Sprite> WeaponEquipmentEvent;





    private void InitDic()
    {
      _dataSO =  Resources.Load("OS/武器ID到数据集合") as IDToDataSO;
        if (_dataSO == null)
        {
            Debug.LogError("[EquipmentBar] 找不到 ID→Weapon 数据资产");
            return;
        }

        foreach (idToData i in _dataSO.ListIDToData)
        {
            _DicIDtoWeaponData.Add(i.id, i.weaponData);
        }
        isInitDic = true;
    }

    public WeaponData FromIDToWeaponData( int id)
    {
       if(_DicIDtoWeaponData == null) {Debug.LogWarning("未在ID转数据的SO文件中找到对应数据"); return null; }
        return _DicIDtoWeaponData[id];
    }

    public void EquipTheWeapon(PackageLocalItem weapon)
    {
        if (!isInitDic) { Debug.LogError("EquipmentBar中id 武器数据字典未初始化");return; }

        if(weapon.type!=itemType.Weapon)
        { Debug.Log("尝试在武器栏装备非武器物品");return; }
        //更新装备栏武器
        _weapon = weapon;
        //发送广播 更新UI武器栏显示
        WeaponEquipmentEvent?.Invoke(_weapon.type,PackageInventoryService.Instance.FromIDToSprite(_weapon.id));
        //发送广播 更新背包数据

        //发送广播 为武器脚本更新数据
        if (Player == null)
        {
            Player = GameObject.Find("MainPlayer");
            if (Player == null)
            {
                Debug.LogWarning("装备栏找不到玩家！");
                return;
            }
        }
        Player.transform.Find("Weapon").GetComponent<InitWeaponSystem>().UpdateWeaponData(FromIDToWeaponData(weapon.id));
        Debug.Log("已装备武器:" + weapon.uid);
        
    }

    //private WeaponData FromIDToWeaponData(int id)
    //{

    //}
 

}
