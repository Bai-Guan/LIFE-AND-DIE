using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRigidbar : MonoBehaviour
{
    [SerializeField] private float maxRigid = 2;
    public float MaxRigid {  get { return maxRigid; } }
    [SerializeField] private float RigidTime = 1.0f;
    public float currentRigidValue = 0;
    [SerializeField] InitEnemySystem body;
    private void Awake()
    {
        body = GetComponent<InitEnemySystem>();
        body.BeAttack += BeHurt;
    }

    private void BeHurt()
    {
        currentRigidValue += 1;
     
    }
    public void 清空僵直条()
    {
        currentRigidValue = 0;
    }
    public void 加减僵直条(float value)
    {
        currentRigidValue += value;
        if(currentRigidValue > maxRigid) currentRigidValue = maxRigid;
        if(currentRigidValue < 0) currentRigidValue=0;
    }
    public bool 检测是否僵直()
    {
        if(currentRigidValue >= maxRigid) return true;
        else return false;
    }
  
    private void OnDestroy()
    {
        body.BeAttack -= BeHurt;
    }
}
