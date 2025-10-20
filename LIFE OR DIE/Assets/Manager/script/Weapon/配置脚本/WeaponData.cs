using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "װ����̬����/����", fileName = "��������")]
public class WeaponData : ScriptableObject
{
    //[SerializeField] public int ID;
    [SerializeField] public int NumberOfAttacks;
    [SerializeField] public string WeaponName;
    [SerializeField] public Sprite PickUpSprit;
    [SerializeField] public Animator WeaponAnimator;
   
    // public List<int> test;
    //�������л�  ��������������Ҫ֧�ֶ����л� ���������Ԫ��ֻ��Ҫ[System.Serializable]�������⼴��
    //̫��ᵼ�����л�����
    [field: SerializeReference] public List<ComponentData> componentDatas { get; private set; } = new List<ComponentData>();
    public void GetData<t>()
    {

    }

    [ContextMenu(itemName:"�������ͼƬ����")]
    private void AddSpriteData() { componentDatas.Add(new WeaponSpriteData()); }
    
}
