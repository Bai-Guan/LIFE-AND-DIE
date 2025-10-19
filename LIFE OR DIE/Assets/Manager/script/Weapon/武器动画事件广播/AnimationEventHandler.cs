using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    public event Action OnFinish;

    public event Action OneAttackFinished;

    private void AnimationFinishedTrigger()=>OnFinish?.Invoke();

    private void AnimationAttackFinished() =>OneAttackFinished?.Invoke();
    
}
