using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlacematDisplayer))]
public class Placemat : MonoBehaviour
{
    public string playerName = "Player1";
    public Player player;

    private PlacematDisplayer placematDisplayer;

    // Start is called before the first frame update
    void Start()
    {
        player = new Player(playerName);
        //
        placematDisplayer = GetComponent<PlacematDisplayer>();
        placematDisplayer.setPlayer(player);
    }
}
