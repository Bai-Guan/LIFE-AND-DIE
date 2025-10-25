using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "װ����̬����/����", fileName = "��������")]
public class WeaponData : ScriptableObject
{
    //[SerializeField] public int ID;
    [SerializeField] public int NumberOfAttacks;
    [SerializeField] public string WeaponName;
    [SerializeField] public Sprite PickUpSprit;
    [Header("����������")]
    [SerializeField] public RuntimeAnimatorController BaseAnimator;
   
    // public List<int> test;
    //�������л�  ��������������Ҫ֧�ֶ����л� ���������Ԫ��ֻ��Ҫ[System.Serializable]�������⼴��
    //̫��ᵼ�����л�����
    [field: SerializeReference] public List<ComponentData> componentDatas { get; private set; } = new List<ComponentData>();
    public void GetData<t>()
    {

    }
   public void AddData(ComponentData data)
    {
        if(componentDatas.FirstOrDefault(i=>i.GetType() == data.GetType())!=null)return;
     
        
        componentDatas.Add(data);
    }





    
        
    

}
