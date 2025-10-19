using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "武器ID到数据集合", fileName = "武器ID到数据集合")]
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