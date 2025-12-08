using  BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 清空僵直条 : Action
{
    private EnemyRigidbar enemyRigidbar;


    public override void OnAwake()
    {
        enemyRigidbar = GetComponent<EnemyRigidbar>();
    }

    public override TaskStatus OnUpdate()
    {
      enemyRigidbar.清空僵直条();
        return TaskStatus.Success;
       
    }
}
