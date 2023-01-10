using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[DisallowMultipleComponent]
public class PlacematDisplayer : MonoBehaviour
{

    public List<PlayerDisplayUI> uiElements;

    private Player player;
    private Game game;

    public void setPlayer(Player player, Game game)
    {
        //Game
        this.game = game;
        uiElements.ForEach(ui =>
        {
            ui.setGame(game);
        });
        //
        registerUIChanges();
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
        registerUIChanges();
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

    private void registerUIChanges()
    {
        uiElements.ForEach(ui => registerUIChange(ui));
    }
    private void registerUIChange(PlayerDisplayUI playerDisplayUI)
    {
        playerDisplayUI.onDisplayerCreated -= onUICreated;
        playerDisplayUI.onDisplayerCreated += onUICreated;
        playerDisplayUI.onDisplayerDestroyed -= onUIDestroyed;
        playerDisplayUI.onDisplayerDestroyed += onUIDestroyed;
    }

    private void onUICreated(PlayerDisplayUI playerDisplayUI)
    {
        if (!uiElements.Contains(playerDisplayUI))
        {
            uiElements.Add(playerDisplayUI);
        }
        registerUIChange(playerDisplayUI);
        if (player)
        {
            playerDisplayUI.registerDelegates(player, true);
        }
    }
    private void onUIDestroyed(PlayerDisplayUI playerDisplayUI)
    {
        uiElements.Remove(playerDisplayUI);
        if (player)
        {
            playerDisplayUI.registerDelegates(player, false);
        }
    }
}
