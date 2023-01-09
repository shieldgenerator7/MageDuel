using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlacematController))]
[RequireComponent(typeof(PlacematDisplayer))]
public class Placemat : MonoBehaviour
{
    private Player player;

    private PlacematController placematController;
    private PlacematDisplayer placematDisplayer;

    // Start is called before the first frame update
    public void setPlayer(Player player)
    {
        this.player = player;
        //
        placematController ??= GetComponent<PlacematController>();
        placematController.setPlayer(player);
        //
        placematDisplayer ??= GetComponent<PlacematDisplayer>();
        placematDisplayer.setPlayer(player);
    }
}
