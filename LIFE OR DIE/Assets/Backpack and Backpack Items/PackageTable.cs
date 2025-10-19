using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Weapons, Armor, Food



[CreateAssetMenu(menuName ="静态数据/静态数据集",fileName ="packageTable")]
public class PackageTable : ScriptableObject
{
    public const int WEAPON = 1;
    public const int ARMOR = 2;
    public const int FOOD = 3;
   public List<PackageTableItem> DataList = new List<PackageTableItem>();

}
//背包负责记录 里有多少物品，有什么样的物品 （目前有 武器 护甲 食物 3类)
//以及记录装备栏都有什么装备

[System.Serializable]
public class PackageTableItem
{
    public int id;
    public itemType type;
    public string name;
    public string description;
    public string skillDescript;
    public string attribute;

    public int atk;
    public int defense;
    public int hp;
    public SkillName skillName;

    public string imagePath;
    public Sprite itemImage;
    

}

public enum itemType
{
    Weapon =1,
    Armor=2,
    Food =3,
   
}

public enum SkillName
{
    Null,
    FireSwordBurn,
    HealthRegenArmor,
    ForgeSwordsFromFlesh
}