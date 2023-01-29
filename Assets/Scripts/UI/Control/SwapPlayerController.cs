using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPlayerController : MonoBehaviour
{
    public void activate()
    {
        onActivated?.Invoke();
    }
    public delegate void OnActivated();
    public event OnActivated onActivated;
}
