using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 检测是否应该僵直 : Conditional
{
    
   private EnemyRigidbar enemyRigidbar;
    

    public override void OnAwake()
    {
        enemyRigidbar = GetComponent<EnemyRigidbar>();
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyRigidbar.检测是否僵直())
        { 
            
          
        return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
