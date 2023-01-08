using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControlUI : MonoBehaviour
{
    protected Player player;

    public void setPlayer(Player player)
    {
        this.player = player;
    }

    public abstract void activate();
}
