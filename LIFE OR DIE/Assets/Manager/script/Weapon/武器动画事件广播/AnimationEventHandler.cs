using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    public event Action OnFinish;

    public event Action OneAttack;

    private void AnimationFinishedTrigger() {print("���ڹ㲥�����忪ʼ�˳�" ); OnFinish?.Invoke(); }

    private void AnimationAttackFinished() =>OneAttack?.Invoke();
    
}
