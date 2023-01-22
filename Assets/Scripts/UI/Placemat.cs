using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlacematController))]
[RequireComponent(typeof(PlacematDisplayer))]
public class Placemat : MonoBehaviour
{
    private Player player;
    private UIVariables uiVars;

    private PlacematController placematController;
    private PlacematDisplayer placematDisplayer;

    private void Start()
    {
        placematController ??= GetComponent<PlacematController>();
        placematDisplayer ??= GetComponent<PlacematDisplayer>();
        placematDisplayer.onUIChanged += placematController.refreshChildren;
    }

    // Start is called before the first frame update
    public void setPlayer(Player player, UIVariables uiVars)
    {
        this.player = player;
        this.uiVars = uiVars;
        //
        placematController ??= GetComponent<PlacematController>();
        placematController.setPlayer(player, uiVars);
        //
        placematDisplayer ??= GetComponent<PlacematDisplayer>();
        placematDisplayer.setPlayer(player, uiVars);
    }
}
