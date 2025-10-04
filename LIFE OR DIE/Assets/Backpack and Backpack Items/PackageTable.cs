using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Weapons, Armor, Food



[CreateAssetMenu(menuName ="��̬����/������̬����",fileName ="packageTable")]
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
    public int type;
    public string name;
    public string description;

    public string skillDescript;
    public string imagePath;

    public int num;
}