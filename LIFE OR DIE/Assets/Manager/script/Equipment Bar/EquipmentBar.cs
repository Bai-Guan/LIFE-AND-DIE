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
      _dataSO =  Resources.Load("OS/����ID�����ݼ���") as IDToDataSO;
        if (_dataSO == null)
        {
            Debug.LogError("[EquipmentBar] �Ҳ��� ID��Weapon �����ʲ�");
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
       if(_DicIDtoWeaponData == null) {Debug.LogWarning("δ��IDת���ݵ�SO�ļ����ҵ���Ӧ����"); return null; }
        return _DicIDtoWeaponData[id];
    }

    public void EquipTheWeapon(PackageLocalItem weapon)
    {
        if (!isInitDic) { Debug.LogError("EquipmentBar��id ���������ֵ�δ��ʼ��");return; }

        if(weapon.type!=itemType.Weapon)
        { Debug.Log("������������װ����������Ʒ");return; }
        //����װ��������
        _weapon = weapon;
        //���͹㲥 ����UI��������ʾ
        WeaponEquipmentEvent?.Invoke(_weapon.type,PackageInventoryService.Instance.FromIDToSprite(_weapon.id));
        //���͹㲥 ���±�������

        //���͹㲥 Ϊ�����ű���������
        if (Player == null)
        {
            Player = GameObject.Find("MainPlayer");
            if (Player == null)
            {
                Debug.LogWarning("װ�����Ҳ�����ң�");
                return;
            }
        }
        Player.transform.Find("Weapon").GetComponent<InitWeaponSystem>().UpdateWeaponData(FromIDToWeaponData(weapon.id));
        Debug.Log("��װ������:" + weapon.uid);
        
    }

    //private WeaponData FromIDToWeaponData(int id)
    //{

    //}
 

}
