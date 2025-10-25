using UnityEngine;
[CreateAssetMenu(menuName ="����SO����",fileName ="��������")]
public class EnemyBaseData : ScriptableObject
{
    [Header("�����������")]
    [SerializeField] public int MaxHP;

    [Header("���˷���ֵ")]
    [SerializeField] public int Defense;

    [Header("���˿�����ֵ,���100")]
    [Range(0, 100)]
    [SerializeField] public int KnockBackResistance;

    [Header("��������ֵ")]
    [SerializeField] public int ToughnessValue;

}
