using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    public event Action OnFinish;

    public event Action OneAttack;

    private void AnimationFinishedTrigger() {print("正在广播：集体开始退出" ); OnFinish?.Invoke(); }

    private void AnimationAttackFinished() =>OneAttack?.Invoke();
    
}
