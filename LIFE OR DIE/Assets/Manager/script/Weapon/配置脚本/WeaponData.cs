using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "装备静态数据/武器", fileName = "武器数据")]
public class WeaponData : ScriptableObject
{
    //[SerializeField] public int ID;
    [SerializeField] public int NumberOfAttacks;
    [SerializeField] public string WeaponName;
    [SerializeField] public Sprite PickUpSprit;
    [SerializeField] public Animator WeaponAnimator;
   
    // public List<int> test;
    //关于序列化  经验上最外层的需要支持多序列化 其余的子类元素只需要[System.Serializable]套在类外即可
    //太多会导致序列化混乱
    [field: SerializeReference] public List<ComponentData> componentDatas { get; private set; } = new List<ComponentData>();
    public void GetData<t>()
    {

    }

    [ContextMenu(itemName:"添加武器图片数据")]
    private void AddSpriteData() { componentDatas.Add(new WeaponSpriteData()); }
    
}
