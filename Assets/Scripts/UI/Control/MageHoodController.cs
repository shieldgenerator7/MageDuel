using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHoodController : PlayerControlUI
{
    public override void activate()
    {
        if (uiVars.game.Phase == Game.GamePhase.LINEUP)
        {
            if (player.Lineup.Count > 0)
            {
                player.State = Player.PlayState.CASTING;
            }
        }
    }
}
