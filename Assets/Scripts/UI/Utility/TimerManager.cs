using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private List<Timer> timers = new List<Timer>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Timer startTimer(float duration, Timer.OnTimerFinished callback)
    {
        Timer timer = new Timer(Time.time, duration);
        timer.onTimerFinished += callback;
        timers.Add(timer);
        return timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (timers.Count > 0)
        {
            timers.ToList().ForEach(timer => timer.update(Time.time));
            timers.RemoveAll(timer => timer.Completed);
        }
    }
}
