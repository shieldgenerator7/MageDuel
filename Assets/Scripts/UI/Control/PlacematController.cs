using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class PlacematController : MonoBehaviour
{
    private List<PlayerControlUI> controls = new List<PlayerControlUI>();

    private Player player;
    private UIVariables uiVars;

    private void Start()
    {
        refreshChildren();
    }

    public void setPlayer(Player player, UIVariables uiVars)
    {
        this.player = player;
        this.uiVars = uiVars;
        controls.ForEach(c => c.setPlayer(player, uiVars));
    }

    public void refreshChildren()
    {
        controls = GetComponentsInChildren<PlayerControlUI>().ToList();
        setPlayer(player, uiVars);
    }
}
