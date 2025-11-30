using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 动画播放 : Action
{
  private  小怪动画事件 anim;
    public SharedString 动画名称;

    public override void OnAwake()
    {
        anim = GetComponent<小怪动画事件>();
    }
    public override void OnStart()
    {
     

    }

    public override TaskStatus OnUpdate()
    {
       anim.Play(动画名称.Value);
        return TaskStatus.Success;
    }

}
