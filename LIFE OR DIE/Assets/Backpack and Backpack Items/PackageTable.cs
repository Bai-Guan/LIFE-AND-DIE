using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Weapons, Armor, Food



[CreateAssetMenu(menuName ="��̬����/��̬���ݼ�",fileName ="packageTable")]
public class PackageTable : ScriptableObject
{
    public const int WEAPON = 1;
    public const int ARMOR = 2;
    public const int FOOD = 3;
   public List<PackageTableItem> DataList = new List<PackageTableItem>();

}
//���������¼ ���ж�����Ʒ����ʲô������Ʒ ��Ŀǰ�� ���� ���� ʳ�� 3��)
//�Լ���¼װ��������ʲôװ��

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