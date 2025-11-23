using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy/攻击框SO数据")]
public class EnemyHitBoxSO : ScriptableObject
{
   [SerializeField] public List<EHBData> hitBoxes;
}
[System.Serializable]
public class EHBData
{
    [Tooltip("本地坐标下的矩形判定框，支持多段")]
    public Rect hitBoxes;

    [Tooltip("攻击种类")]
    public DamageType damageType;

    [Tooltip("持续多少帧")]
    public int duration = 1;

    [Tooltip("招式名称")]
    public string attackName;

    [Tooltip("是否开启可视化检测")]
    public bool isDebug = false;
}