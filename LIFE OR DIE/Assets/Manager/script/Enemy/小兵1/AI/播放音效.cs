using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class 播放音效 : Action
{
    public SharedString 音频名称;
    public override void OnAwake()
    {

    }
    public override void OnStart()
    {

    }
    public override TaskStatus OnUpdate()
    {
        if(音频名称.Value!=null)
        {
            AudioManager.Instance.PlaySFX(音频名称.Value);
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
       
    }
}
