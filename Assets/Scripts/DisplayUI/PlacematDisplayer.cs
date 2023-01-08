using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

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
        registerDelegates(true);
    }
    private void OnDisable()
    {
        registerDelegates(false);
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
