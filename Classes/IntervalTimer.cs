using System;
using ImprovedTimers;
using UnityEngine;

public class IntervalTimer : Timer
{
    private readonly float interval;
    private float nextInterval;

    public Action OnInterval = delegate { };

    public IntervalTimer(float totalTime, float intervalSeconds) : base(totalTime)
    {
        interval = intervalSeconds;
        nextInterval = totalTime - interval;
    }
    
    public override void Tick()
    {
        if(IsRunning && CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;
            
            while(CurrentTime <= nextInterval && nextInterval >= 0)
            {
                OnInterval?.Invoke();
                nextInterval -= interval;
            }
        }

        if (!IsRunning || !(CurrentTime <= 0)) return;
        
        CurrentTime = 0;
        Stop();
    }

    public override bool IsFinished => CurrentTime <= 0;
}
