using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PlayerControlUI : MonoBehaviour, IPointerClickHandler
{
    protected Player player { get; private set; }
    protected Game game { get; private set; }

    public void setPlayer(Player player, Game game)
    {
        this.player = player;
        this.game = game;
    }

    public abstract void activate();
    public virtual void activate(PointerEventData eventData) { }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        activate(eventData);
    }
}
