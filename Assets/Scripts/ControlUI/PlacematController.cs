using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlacematController : MonoBehaviour
{
    public List<PlayerControlUI> controls;

    private Player player;

    public void setPlayer(Player player)
    {
        this.player = player;
        controls.ForEach(c => c.setPlayer(player));
    }
}
