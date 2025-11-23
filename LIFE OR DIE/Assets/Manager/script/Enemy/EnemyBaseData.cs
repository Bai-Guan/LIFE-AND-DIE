using UnityEngine;
[CreateAssetMenu(menuName ="敌人SO数据",fileName ="基础数据")]
public class EnemyBaseData : ScriptableObject
{
    [Header("敌人名字")]
    [SerializeField] public string Name;

    [Header("敌人最大生命")]
    [SerializeField] public int MaxHP;

    [Header("敌人防御值")]
    [SerializeField] public int Defense;

    [Header("敌人抗击退值,最大100")]
    [Range(0, 100)]
    [SerializeField] public int KnockBackResistance;

    [Header("敌人韧性值")]
    [SerializeField] public int ToughnessValue;

}
