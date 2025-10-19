using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "����ID�����ݼ���", fileName = "����ID�����ݼ���")]
public class IDToDataSO :ScriptableObject
{

  [SerializeField] public  List<idToData> ListIDToData = new List<idToData>();

}
[System.Serializable]
public class idToData
{
    public int id;
    public WeaponData weaponData;
}