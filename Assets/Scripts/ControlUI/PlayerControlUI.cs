using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PlayerControlUI : MonoBehaviour, IPointerClickHandler
{
    protected Player player;

    public void setPlayer(Player player)
    {
        this.player = player;
    }

    public abstract void activate();
    public virtual void activate(PointerEventData eventData) { }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        activate(eventData);
    }
}
