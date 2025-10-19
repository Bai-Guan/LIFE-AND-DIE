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
    public PackageLocalItem Weapon { private set { } get { return _weapon; } }
    private PackageLocalItem _armor;
    public PackageLocalItem Armor { private set { } get { return _armor; } }

    private GameObject Player;

    private Dictionary<int, WeaponData> _DicIDtoWeaponData = new Dictionary<int, WeaponData>();
    private IDToDataSO _dataSO;
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
       
        return _DicIDtoWeaponData[id];
    }

    public void EquipTheWeapon(PackageLocalItem weapon)
    {
        if (!isInitDic) { Debug.LogError("EquipmentBar��id ���������ֵ�δ��ʼ��");return; }

        if(weapon.type!=itemType.Weapon)
        { Debug.Log("������������װ����������Ʒ");return; }
        _weapon = weapon;
        //���͹㲥 ����UI������

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
