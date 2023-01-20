using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float startTime;
    private float duration;
    private bool completed = false;
    public bool canceled = false;

    private bool running = true;

    public bool Completed => completed;

    public Timer(float startTime, float duration)
    {
        this.startTime = startTime;
        this.duration = duration;
        this.completed = false;
        this.running = true;
    }
    public void update(float time)
    {
        if (completed)
        {
            return;
        }
        if (canceled)
        {
            return;
        }
        if (!running)
        {
            return;
        }
        float percent = (time - startTime) / duration;
        percent = Mathf.Clamp(percent, 0, 1);
        onTimerProgressPercent?.Invoke(percent);
        if (time >= startTime + duration)
        {
            completed = true;
            onTimerFinished?.Invoke();
        }
    }
    public delegate void OnTimerFinished();
    public event OnTimerFinished onTimerFinished;
    public delegate void OnTimerProgressPercent(float percent);
    public event OnTimerProgressPercent onTimerProgressPercent;

    public void stop()
    {
        running = false;
    }
    public void reset()
    {
        running = true;
        startTime = Time.time;
    }
}
