using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPlayerController : MonoBehaviour
{
    public float cooldown = 1;

    private bool canActivate = true;

    public void activate()
    {
        if (canActivate)
        {
            canActivate = false;
            Managers.Timer.startTimer(cooldown, () => canActivate = true);
            onActivated?.Invoke();
        }
    }
    public delegate void OnActivated();
    public event OnActivated onActivated;
}
