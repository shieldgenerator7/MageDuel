using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlacematController : MonoBehaviour
{
    public List<PlayerControlUI> controls;

    private Player player;
    private Game game;

    public void setPlayer(Player player, Game game)
    {
        this.player = player;
        this.game = game;
        controls.ForEach(c => c.setPlayer(player, game));
    }
}
