using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class PlayerDisplayUI : MonoBehaviour
{
    protected Player player;
    public Player Player => player;

    private void OnEnable()
    {
        if (player)
        {
            _registerDelegates(true);
            forceUpdate();
        }
    }

    private void OnDisable()
    {
        if (player)
        {
            _registerDelegates(false);
        }
    }

    public void registerDelegates(Player player, bool register = true)
    {
        if (this.player)
        {
            _registerDelegates(false);
            this.player = null;
        }
        if (register)
        {            
            this.player = player;
            _registerDelegates(true);
        }
    }
    protected abstract void _registerDelegates(bool register);

    public abstract void forceUpdate();
}
