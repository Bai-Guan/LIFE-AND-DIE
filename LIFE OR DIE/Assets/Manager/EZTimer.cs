using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZTimer
{
    //�򵥵ļ�ʱ��
    public event Action OnTimerDone;
    private float startTime;
    private float duration;
    private float targetTime;
    private bool isActive;

    public EZTimer(float durationTime)
    {
        this.duration = durationTime;
    }
    public void StartTimer()
    {
      
        startTime = Time.time;
        targetTime = startTime + duration;
        isActive = true;
    }

    public void StopTime()
    {
        isActive = false;
    }
    public void Tick()
    {
        if (!isActive) return;

        if (Time.time >= targetTime)
        {
            OnTimerDone?.Invoke();
            StopTime();
        }
    }


}
