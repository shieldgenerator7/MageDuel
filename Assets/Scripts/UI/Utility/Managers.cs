using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static TimerManager Timer { get; private set; }
    public static AnimationManager Animation { get; private set; }

    void Awake()
    {
        Timer = FindObjectOfType<TimerManager>();
        Animation = FindObjectOfType<AnimationManager>();
    }
}
