using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlacematController))]
[RequireComponent(typeof(PlacematDisplayer))]
public class Placemat : MonoBehaviour
{
    private Player player;
    private Game game;

    private PlacematController placematController;
    private PlacematDisplayer placematDisplayer;

    // Start is called before the first frame update
    public void setPlayer(Player player, Game game)
    {
        this.player = player;
        this.game = game;
        //
        placematController ??= GetComponent<PlacematController>();
        placematController.setPlayer(player, game);
        //
        placematDisplayer ??= GetComponent<PlacematDisplayer>();
        placematDisplayer.setPlayer(player, game);
    }
}
