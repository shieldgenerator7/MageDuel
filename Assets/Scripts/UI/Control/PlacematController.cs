using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlacematController : MonoBehaviour
{
    public List<PlayerControlUI> controls;

    private Player player;
    private UIVariables uiVars;

    public void setPlayer(Player player, UIVariables uiVars)
    {
        this.player = player;
        this.uiVars = uiVars;
        controls.ForEach(c => c.setPlayer(player, uiVars));
    }
}
