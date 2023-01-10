using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PlayerControlUI : MonoBehaviour, IPointerClickHandler
{
    protected Player player { get; private set; }
    protected UIVariables uiVars { get; private set; }

    public void setPlayer(Player player, UIVariables uiVars)
    {
        this.player = player;
        this.uiVars = uiVars;
    }

    public abstract void activate();
    public virtual void activate(PointerEventData eventData) { }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        activate(eventData);
    }
}
