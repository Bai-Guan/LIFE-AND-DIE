using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMoveData : ComponentData
{
   [SerializeField] public OneAttackMoveData[] AttackMoveDatas;
}
[System.Serializable]
public class OneAttackMoveData
{
    [SerializeField] public  float MoveX;
    [SerializeField] public   float MoveY;
    [SerializeField] public   float SpeedMove;
    
}