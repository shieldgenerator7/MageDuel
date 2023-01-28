using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class PlayerDisplayUI : MonoBehaviour
{
    protected Player player { get; private set; }
    protected UIVariables uiVars { get; private set; }
    public Player Player => player;
    public UIVariables UIVars => uiVars;

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

    public void setUIVars(UIVariables uiVars)
    {
        this.uiVars = uiVars;
    }

    public void registerDelegates(Player player, bool register = true)
    {
        _registerDelegates(false);
        this.player = null;
        if (register)
        {
            this.player = player;
            _registerDelegates(true);
        }
    }
    protected abstract void _registerDelegates(bool register);

    public abstract void forceUpdate();


    protected void callOnDisplayerCreated(PlayerDisplayUI playerDisplayUI)
    {
        onDisplayerCreated?.Invoke(playerDisplayUI);
    }
    protected void callOnDisplayerDestroyed(PlayerDisplayUI playerDisplayUI)
    {
        onDisplayerDestroyed?.Invoke(playerDisplayUI);
    }
    public delegate void OnDisplayerChanged(PlayerDisplayUI playerDisplayUI);
    public event OnDisplayerChanged onDisplayerCreated;
    public event OnDisplayerChanged onDisplayerDestroyed;

}
