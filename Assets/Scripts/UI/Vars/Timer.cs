using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float startTime;
    private float duration;
    private bool running = false;

    public bool Running => running;

    public Timer(float startTime, float duration)
    {
        this.startTime = startTime;
        this.duration = duration;
        this.running = true;
    }
    public void update(float time)
    {
        if (!running)
        {
            return;
        }
        float percent = (time - startTime) / duration;
        percent = Mathf.Clamp(percent, 0, 1);
        onTimerProgressPercent?.Invoke(percent);
        if (time >= startTime + duration)
        {
            running = false;
            onTimerFinished?.Invoke();
        }
    }
    public delegate void OnTimerFinished();
    public event OnTimerFinished onTimerFinished;
    public delegate void OnTimerProgressPercent(float percent);
    public event OnTimerProgressPercent onTimerProgressPercent;
}
