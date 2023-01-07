using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlacematDisplayer : MonoBehaviour
{
    public string playerName = "Player1";
    public Player player;

    public List<PlayerDisplayUI> uiElements;

    void OnEnable()
    {
        player = new Player(playerName);
        uiElements.ForEach(ui =>
        {
            ui.registerDelegates(player);
            ui.forceUpdate();
        });
    }
    private void OnDisable()
    {
        uiElements.ForEach(ui => ui.registerDelegates(player, false));
    }
}
