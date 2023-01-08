using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[DisallowMultipleComponent]
public class PlacematDisplayer : MonoBehaviour
{

    public List<PlayerDisplayUI> uiElements;

    private Player player;

    public void setPlayer(Player player)
    {
        //Unlink from previous player
        if (this.player)
        {
            registerDelegates(false);
        }
        //Set player
        this.player = player;
        //Link to new player
        if (this.player)
        {
            registerDelegates(true);
        }
    }

    void OnEnable()
    {
        if (this.player)
        {
            registerDelegates(true);
        }
    }
    private void OnDisable()
    {
        if (this.player)
        {
            registerDelegates(false);
        }
    }

    private void registerDelegates(bool register)
    {
        if (register)
        {
            uiElements.ForEach(ui =>
            {
                ui.registerDelegates(player);
                ui.forceUpdate();
            });
        }
        else
        {
            uiElements.ForEach(ui => ui.registerDelegates(player, false));
        }
    }
}
